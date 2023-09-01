using MVS.Business;
using MVS.Common;
using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Models;
using MVS.Common.Specifications;
using MVS.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smartclause.SDK;
using System.Net;
using System.Security.Claims;

namespace MVS.Web.Pages.Vault.InfosForms
{
    public class AnticipationMeasuresModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IAnswerService _answerService;
        private readonly IVaultAnswerAnticipationMeasuresService _answerAnticipationMeasuresService;
        private readonly IVaultService _vaultService;
        private readonly IVaultDocumentService _folderDocumentService;
        private readonly IVaultAnticipationMeasuresInfoService _folderAnticipationMeasuresInfoService;
        private readonly IAccessService<Common.Models.Vault> _accessService;

        public string _returnUrl { get; set; }
        public string _folderId { get; set; }
        public Common.Models.Vault _vault { get; set; }
        public int _folderField { get; set; }
        public Dictionary<string, string> _breadcrumb { get; set; }
        public bool _isFutureProtection { get; set; }
        public VaultAnticipationMeasuresInfo _folderAnticipationMeasuresInfo { get; set; }
        private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public List<int> _folderDocumentType { get; set; }
        public int _folderPaymentNumber { get; set; }
        public int _folderAskDeleteNumber { get; set; }

        public AnticipationMeasuresModel(IConfiguration configuration, IAnswerService answerService, IVaultAnswerAnticipationMeasuresService answerAnticipationMeasuresService,
                                            IVaultService vaultService, IVaultAnticipationMeasuresInfoService folderAnticipationMeasuresInfoService, IVaultDocumentService folderDocumentService)
        {
            this._configuration = configuration;
            this._answerService = answerService;
            this._answerAnticipationMeasuresService = answerAnticipationMeasuresService;
            this._vaultService = vaultService;
            this._folderAnticipationMeasuresInfoService = folderAnticipationMeasuresInfoService;
            this._accessService = new AccessService<Common.Models.Vault>(this._configuration);
            this._folderDocumentService = folderDocumentService;
        }

        public async Task OnGetAsync(string vaultId)
        {
            await this._accessService.CheckAccess(vaultId, this._userId, this.User.IsInRole("SuperAdmin"));

            this._folderPaymentNumber = 0;

            List<Common.Models.Vault> folderAskDelete = await this._vaultService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = folderAskDelete.Count;

            this._returnUrl = this.Url.Page("/Vault/Informations", new { vaultId });

            Common.Models.Vault vault = await this._vaultService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));

            this._breadcrumb = new Dictionary<string, string>();
            this._breadcrumb.Add("/Vault", "Mes coffre-forts");
            this._breadcrumb.Add(this.Url.Page("/Vault/Informations", new { vaultId }), $"Vault N°{vault.Title} - Informations");
            this._breadcrumb.Add(this.Url.Page("/Vault/Formulaires/AnticipationMeasures", new { vaultId }), "Mesures d’anticipations");

            List<VaultDocument> folderDocuments = await this._folderDocumentService.Search(new Specification<VaultDocument>(fd => fd.VaultId == vaultId));
            this._folderDocumentType = folderDocuments.Select(fd => fd.Type).ToList();

            /*foreach (Question question in this._questions)
            {
                foreach (PossibleAnswer possibleAnswer in question.PossibleAnswers)
                {
                    if (question.AnswersAnticipationMeasures.FirstOrDefault(a => a.Answer == possibleAnswer.Id) == null)
                    {
                        question.AnswersAnticipationMeasures.Add(new VaultAnswersAnticipationMeasure()
                        {
                            Answer = possibleAnswer.Id,
                            QuestionId = question.Id,
                            VaultId = vaultId,
                            Selected = false,
                        });
                    }
                }
            }*/

            this._folderAnticipationMeasuresInfo = await this._folderAnticipationMeasuresInfoService.Get(new Specification<VaultAnticipationMeasuresInfo>(fami => fami.VaultId == vaultId));
            if (this._folderAnticipationMeasuresInfo == null)
            {
                this._folderAnticipationMeasuresInfo = new VaultAnticipationMeasuresInfo()
                {
                    VaultId = vaultId,
                    CompletedForm = false,
                };
            }
        }

        public async Task<IActionResult> OnPostSaveInfos(VaultAnticipationMeasuresInfo vaultAnticipationMeasuresInfo, List<VaultAnswersAnticipationMeasure> answers)
        {
            try
            {
                if (vaultAnticipationMeasuresInfo.Id == null)
                {
                    vaultAnticipationMeasuresInfo.Id = Guid.NewGuid().ToString();
                    await this._folderAnticipationMeasuresInfoService.Add(vaultAnticipationMeasuresInfo);
                }
                else
                {
                    await this._folderAnticipationMeasuresInfoService.Update(vaultAnticipationMeasuresInfo);
                }

                List<VaultAnswersAnticipationMeasure> answersToDelete = await this._answerAnticipationMeasuresService.Search(new Specification<VaultAnswersAnticipationMeasure>(a => !answers.Select(answer => answer.Id).Contains(a.Id) && a.VaultId == vaultAnticipationMeasuresInfo.VaultId));
                foreach (VaultAnswersAnticipationMeasure answerToDelete in answersToDelete)
                {
                    await this._answerAnticipationMeasuresService.Delete(answerToDelete);
                }
                foreach (VaultAnswersAnticipationMeasure answer in answers)
                {
                    if (answer.Id == 0)
                    {
                        await this._answerAnticipationMeasuresService.Add(answer);
                    }
                    else
                    {
                        await this._answerAnticipationMeasuresService.Update(answer);
                    }
                }

                return this.StatusCode((int)HttpStatusCode.OK, null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<FileContentResult> OnGetDownloadAnticipationMeasuresInfoPdf(string vaultId)
        {
            string title = "Mesures d'anticiption";

            Specification<Common.Models.Vault> spec = new(f => f.Id == vaultId);
            spec.Includes.Add(f => f.VaultAnticipationMeasuresInfos);
            Common.Models.Vault vault = await this._vaultService.Get(spec);

            string questions = "";
            return this.File(VaultInfosHelper.BuildFolderInfosPdf(vault, questions, title), "application/pdf", $"Mesures d'anticiption - {vault.FirstName} {vault.LastName}.pdf");
        }
    }
}
