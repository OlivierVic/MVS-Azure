using MVS.Business;
using MVS.Common;
using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Models;
using MVS.Common.Specifications;
using MVS.EmailSender.Sender;
using MVS.EmailSender.Templates.Models;
using MVS.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Security.Claims;

namespace MVS.Web.Pages.VaultContact.Professional.ImmediateProtection;

public class AddStep2Model : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly IVaultContactService _contactService;
    private readonly IAccessService<Common.Models.Vault> _accessService;
    private readonly IVaultService _folderService;
    private readonly IAspNetUserService _userService;
    private readonly IVaultUsersService _folderUsersService;
    private readonly IEmailSender _emailSender;
    private readonly UserManager<ApplicationUser> _userManager;

    private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
    public Common.Models.Vault _folder { get; set; }
    public Dictionary<string, string> _breadcrumb { get; set; }
    public Dictionary<string, string> _folderInfoHeader { get; set; }
    public Common.Models.VaultContact _contact { get; set; }
    public string _contactId { get; set; }
    public string _folderId { get; set; }
    public int _folderPaymentNumber { get; set; }
    public int _folderAskDeleteNumber { get; set; }

    public AddStep2Model(IVaultContactService contactService, IConfiguration configuration, IVaultService folderService, IAspNetUserService userService, IEmailSender emailSender, IVaultUsersService folderUsersService, UserManager<ApplicationUser> userManager)
    {
        this._configuration = configuration;
        this._accessService = new AccessService<Common.Models.Vault>(this._configuration);
        this._contactService = contactService;
        this._folderService = folderService;
        this._userService = userService;
        this._folderUsersService = folderUsersService;
        this._emailSender = emailSender;
        this._userManager = userManager;
    }
    public async Task OnGetAsync(string contactId)
    {
        this._contact = await this._contactService.Get(new Specification<Common.Models.VaultContact>(c => c.Id == contactId));
        if (this._contact == null)
        {
            throw new ArgumentException("La donnée que vous voulez récupérer n'existe pas");
        }

        await this._accessService.CheckAccess(this._contact.VaultId, this._userId, this.User.IsInRole("SuperAdmin"));

        this._folderPaymentNumber = 0;

        List<Common.Models.Vault> folderAskDelete = await this._folderService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
        this._folderAskDeleteNumber = folderAskDelete.Count;

        this._contactId = this._contact.Id;
        this._folderId = this._contact.VaultId;

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

        contactInfo.IsFolderAdmin = contact.IsFolderAdmin;
        contactInfo.IsSetJudge = contact.IsSetJudge;
        contactInfo.TypeMission = contact.TypeMission;
        contactInfo.OpinionPro = contact.OpinionPro;
        contactInfo.Confidence = contact.Confidence;
        contactInfo.MoreInfo = contact.MoreInfo;

        contactInfo.AdviceStatus = contactInfo.OpinionPro ?? false ? (int)ContactAdviceStatus.Requested : (int)ContactAdviceStatus.NotRequested;

        await this._contactService.Update(contactInfo);

        if ((bool)contactInfo.IsFolderAdmin)
        {
            await OnPostCreateAdminFolderUser(contactInfo.VaultId, contactInfo.Email, contactInfo.FirstName, contactInfo.LastName, contactInfo.Ispro);
        }

        return new RedirectToPageResult("/Vault/AddressBook", new { vaultId = contactInfo.VaultId });
    }

    public async Task<IActionResult> OnPostCreateAdminFolderUser(string vaultId, string email, string firstName, string lastName, bool isPro)
    {
        Common.Models.Vault folder = await this._folderService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));
        AspNetUser user = await this._userService.Get(new Specification<AspNetUser>(u => u.Email == email && u.FirstName == firstName && u.LastName == lastName));

        if (isPro)
        {
            if (user != null)
            {
                await this.OnGetSendEmailProAddInFolder(email, user);

                VaultUser folderUsers = await this._folderUsersService.Get(new Specification<VaultUser>(fu => fu.VaultId == vaultId && fu.UserId == user.Id));

                if (folderUsers == null)
                {
                    folder.VaultUsers = new List<VaultUser>()
                    {
                        new VaultUser()
                        {
                            Id = Guid.NewGuid().ToString(),
                            VaultId = folder.Id,
                            UserId = user.Id,
                        }
                    };

                    foreach (VaultUser answer in folder.VaultUsers)
                    {
                        try
                        {
                            if (answer.VaultId == folder.Id && answer.UserId == user.Id)
                            {
                                await this._folderUsersService.Add(answer);
                            }
                            else
                            {
                                await this._folderUsersService.Update(answer);
                            }

                            return this.StatusCode((int)HttpStatusCode.OK, null);
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }

                    return this.StatusCode((int)HttpStatusCode.OK, null);
                }
            }
            else
            {
                ApplicationUser newUserCreate = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = email,
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    EmailConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = false,
                    AccessFailedCount = 0,
                    Gender = 0,
                };
                await this._userManager.CreateAsync(newUserCreate);

                await this._userManager.AddToRoleAsync(newUserCreate, "Pro");

                AspNetUser newUser = await this._userService.Get(new Specification<AspNetUser>(u => u.Email == newUserCreate.Email && u.FirstName == newUserCreate.FirstName && u.LastName == newUserCreate.LastName));
                VaultUser folderUsers = await this._folderUsersService.Get(new Specification<VaultUser>(fu => fu.VaultId == vaultId && fu.UserId == newUser.Id));
                await this.OnGetSendEmailProRegister(newUser.Email, newUser);

                if (folderUsers == null)
                {
                    folder.VaultUsers = new List<VaultUser>()
                    {
                        new VaultUser()
                        {
                            Id = Guid.NewGuid().ToString(),
                            VaultId = folder.Id,
                            UserId = newUser.Id,
                        }
                    };

                    foreach (VaultUser answer in folder.VaultUsers)
                    {
                        try
                        {
                            if (answer.VaultId == folder.Id && answer.UserId == newUser.Id)
                            {
                                await this._folderUsersService.Add(answer);
                                return this.StatusCode((int)HttpStatusCode.OK, null);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                }

                return this.StatusCode((int)HttpStatusCode.OK, null);
            }
        }

        return this.StatusCode((int)HttpStatusCode.OK, null);
    }

    public async Task OnGetSendEmailProRegister(string email, AspNetUser user)
    {
        string plateformUrl = this.Url.PageLink("/Home/Index", null, null, this.Request.Scheme);
        string RGPDPageUrl = this.Url.PageLink("/RGPD/Index", null, null, this.Request.Scheme);
        string MoreInfoPageUrl = this.Url.PageLink("/Home/Index", null, null, this.Request.Scheme);
        string DefinePasswordUrl = this.Url.PageLink("/Home/PasswordResetNewUserAdmin", null, new { email, user.FirstName, user.LastName }, this.Request.Scheme);

        await this._emailSender.SendEmailAsync(email, new RegisterUserAdminProTemplate
        {
            Model = new RegisterUserAdminProTemplateModel
            {
                RGPDPageUrl = RGPDPageUrl,
                DefinePasswordUrl = DefinePasswordUrl,
                PlateformUrl = plateformUrl,
                MoreInfoPageUrl = MoreInfoPageUrl
            }
        });
    }

    public async Task OnGetSendEmailProAddInFolder(string email, AspNetUser user)
    {
        string _plateformUrl = this.Url.PageLink("/Home/Index", null, null, this.Request.Scheme);
        string _connectionPageUrl = this.Url.PageLink("/Home/Login", null, null, this.Request.Scheme);
        string _RGPDPageUrl = this.Url.PageLink("/RGPD/Index", null, null, this.Request.Scheme);

        await this._emailSender.SendEmailAsync(email, new AddProUserInFolderTemplate
        {
            Model = new AddProUserInFolderTemplateModel
            {
                ConnectionPageUrl = _connectionPageUrl,
                RGPDPageUrl = _RGPDPageUrl,
                PlateformUrl = _plateformUrl
            }
        });
    }
}
