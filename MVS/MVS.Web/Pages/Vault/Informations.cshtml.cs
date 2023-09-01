using MVS.Business;
using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Models;
using MVS.Common.Specifications;
using MVS.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Security.Claims;

namespace MVS.Web.Pages.Vault
{
    public class InformationsModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IVaultService _vaultService;
        private readonly IAccessService<Common.Models.Vault> _accessService;
        //
        private readonly IVaultPersonalInfoService _vaultPersonalInfoService;
        private readonly IVaultFamilyInfoService _vaultFamilyInfoService;
        private readonly IVaultHeritageService _vaultHeritageService;
        private readonly IVaultAnticipationMeasuresInfoService _folderAnticipationMeasuresInfoService;
        private readonly IVaultFuneraryVolonteService _vaultFuneraryVolonteService;
        private readonly IVaultDigitalLifeService _vaultDigitalLifeService;
        private readonly IVaultAdministrativeLifeService _vaultAdministrativeLifeService;
        //

        private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public Common.Models.Vault _vault { get; set; }
        public VaultPersonalInfo _vaultPersonalInfo { get; set; }
        public VaultFamilyInfo _vaultFamilyInfo { get; set; }
        public VaultHeritage _vaultHeritage { get; set; }
        public VaultAnticipationMeasuresInfo _vaultAnticipationMeasuresInfo { get; set; }
        public VaultAdministrativeLife _vaultAdministrativeLife { get; set; }
        public VaultDigitalLife _vaultDigitalLife { get; set; }
        public VaultFuneraryVolonte _vaultFuneraryVolonte { get; set; }
        public string _vaultId { get; set; }
        public bool _personalSituationFill { get; set; }
        public bool _familySituationFill { get; set; }
        public bool _lifestyleFill { get; set; }
        public bool _healthFill { get; set; }
        public bool _heritageFill { get; set; }
        public bool _budgetFill { get; set; }
        public bool _anticipationMeasuresFill { get; set; }
        public bool _futureChoicesFill { get; set; }
        public Dictionary<string, string> _breadcrumb { get; set; }
        public Dictionary<string, string> _folderInfoHeader { get; set; }

        public bool _personalCreate { get; set; }
        public bool _familyCreate { get; set; }
        public bool _heritageCreate { get; set; }
        public bool _measureCreate { get; set; }
        public bool _vieNumeriqueCreate { get; set; }
        public bool _vieAdministrativeCreate { get; set; }
        public bool _volonteFuneraireCreate { get; set; }

        public bool _personalComplete { get; set; }
        public bool _familyComplete { get; set; }
        public bool _heritageComplete { get; set; }
        public bool _measureComplete { get; set; }
        public bool _vieNumeriqueComplete { get; set; }
        public bool _vieAdministrativeComplete { get; set; }
        public bool _volonteFuneraireComplete { get; set; }

        public int _folderPaymentNumber { get; set; }
        public int _folderAskDeleteNumber { get; set; }


        public InformationsModel(IVaultService vaultService, IConfiguration configuration, IVaultPersonalInfoService personalService, IVaultFamilyInfoService familyService,
            IVaultHeritageService heritageService, IVaultAnticipationMeasuresInfoService measureService, IVaultFuneraryVolonteService funeraryVolonteService, IVaultDigitalLifeService digitalLifeService, IVaultAdministrativeLifeService administrativeLifeService)
        {
            this._vaultService = vaultService;
            this._configuration = configuration;
            this._accessService = new AccessService<Common.Models.Vault>(this._configuration);

            this._vaultPersonalInfoService = personalService;
            this._vaultFamilyInfoService = familyService;
            this._vaultHeritageService = heritageService;
            this._folderAnticipationMeasuresInfoService = measureService;
            this._vaultFuneraryVolonteService = funeraryVolonteService;
            this._vaultDigitalLifeService = digitalLifeService;
            this._vaultAdministrativeLifeService = administrativeLifeService;
        }

        public async Task<IActionResult> OnGet(string vaultId)
        {
            await this._accessService.CheckAccess(vaultId, this._userId, this.User.IsInRole("SuperAdmin"));

            List<Common.Models.Vault> folderAskDelete = await this._vaultService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = folderAskDelete.Count;

            Specification<Common.Models.Vault> spec = new(f => f.Id == vaultId);
            this._vault = await this._vaultService.Get(spec);


            this._breadcrumb = new Dictionary<string, string>();
            this._breadcrumb.Add("/Vault", "Mes coffre-fort");
            this._breadcrumb.Add(this.Url.Page("/Vault/Informations", new { vaultId }), $"Coffre_fort N°{this._vault.Title}");

            this._folderInfoHeader = VaultInfosHelper.GetFolderInfoHeader(this._vault);

            this._personalSituationFill = this._vault.VaultPersonalInfos.Count > 0;
            this._familySituationFill = this._vault.VaultFamilyInfos.Count > 0;
            this._anticipationMeasuresFill = this._vault.VaultAnticipationMeasuresInfos.Count > 0;

            this._vaultId = vaultId;

            this._vaultPersonalInfo = await this._vaultPersonalInfoService.Get(new Specification<VaultPersonalInfo>(vpi => vpi.VaultId == vaultId));
            this._vaultFamilyInfo = await this._vaultFamilyInfoService.Get(new Specification<VaultFamilyInfo>(vfi => vfi.VaultId == vaultId));
            this._vaultHeritage = await this._vaultHeritageService.Get(new Specification<VaultHeritage>(vh => vh.VaultId == vaultId));
            this._vaultAnticipationMeasuresInfo = await this._folderAnticipationMeasuresInfoService.Get(new Specification<VaultAnticipationMeasuresInfo>(vami => vami.VaultId == vaultId));

            this._vaultAdministrativeLife = await this._vaultAdministrativeLifeService.Get(new Specification<VaultAdministrativeLife>(val => val.VaultId == vaultId));
            this._vaultDigitalLife = await this._vaultDigitalLifeService.Get(new Specification<VaultDigitalLife>(vdl => vdl.VaultId == vaultId));
            this._vaultFuneraryVolonte = await this._vaultFuneraryVolonteService.Get(new Specification<VaultFuneraryVolonte>(vfv => vfv.VaultId == vaultId));

            if (this._vaultPersonalInfo != null)
            {
                this._personalCreate = true;
                this._personalComplete = this._vaultPersonalInfo.CompletedForm;
            }

            if (this._vaultFamilyInfo != null)
            {
                this._familyCreate = true;
                this._familyComplete = this._vaultFamilyInfo.CompletedForm;
            }

            if (this._vaultHeritage != null)
            {
                this._heritageCreate = true;
                this._heritageComplete = this._vaultHeritage.CompletedForm;
            }

            if (this._vaultAnticipationMeasuresInfo != null)
            {
                this._measureCreate = true;
                this._measureComplete = this._vaultAnticipationMeasuresInfo.CompletedForm;
            }

            if (this._vaultAdministrativeLife != null)
            {
                this._vieAdministrativeCreate = true;
                this._vieAdministrativeComplete = this._vaultAdministrativeLife.CompletedForm;
            }

            if (this._vaultDigitalLife != null)
            {
                this._vieNumeriqueCreate = true;
                this._vieNumeriqueComplete = this._vaultDigitalLife.CompletedForm;
            }

            if (this._vaultFuneraryVolonte != null)
            {
                this._volonteFuneraireCreate = true;
                this._volonteFuneraireComplete = this._vaultFuneraryVolonte.CompletedForm;
            }

            return this.Page();
        }

        public async Task<IActionResult> OnGetAddCapacity(string vaultId)
        {
            Common.Models.Vault vault = await this._vaultService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));


            await this._vaultService.Update(vault);

            return this.RedirectToPage($"/Vault/Informations", new { vaultId });
        }
    }
}
