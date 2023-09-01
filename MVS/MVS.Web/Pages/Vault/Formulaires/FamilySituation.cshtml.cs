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

namespace MVS.Web.Pages.Vault.InfosForms;

public class FamilySituationModel : PageModel
{
    private readonly IVaultCategoryService _categoryService;
    private readonly IVaultService _vaultService;
    private readonly IVaultFamilyInfoService _vaultFamilyInfoService;
    private readonly IConfiguration _configuration;
    private readonly IAccessService<Common.Models.Vault> _accessService;
    private readonly IVaultDocumentService _folderDocumentService;

    public string _returnUrl { get; set; }
    public Common.Models.Vault _folder { get; set; }
    public VaultFamilyInfo _folderFamilyInfo { get; set; }
    public bool _isFutureProtection { get; set; }
    public Dictionary<string, string> _breadcrumb { get; set; }

    private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
    public List<int> _folderDocumentType { get; set; }
    public int _folderPaymentNumber { get; set; }
    public int _folderAskDeleteNumber { get; set; }

    public FamilySituationModel(IVaultCategoryService categoryService, IVaultService vaultService,
                                    IVaultFamilyInfoService folderFamilyInfoService, IConfiguration configuration, IVaultDocumentService folderDocumentService)
    {
        this._categoryService = categoryService;
        this._vaultService = vaultService;
        this._vaultFamilyInfoService = folderFamilyInfoService;
        this._configuration = configuration;
        this._accessService = new AccessService<Common.Models.Vault>(this._configuration);
        this._folderDocumentService = folderDocumentService;
    }

    public async Task OnGetAsync(string vaultId)
    {
        await this._accessService.CheckAccess(vaultId, this._userId, this.User.IsInRole("SuperAdmin"));

        List<Common.Models.Vault> folderAskDelete = await this._vaultService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
        this._folderAskDeleteNumber = folderAskDelete.Count;

        this._returnUrl = this.Url.Page("/Vault/Informations", new { vaultId });

        Common.Models.Vault vault = await this._vaultService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));

        this._breadcrumb = new Dictionary<string, string>();
        this._breadcrumb.Add("/Vault", "Mes coffre-forts");
        this._breadcrumb.Add(this.Url.Page("/Vault/Informations", new { vaultId }), $"Vault N°{vault.Title} - Informations");
        this._breadcrumb.Add(this.Url.Page("/Vault/Formulaires/FamilySituation", new { vaultId }), "Situation familiale");

        List<VaultDocument> folderDocuments = await this._folderDocumentService.Search(new Specification<VaultDocument>(fd => fd.VaultId == vaultId));
        this._folderDocumentType = folderDocuments.Select(fd => fd.Type).ToList();

        this._folderFamilyInfo = await this._vaultFamilyInfoService.Get(new Specification<VaultFamilyInfo>(ffi => ffi.VaultId == vaultId));
        if (this._folderFamilyInfo == null)
        {
            this._folderFamilyInfo = new VaultFamilyInfo()
            {
                VaultId = vaultId,
                CompletedForm = false,
            };
        }
    }

    public async Task<IActionResult> OnPostSaveInfos(VaultFamilyInfo folderFamilyInfo)
    {
        try
        {
            if (folderFamilyInfo.Id == null)
            {
                folderFamilyInfo.Id = Guid.NewGuid().ToString();
                await this._vaultFamilyInfoService.Add(folderFamilyInfo);
            }
            else
            {
                await this._vaultFamilyInfoService.Update(folderFamilyInfo);
                Thread.Sleep(2000);
            }

            return this.StatusCode((int)HttpStatusCode.OK, null);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<FileContentResult> OnGetDownloadFamilyInfoPdf(string vaultId)
    {
        string title = "Situation Familliale";

        Specification<Common.Models.Vault> spec = new(f => f.Id == vaultId);
        spec.Includes.Add(f => f.VaultFamilyInfos);
        Common.Models.Vault folder = await this._vaultService.Get(spec);

        string questions = "";
        return this.File(VaultInfosHelper.BuildFolderInfosPdf(folder, questions, title), "application/pdf", $"Situation familliale - {folder.FirstName} {folder.LastName}.pdf");
    }
}
