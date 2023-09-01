using MVS.Business;
using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Models;
using MVS.Common.Specifications;
using MVS.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MVS.Web.Pages.Vault
{
    public class AddressBookParticularModel : PageModel
    {
        private readonly IAccessService<Common.Models.Vault> _accessService;
        private readonly IVaultService _folderService;
        private readonly IVaultContactService _contactService;
        private readonly IConfiguration _configuration;

        public string _folderId { get; set; }
        public Common.Models.Vault _folder { get; set; }
        public bool _isImmediateProtection { get; set; }
        public List<Common.Models.VaultContact> _contacts { get; set; }
        public Dictionary<int, string> _kinships { get; set; }
        public Dictionary<string, string> _breadcrumb { get; set; }
        public Dictionary<string, string> _folderInfoHeader { get; set; }
        private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public int _folderPaymentNumber { get; set; }
        public int _folderAskDeleteNumber { get; set; }


        public AddressBookParticularModel(IVaultService folderService, IVaultContactService contactService, IConfiguration configuration)
        {
            this._folderService = folderService;
            this._contactService = contactService;
            this._configuration = configuration;
            this._accessService = new AccessService<Common.Models.Vault>(this._configuration);
        }

        public async Task OnGetAsync(string vaultId)
        {
            await this._accessService.CheckAccess(vaultId, this._userId, this.User.IsInRole("SuperAdmin"));

            this._folderPaymentNumber = 0;

            List<Common.Models.Vault> folderAskDelete = await this._folderService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = folderAskDelete.Count;

            this._folder = await this._folderService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));
            this._folderId = vaultId;

            this._breadcrumb = new Dictionary<string, string>();
            this._breadcrumb.Add("/Vault", "Mes dossiers");
            this._breadcrumb.Add(this.Url.Page("/Vault/Vault", new { vaultId }), $"Dossier N°{this._folder.Title}");
            this._breadcrumb.Add(this.Url.Page("/Vault/AddressBook", new { vaultId }), "Carnet d'adresses");
            this._breadcrumb.Add(this.Url.Page("/Vault/AddressBookParticular", new { vaultId }), "Carnet d'adresses des proches");

            this._folderInfoHeader = VaultInfosHelper.GetFolderInfoHeader(this._folder);

            Common.Models.Vault folder = await this._folderService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));

            Specification<Common.Models.VaultContact> spec = new(c => c.VaultId == vaultId && c.Ispro);
            spec.ApplyPaging(0, 5);
            this._contacts = await this._contactService.Search(spec);

            this._kinships = Enum.GetValues(typeof(Kinship))
               .Cast<Kinship>()
               .ToDictionary(t => (int)t, t => EnumHelper.GetDescription(t));
        }

        public async Task<JsonResult> OnGetContactParticular(string vaultId, int currentPage, int nbElem = 8)
        {
            Specification<Common.Models.VaultContact> spec = new(c => c.VaultId == vaultId && !c.Ispro);

            //Compute nbPages
            int nbContact = await this._contactService.Count(spec);
            int nbPages = (nbContact / nbElem) + (nbContact % nbElem == 0 ? 0 : 1);

            //Paging
            int skip = (currentPage - 1) * nbElem;
            spec.ApplyPaging(skip, nbElem);

            List<Common.Models.VaultContact> contacts = await this._contactService.Search(spec);

            return new JsonResult(new
            {
                contacts,
                nbPages
            });
        }
    }
}
