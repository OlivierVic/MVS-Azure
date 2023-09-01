using MVS.Business;
using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Models;
using MVS.Common.Specifications;
using MVS.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MVS.Web.Pages.VaultContact;

public class AddModel : PageModel
{
    private readonly IVaultContactService _contactService;
    private readonly IVaultCategoryService _categoryService;
    private readonly IJobProfessionelService _jobProfessionelService;
    private readonly IJobParticularService _jobParticularService;
    private IConfiguration _configuration;
    private readonly IAccessService<Common.Models.Vault> _accessService;
    private readonly IVaultService _folderService;

    private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);

    public List<VaultCategory> _categories { get; set; }
    public List<JobParticular> _jobParticular { get; set; }
    public List<JobProfessionel> _jobs { get; set; }
    public Common.Models.VaultContact _contact { get; set; }
    public Common.Models.Vault _folder { get; set; }
    public Dictionary<string, string> _breadcrumb { get; set; }
    public Dictionary<string, string> _folderInfoHeader { get; set; }
    public int _folderPaymentNumber { get; set; }
    public int _folderAskDeleteNumber { get; set; }

    public AddModel(IVaultContactService contactService, IVaultCategoryService categoryService, IJobParticularService jobParticularService,
                        IConfiguration configuration, IVaultService folderService, IJobProfessionelService jobService)
    {
        this._configuration = configuration;
        //this._accessService = new AccessService<Common.Models.Vault>(this._configuration);
        this._contactService = contactService;
        this._categoryService = categoryService;
        this._jobParticularService = jobParticularService;
        this._folderService = folderService;
        this._jobProfessionelService = jobService;
    }

    public async Task OnGet(string vaultId, string contactId = null, bool isFutuAgent = false, bool isSetAskProtection = false, bool isProtector = false, bool isDoctor = false)
    {
        await this._accessService.CheckAccess(vaultId, this._userId, this.User.IsInRole("SuperAdmin"));

        
        this._folderPaymentNumber = 0;

        List<Common.Models.Vault> folderAskDelete = await this._folderService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
        this._folderAskDeleteNumber = folderAskDelete.Count;

        this._folder = await this._folderService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));

        if (contactId != null)
        {
            this._contact = await this._contactService.Get(new Specification<Common.Models.VaultContact>(c => c.Id == contactId));
            if (this._contact == null)
            {
                throw new ArgumentException("La donnée que vous voulez récupérer n'existe pas");
            }
        }
        else
        {
            this._contact = new Common.Models.VaultContact()
            {
                VaultId = vaultId,
            };
        }

        //this._folderInfoHeader = VaultInfosHelper.GetFolderInfoHeader(this._folder);

        this._breadcrumb = new Dictionary<string, string>();
        this._breadcrumb.Add("/Vault", "Mes dossiers");
        this._breadcrumb.Add(this.Url.Page("/Vault/Vault", new { vaultId }), $"Dossier n°{this._folder.Title} - Avancement du dossier");
        this._breadcrumb.Add(this.Url.Page("/VaultContact/Add", new { vaultId }), "Création d’un contact");

        this._contact.IsFutuAgent = isFutuAgent;
        this._contact.IsSetAskProtection = isSetAskProtection;
        this._contact.IsProtector = isProtector;
        this._contact.IsDoctor = isDoctor;

        this._categories = await this._categoryService.Search(new Specification<VaultCategory>(c => true));
        this._jobParticular = await this._jobParticularService.Search(new Specification<JobParticular>(c => true));
        this._jobs = await this._jobProfessionelService.Search(new Specification<JobProfessionel>(c => true));
    }

    public async Task<IActionResult> OnPostAsync(Common.Models.VaultContact contact)
    {
        if (contact.Id == null)
        {
            contact.Id = Guid.NewGuid().ToString();
            contact.RequestCopy = false;
            contact.TypeContact = (int)JobEnum.Null;
            await this._contactService.Add(contact);
        }
        else
        {
            await this._contactService.Update(contact);
        }

        Common.Models.Vault folder = await this._folderService.Get(new Specification<Common.Models.Vault>(f => f.Id == contact.VaultId));
        if (contact.Ispro)
        {
            return (IActionResult)new RedirectToPageResult("/VaultContact/Professional/FutureProtection/AddStep2", new { contactId = contact.Id });
        }
        else
        {
            return new RedirectToPageResult("/VaultContact/Particular/AddStep2", new { contactId = contact.Id });
        }
    }
}
