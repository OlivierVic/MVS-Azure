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
using NuGet.Protocol.Plugins;

namespace MVS.Web.Pages.Vault.InfosForms
{
    public class PersonalSitutationModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IJobParticularService _jobParticularService;
        private readonly IVaultService _folderService;
        private readonly IVaultPersonalInfoService _vaultPersonalInfoService;
        private readonly IAccessService<Common.Models.Vault> _accessService;
        private readonly IVaultDocumentService _folderDocumentService;

        private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public string _returnUrl { get; set; }
        public string _vaultId { get; set; }
        public Common.Models.Vault _vault { get; set; }
        public List<JobParticular> _jobsParticular { get; set; }
        public VaultPersonalInfo _folderPersonalInfo { get; set; }
        public bool _isImmediateProtection { get; set; }
        public Dictionary<string, string> _breadcrumb { get; set; }
        public List<int> _folderDocumentType { get; set; }
        public int _folderPaymentNumber { get; set; }
        public int _folderAskDeleteNumber { get; set; }

        public PersonalSitutationModel(IVaultCategoryService categoryService,
                                            IJobParticularService jobParticularService, IVaultService folderService,
                                            IVaultPersonalInfoService folderPersonalInfoService, IConfiguration configuration, IVaultDocumentService folderDocumentService)
        {
            this._jobParticularService = jobParticularService;
            this._folderService = folderService;
            this._vaultPersonalInfoService = folderPersonalInfoService;
            this._configuration = configuration;
            this._accessService = new AccessService<Common.Models.Vault>(this._configuration);
            this._folderDocumentService = folderDocumentService;

        }

        public async Task OnGetAsync(string vaultId)
        {
            await this._accessService.CheckAccess(vaultId, this._userId, this.User.IsInRole("SuperAdmin"));


            List<Common.Models.Vault> folderAskDelete = await this._folderService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = folderAskDelete.Count;

            this._returnUrl = this.Url.Page("/Vault/Informations", new { vaultId });
            this._jobsParticular = await this._jobParticularService.Search(new Specification<JobParticular>(jb => true));
            this._vault = await this._folderService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));
            this._vaultId = vaultId;

            this._breadcrumb = new Dictionary<string, string>();
            this._breadcrumb.Add("/Vault", "Mes coffre-forts");
            this._breadcrumb.Add(this.Url.Page("/Vault/Informations", new { vaultId }), $"Vault N°{this._vault.Title} - Informations");
            this._breadcrumb.Add(this.Url.Page("/Vault/Formulaires/PersonalSitutation", new { vaultId }), "Situation personnelle");

            List<VaultDocument> folderDocuments = await this._folderDocumentService.Search(new Specification<VaultDocument>(fd => fd.VaultId == vaultId));
            this._folderDocumentType = folderDocuments.Select(fd => fd.Type).ToList();

            this._folderPersonalInfo = await this._vaultPersonalInfoService.Get(new Specification<VaultPersonalInfo>(fpi => fpi.VaultId == vaultId));
            if (this._folderPersonalInfo == null)
            {
                this._folderPersonalInfo = new VaultPersonalInfo()
                {
                    VaultId = vaultId,
                    CompletedForm = false,
                };
            }
            Common.Models.Vault folder = await this._folderService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));
        }

        public async Task<IActionResult> OnPostSaveInfos(VaultPersonalInfo vaultPersonalInfo)
        {
            try
            {
                if (vaultPersonalInfo.Id == null)
                {
                    vaultPersonalInfo.Id = Guid.NewGuid().ToString();

                    //await this.CheckForCreateDocument(folderPersonalInfo);

                    await this._vaultPersonalInfoService.Add(vaultPersonalInfo);
                }
                else
                {
                    //await this.CheckForCreateDocument(folderPersonalInfo);

                    await this._vaultPersonalInfoService.Update(vaultPersonalInfo);
                }

                return this.StatusCode((int)HttpStatusCode.OK, null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<FileContentResult> OnGetDownloadPersonnalInfoPdf(string vaultId)
        {
            string title = "Situation Personnelle";

            Specification<Common.Models.Vault> spec = new(f => f.Id == vaultId);
            spec.Includes.Add(f => f.VaultPersonalInfos);
            Common.Models.Vault folder = await this._folderService.Get(spec);

            string questions = "";
            return this.File(VaultInfosHelper.BuildFolderInfosPdf(folder, questions, title), "application/pdf", $"Situation personnelle - {folder.FirstName} {folder.LastName}.pdf");
        }

        /*public async Task CreateEmptyDocumentMandatory(string vaultId, string name, int type, string typeName)
        {
            if (!await this._folderDocumentService.Any(new Specification<VaultDocument>(fd => fd.Type == type && fd.VaultId == vaultId)))
            {
                VaultDocument folderDocument = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    VaultId = vaultId,
                    Type = type,
                    TypeName = typeName,
                    Name = name,
                };

                await this._folderDocumentService.Add(folderDocument);
            }
*//*
            return this.StatusCode((int)HttpStatusCode.OK, null);*//*
        }

        public async Task CheckForCreateDocument(VaultPersonalInfo folderPersonalInfo)
        {
            if (folderPersonalInfo.LivingEnvironment == "Maison de retraite")
            {
                await this.CreateEmptyDocumentMandatory(folderPersonalInfo.VaultId, "Contrat d'hébergement en maison de retraite", (int)FolderDocumentTypeEnum.ContractAccommodationRetirementHome, "Situation Personnelle");
            }

            if (folderPersonalInfo.HousingLaw == "Propriétaire")
            {
                await this.CreateEmptyDocumentMandatory(folderPersonalInfo.VaultId, "Avis de taxe foncière", (int)FolderDocumentTypeEnum.PropertyTaxNotice, "Situation Personnelle");
            }

            if (folderPersonalInfo.HousingLaw == "Locataire")
            {
                await this.CreateEmptyDocumentMandatory(folderPersonalInfo.VaultId, "Bail d'habitation", (int)FolderDocumentTypeEnum.ResidentialLease, "Situation Personnelle");
            }
        }*/
    }
}
