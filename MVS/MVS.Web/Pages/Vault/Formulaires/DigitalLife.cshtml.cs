using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVS.Business;
using MVS.Common.Interfaces;
using MVS.Common.Models;
using MVS.Common.Specifications;
using MVS.Common;
using MVS.Web.Helpers;
using Smartclause.SDK;
using System.Net;
using System.Security.Claims;
using static NuGet.Packaging.PackagingConstants;

namespace MVS.Web.Pages.Vault.Formulaires
{
    public class DigitalLifeModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IVaultService _vaultService;
        private readonly IVaultDigitalLifeService _digitalLifeService;
        private readonly IVaultAnswerDigitalLifeService _answerDigitalLifeService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Client _client;
        private readonly IAccessService<Common.Models.Vault> _accessService;

        /*private readonly IVaultContactService _contactService;*/

        public string _returnUrl { get; set; }
        public string _vaultId { get; set; }
        public Common.Models.Vault _vault { get; set; }
        public VaultDigitalLife _vaultDigitalLife{ get; set; }
        public Dictionary<string, string> _breadcrumb { get; set; }
        private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public int _folderAskDeleteNumber { get; set; }
        public List<VaultAnswersDigitalLife> _answerDigitalLife { get; set; }

        public DigitalLifeModel(IConfiguration configuration, IVaultService vaultService, Client client,
                                UserManager<ApplicationUser> userManager, IVaultDigitalLifeService vaultDigitalLife, IVaultAnswerDigitalLifeService answerDigitalLifeService)
        {
            this._configuration = configuration;
            this._vaultService = vaultService;
            this._client = client;
            this._userManager = userManager;
            this._digitalLifeService = vaultDigitalLife;
            this._answerDigitalLifeService = answerDigitalLifeService;
            this._accessService = new AccessService<Common.Models.Vault>(this._configuration);
            this._answerDigitalLifeService = answerDigitalLifeService;
        }

        public async Task OnGetAsync(string vaultId)
        {
            await this._accessService.CheckAccess(vaultId, this._userId, this.User.IsInRole("SuperAdmin"));

            List<Common.Models.Vault> folderAskDelete = await this._vaultService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = folderAskDelete.Count;

            this._returnUrl = this.Url.Page("/Vault/Informations", new { vaultId });
            this._vaultId = vaultId;
            Common.Models.Vault folder = await this._vaultService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));
            this._vault = await this._vaultService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));

            this._breadcrumb = new Dictionary<string, string>();
            this._breadcrumb.Add("/Vault", "Mes coffre-forts");
            this._breadcrumb.Add(this.Url.Page("/Vault/Informations", new { vaultId }), $"Vault N°{this._vault.Title} - Informations");
            this._breadcrumb.Add(this.Url.Page("/Vault/Formulaires/Heritage", new { vaultId }), "Patrimoine");

            /*List<Common.Models.VaultContact> folderContact = await this._contactService.Search(new Specification<Common.Models.VaultContact>(fc => fc.VaultId == vaultId));
            this._folderTypeContact = folderContact.Select(fc => fc.TypeContact).ToList();*/

            this._vaultDigitalLife = await this._digitalLifeService.Get(new Specification<VaultDigitalLife>(vdl => vdl.VaultId == vaultId));
            if (this._vaultDigitalLife == null)
            {
                this._vaultDigitalLife = new VaultDigitalLife()
                {
                    VaultId = vaultId,
                    CompletedForm = false,
                };
            }
            //------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            this._answerDigitalLife = await this._answerDigitalLifeService.Search(new Specification<VaultAnswersDigitalLife>(vadl => vadl.VaultId == vaultId));
            /*if (this._answerDigitalLife.Count != 0)
            {
                List<VaultAnswersDigitalLife> test = this._answerDigitalLife.OrderBy(ptl => ptl.Order).ToList();
                int tmpi = test.Count - 1;

                VaultAnswersDigitalLife tmpp = test[tmpi];
                int te = tmpp.Order;

                for (int i = 1; i <= te; i++)
                {
                    List<VaultAnswersDigitalLife> test1 = await this._answerDigitalLifeService.Search(new Specification<VaultAnswersDigitalLife>(ptl => ptl.VaultId == vaultId && ptl.Order == i));

                    foreach (VaultAnswersDigitalLife test2 in test1)
                    {
                        int tn = test1.Count;
                        if (tn > 1)
                        {
                            await this._answerDigitalLifeService.Delete(test2);
                            this._answerDigitalLife.Remove(test2);
                        }

                        tn--;
                    }
                }
            }*/

            /*if (this._placesToLive.Count == 0)
            {
                this._placesToLive = CreatePlaceToLiveResponses(folderId);
            }

            this._answerDigitalLife = this._placesToLive.OrderBy(ptl => ptl.Order).ToList();*/
        }

        public async Task<FileContentResult> OnGetDownloadDigitalLifeInfoPdf(string vaultId)
        {
            string title = "Patrimoine";

            Specification<Common.Models.Vault> spec = new(f => f.Id == vaultId);
            spec.Includes.Add(f => f.VaultHeritages);
            Common.Models.Vault folder = await this._vaultService.Get(spec);

            string questions = "";
            return this.File(VaultInfosHelper.BuildFolderInfosPdf(folder, questions, title), "application/pdf", $"Vie numérique - {folder.FirstName} {folder.LastName}.pdf");
        }

        public async Task<IActionResult> OnPostSaveInfos(VaultDigitalLife vaultDigitalLife, List<VaultAnswersDigitalLife> answersDigitalLife, List<VaultAnswersDigitalLife> answersDigitalLifeToDelete)
        {
            /*if (vaultDigitalLife.Id == null)
            {
                vaultDigitalLife.Id = Guid.NewGuid().ToString();
                await this._digitalLifeService.Add(vaultDigitalLife);
            }
            else
            {
                await this._digitalLifeService.Update(vaultDigitalLife);
            }

            return this.StatusCode((int)HttpStatusCode.OK, null);*/

            if (vaultDigitalLife.Id == null)
            {
                vaultDigitalLife.Id = Guid.NewGuid().ToString();
                await this._digitalLifeService.Add(vaultDigitalLife);
            }
            else
            {
                await this._digitalLifeService.Update(vaultDigitalLife);
            }

            foreach (VaultAnswersDigitalLife answerDigitalLife in answersDigitalLife)
            {
                if (answerDigitalLife.Id == 0)
                {
                    try
                    {
                        await this._answerDigitalLifeService.Add(answerDigitalLife);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                else
                {
                    await this._answerDigitalLifeService.Update(answerDigitalLife);
                }
            }

            foreach (VaultAnswersDigitalLife answerDigitalLife in answersDigitalLifeToDelete)
            {
                await this._answerDigitalLifeService.Delete(answerDigitalLife);
            }

            return this.StatusCode((int)HttpStatusCode.OK, null);
        }
    }
}
