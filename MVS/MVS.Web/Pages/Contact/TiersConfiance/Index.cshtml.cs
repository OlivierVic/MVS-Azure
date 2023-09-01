using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVS.Business;
using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Models;
using MVS.Common.Specifications;
using System.Security.Claims;

namespace MVS.Web.Pages.Contact.TiersConfiance
{
    public class IndexModel : PageModel
    {
        private readonly IVaultTiersContactService _tiersContactService;
        private readonly IAccessService<Common.Models.Vault> _accessService;
        private readonly IVaultService _vaultService;
        private readonly IJobProfessionelService _jobProfessionelService;
        private readonly IJobParticularService _jobParticularService;
        private IConfiguration _configuration;

        public List<JobParticular> _jobParticular { get; set; }
        public List<JobProfessionel> _jobProfessionel { get; set; }

        private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public List<VaultCategory> _categories { get; set; }
        public Common.Models.VaultTiersContact _contact { get; set; }
        public Common.Models.Vault _vault { get; set; }
        public Dictionary<string, string> _breadcrumb { get; set; }
        public Dictionary<string, string> _vaultInfoHeader { get; set; }

        public IndexModel(IVaultTiersContactService contactService, IVaultCategoryService categoryService, IJobParticularService jobParticularService,
                        IConfiguration configuration, IVaultService vaultService, IJobProfessionelService jobProfessionelService)
        {
            this._configuration = configuration;
            this._accessService = new AccessService<Common.Models.Vault>(this._configuration);
            this._tiersContactService = contactService;
            this._jobParticularService = jobParticularService;
            this._vaultService = vaultService;
            this._jobProfessionelService = jobProfessionelService;
        }

        public async Task OnGet(string vaultId, string contactId = null)
        {
            await this._accessService.CheckAccess(vaultId, this._userId, this.User.IsInRole("SuperAdmin"));

            List<Common.Models.Vault> folderAskDelete = await this._vaultService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));

            this._vault = await this._vaultService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));

            if (contactId != null)
            {
                this._contact = await this._tiersContactService.Get(new Specification<Common.Models.VaultTiersContact>(c => c.Id == contactId));
                if (this._contact == null)
                {
                    throw new ArgumentException("La donnée que vous voulez récupérer n'existe pas");
                }
            }
            else
            {
                this._contact = new Common.Models.VaultTiersContact()
                {
                    VaultId = vaultId,
                };
            }

            //this._folderInfoHeader = VaultInfosHelper.GetFolderInfoHeader(this._folder);

            this._breadcrumb = new Dictionary<string, string>();
            this._breadcrumb.Add("/Vault", "Mes coffre-forts");
            this._breadcrumb.Add(this.Url.Page("/Vault/Informations", new { vaultId }), $"Coffre-fort n°{this._vault.Title} - Informations");
            //this._breadcrumb.Add(this.Url.Page("/Contact/TiersConfiance", new { vaultId }), "Création d’un contact tiers de confiance");

            /*this._categories = await this._categoryService.Search(new Specification<VaultCategory>(c => true));*/
            this._jobParticular = await this._jobParticularService.Search(new Specification<JobParticular>(c => true));
            this._jobProfessionel = await this._jobProfessionelService.Search(new Specification<JobProfessionel>(c => true));
        }

        public async Task<IActionResult> OnPostAsync(Common.Models.VaultTiersContact contact)
        {
            if (contact.Id == null)
            {
                contact.Id = Guid.NewGuid().ToString();
                contact.RequestCopy = false;
                contact.TypeContact = (int)JobEnum.Null;
                await this._tiersContactService.Add(contact);
            }
            else
            {
                await this._tiersContactService.Update(contact);
            }

            Common.Models.Vault vault = await this._vaultService.Get(new Specification<Common.Models.Vault>(f => f.Id == contact.VaultId));

            return new RedirectToPageResult("/Vault/Informations", new { vaultId = vault.Id });
        }
    }
}
