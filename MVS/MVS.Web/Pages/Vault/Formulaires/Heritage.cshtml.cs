using MVS.Business;
using MVS.Common;
using MVS.Common.Enum;
using MVS.Common.Enum.FolderInfosForms;
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
    public class HeritageModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IVaultCategoryService _categoryService;
        private readonly IJobParticularService _jobParticularService;
        private readonly IAnswerService _answerService;
        private readonly IVaultAnswerHeritageService _answerHeritageService;
        private readonly IVaultService _vaultService;
        private readonly IVaultHeritageService _heritageService;
        private readonly IVaultContractService _folderContractService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Client _client;
        private readonly IAccessService<Common.Models.Vault> _accessService;

        private readonly IVaultContactService _contactService;

        public string _returnUrl { get; set; }
        public string _folderId { get; set; }
        public int _folderField { get; set; }
        public Common.Models.Vault _vault { get; set; }
        public VaultHeritage _vaultHeritage { get; set; }
        public Dictionary<string, string> _breadcrumb { get; set; }
        private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public List<int?> _folderTypeContact { get; set; }

        [BindProperty]
        public List<JobParticular> _jobsParticular { get; set; }
        public int _folderPaymentNumber { get; set; }
        public int _folderAskDeleteNumber { get; set; }

        public HeritageModel(IConfiguration configuration, IVaultCategoryService categoryService,
                                            IAnswerService answerService, IVaultAnswerHeritageService answerHeritageService,
                                            IJobParticularService jobParticularService, IVaultService vaultService,
                                            IVaultContractService folderContractService, Client client, UserManager<ApplicationUser> userManager,
                                            IVaultHeritageService folderHeritageService, IVaultContactService contactService)
        {
            this._configuration = configuration;
            this._categoryService = categoryService;
            this._answerService = answerService;
            this._answerHeritageService = answerHeritageService;
            this._jobParticularService = jobParticularService;
            this._vaultService = vaultService;
            this._folderContractService = folderContractService;
            this._client = client;
            this._userManager = userManager;

            this._heritageService = folderHeritageService;
            this._accessService = new AccessService<Common.Models.Vault>(this._configuration);
            this._contactService = contactService;

        }

        public async Task OnGetAsync(string vaultId)
        {
            await this._accessService.CheckAccess(vaultId, this._userId, this.User.IsInRole("SuperAdmin"));

            List<Common.Models.Vault> folderAskDelete = await this._vaultService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = folderAskDelete.Count;

            this._returnUrl = this.Url.Page("/Vault/Informations", new { vaultId });
            this._folderId = vaultId;
            Common.Models.Vault folder = await this._vaultService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));
            this._vault = await this._vaultService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));

            this._breadcrumb = new Dictionary<string, string>();
            this._breadcrumb.Add("/Vault", "Mes coffre-forts");
            this._breadcrumb.Add(this.Url.Page("/Vault/Informations", new { vaultId }), $"Vault N°{this._vault.Title} - Informations");
            this._breadcrumb.Add(this.Url.Page("/Vault/Formulaires/Heritage", new { vaultId }), "Patrimoine");

            List<Common.Models.VaultContact> folderContact = await this._contactService.Search(new Specification<Common.Models.VaultContact>(fc => fc.VaultId == vaultId));
            this._folderTypeContact = folderContact.Select(fc => fc.TypeContact).ToList();

            this._vaultHeritage = await this._heritageService.Get(new Specification<VaultHeritage>(fh => fh.VaultId == vaultId));
            if (this._vaultHeritage == null)
            {
                this._vaultHeritage = new VaultHeritage()
                {
                    VaultId = vaultId,
                    CompletedForm = false,
                };
            }
        }

        public async Task<FileContentResult> OnGetDownloadHeritageInfoPdf(string vaultId)
        {
            string title = "Patrimoine";

            Specification<Common.Models.Vault> spec = new(f => f.Id == vaultId);
            spec.Includes.Add(f => f.VaultHeritages);
            Common.Models.Vault folder = await this._vaultService.Get(spec);

            string questions = "";
            return this.File(VaultInfosHelper.BuildFolderInfosPdf(folder, questions, title), "application/pdf", $"Patrimoine - {folder.FirstName} {folder.LastName}.pdf");
        }

        /*public async Task<IActionResult> OnPostAsync(VaultHeritage vaultHeritage, List<Answer> answers, List<Answer> answersToDelete)
        {
            if (vaultHeritage.Id == null)
            {
                vaultHeritage.Id = Guid.NewGuid().ToString();
                await this._heritageService.Add(vaultHeritage);
            }
            else
            {
                await this._heritageService.Update(vaultHeritage);
            }

            foreach (Answer answer in answers)
            {
                if (answer.Id == 0)
                {
                    await this._answerService.Add(answer);
                }
                else
                {
                    await this._answerService.Update(answer);
                }
            }

            foreach (Answer answer in answersToDelete)
            {
                await this._answerService.Delete(answer);
            }

            return this.StatusCode((int)HttpStatusCode.OK, null);
        }*/

        public async Task<IActionResult> OnPostSaveInfos(VaultHeritage vaultHeritage, List<VaultAnswersHeritage> answersHeritages, List<VaultAnswersHeritage> answersHeritagesToDelete)
        {
            if (vaultHeritage.Id == null)
            {
                vaultHeritage.Id = Guid.NewGuid().ToString();
                await this._heritageService.Add(vaultHeritage);
            }
            else
            {
                await this._heritageService.Update(vaultHeritage);
            }

            foreach (VaultAnswersHeritage answerHeritage in answersHeritages)
            {
                if (answerHeritage.Id == 0)
                {
                    try
                    {
                        await this._answerHeritageService.Add(answerHeritage);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                else
                {
                    await this._answerHeritageService.Update(answerHeritage);
                }
            }

            foreach (VaultAnswersHeritage answerHeritage in answersHeritagesToDelete)
            {
                await this._answerHeritageService.Delete(answerHeritage);
            }

            return this.StatusCode((int)HttpStatusCode.OK, null);
        }
    }
}
