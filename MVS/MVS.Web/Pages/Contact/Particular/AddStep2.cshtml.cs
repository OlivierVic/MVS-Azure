using MVS.Business;
using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Specifications;
using MVS.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MVS.Web.Pages.VaultContact.Particular.FutureProtection;

public class AddStep2Model : PageModel
{
    private readonly IVaultContactService _contactService;
    private readonly IAccessService<Common.Models.Vault> _accessService;
    private readonly IConfiguration _configuration;
    private readonly IVaultService _folderService;

    private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
    public string _contactId { get; set; }
    public string _folderId { get; set; }
    public Common.Models.Vault _folder { get; set; }
    public Dictionary<string, string> _breadcrumb { get; set; }
    public Dictionary<string, string> _folderInfoHeader { get; set; }
    public int _folderPaymentNumber { get; set; }
    public int _folderAskDeleteNumber { get; set; }

    public AddStep2Model(IVaultContactService contactService, IConfiguration configuration, IVaultService folderService)
    {
        this._contactService = contactService;
        this._configuration = configuration;
        this._accessService = new AccessService<Common.Models.Vault>(this._configuration);
        this._folderService = folderService;
    }

    public async Task OnGet(string contactId)
    {
        Common.Models.VaultContact contact = await this._contactService.Get(new Specification<Common.Models.VaultContact>(c => c.Id == contactId));
        if (contact == null)
        {
            throw new ArgumentException("La donnée que vous voulez récupérer n'existe pas");
        }

        await this._accessService.CheckAccess(contact.VaultId, this._userId, this.User.IsInRole("SuperAdmin"));

        this._folderPaymentNumber = 0;

        List<Common.Models.Vault> folderAskDelete = await this._folderService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
        this._folderAskDeleteNumber = folderAskDelete.Count;

        this._contactId = contact.Id;
        this._folderId = contact.VaultId;

        this._folder = await this._folderService.Get(new Specification<Common.Models.Vault>(f => f.Id == this._folderId));
        
        this._folderInfoHeader = VaultInfosHelper.GetFolderInfoHeader(this._folder);

        this._breadcrumb = new Dictionary<string, string>();
        this._breadcrumb.Add("/Vault", "Mes dossiers");
        this._breadcrumb.Add(this.Url.Page("/Vault/Vault", new { this._folderId }), $"Dossier n°{this._folder.Title} - Avancement du dossier");
        this._breadcrumb.Add(this.Url.Page("/VaultContact/Add", new { this._folderId }), "Création d’un contact");
    }

    public async Task<IActionResult> OnPostAsync(Common.Models.VaultContact contact)
    {
        Common.Models.VaultContact contactInfo = await this._contactService.Get(new Specification<Common.Models.VaultContact>(c => c.Id == contact.Id));

        if(contactInfo.Kinship == null)
        {
            contactInfo.Kinship = 11;
            contactInfo.Other = "Non Renseigné";
        }
        else
        {
            contactInfo.Kinship = contact.Kinship;
            contactInfo.OtherFamilyTies = contact.OtherFamilyTies;
            contactInfo.Other = contact.Other;
        }
        contactInfo.HelpNeeded = contact.HelpNeeded;
        contactInfo.DateOfBirth = contact.DateOfBirth;
        contactInfo.PlaceOfBirth = contact.PlaceOfBirth;
        contactInfo.Nationality = contact.Nationality;

        await this._contactService.Update(contactInfo);

        Common.Models.Vault folder = await this._folderService.Get(new Specification<Common.Models.Vault>(f => f.Id == contactInfo.VaultId));

        return (IActionResult)new RedirectToPageResult("/VaultContact/Particular/FutureProtection/AddStep3", new { contactId = contact.Id });
    }
}
