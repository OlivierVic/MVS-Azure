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
using System.Net;
using System.Security.Claims;

namespace MVS.Web.Pages.Vault.UpdateFolder;

public class IndexModel : PageModel
{
    private readonly IVaultService _vaultService;
    private readonly IAccessService<Common.Models.Vault> _accessService;
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;

    private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);

    public Common.Models.Vault _vault { get; set; }
    public string _vaultId { get; set; }
    public string _vaultName { get; set; }
    public Dictionary<string, string> _breadcrumb { get; set; }
    public Dictionary<string, string> _folderInfoHeader { get; set; }
    public int _folderPaymentNumber { get; set; }
    public int _folderAskDeleteNumber { get; set; }

    public IndexModel(IConfiguration configuration, IVaultService vaultService, UserManager<ApplicationUser> userManager)
    {
        this._configuration = configuration;
        this._accessService = new AccessService<Common.Models.Vault>(this._configuration);
        this._vaultService = vaultService;
        this._userManager = userManager;
    }

    public async Task OnGet(string vaultId)
    {
        await this._accessService.CheckAccess(vaultId, this._userId, this.User.IsInRole("SuperAdmin"));


        List<Common.Models.Vault> folderAskDelete = await this._vaultService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
        this._folderAskDeleteNumber = folderAskDelete.Count;

        this._vaultId = vaultId;
        this._vault = await this._vaultService.Get(new Specification<Common.Models.Vault>(f => f.Id == this._vaultId));

        this._vaultName = this._vault.FirstName + ' ' + this._vault.LastName;

        this._breadcrumb = new Dictionary<string, string>();
        this._breadcrumb.Add("/Vault", "Mes dossiers");
        this._breadcrumb.Add(this.Url.Page("/Vault/Vault", new { vaultId = this._vault.Id }), $"Dossier N°{this._vault.Title}");
        this._breadcrumb.Add(this.Url.Page("/Vault/UpdateFolder/Index", new { vaultId = this._vault.Id }), "Modifier");

        this._folderInfoHeader = VaultInfosHelper.GetFolderInfoHeader(this._vault);

    }

    public async Task<IActionResult> OnPostUpdateProperty(string vaultId, int sex, string firstName, string lastName, string birthName)
    {
        Common.Models.Vault vault = await this._vaultService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));

        vault.Sex = sex;
        vault.FirstName = firstName;
        vault.LastName = lastName;
        vault.BirthName = birthName;

        await this._vaultService.Update(vault);
        return this.StatusCode((int)HttpStatusCode.OK, null);
    }

    public async Task<IActionResult> OnPostUpdateIdentity(string vaultId, DateTime dateOfBirth, string placeOfBirth)
    {
        Common.Models.Vault vault = await this._vaultService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));

        vault.BirthDate = dateOfBirth;
        vault.BirthLocation = placeOfBirth;

        await this._vaultService.Update(vault);
        return this.StatusCode((int)HttpStatusCode.OK, null);
    }

    public async Task<IActionResult> OnPostUpdateContact(string vaultId, string address, string zipCodeAndCity, string country, string phoneNumber, string email)
    {
        Common.Models.Vault vault = await this._vaultService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));

        vault.Address = address;
        vault.ZipceCodeAndCity = zipCodeAndCity;
        vault.Country = country;
        vault.PhoneNumber = phoneNumber;
        vault.Email = email;

        await this._vaultService.Update(vault);
        return this.StatusCode((int)HttpStatusCode.OK, null);
    }
}
