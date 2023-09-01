using MVS.Common.Interfaces;
using MVS.Common.Specifications;
using MVS.Common.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MVS.Web.Pages.LegalNotice
{
    public class IndexModel : PageModel
    {
        private readonly IVaultService _folderService;

        public Dictionary<string, string> _breadcrumb { get; set; }
        public int _folderPaymentNumber { get; set; }
        public int _folderAskDeleteNumber { get; set; }

        public IndexModel(IVaultService folderService)
        {
            this._folderService = folderService;
        }

        public async Task OnGet()
        {

            List<Common.Models.Vault> folderAskDelete = await this._folderService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = folderAskDelete.Count;

            this._breadcrumb = new Dictionary<string, string>();
            this._breadcrumb.Add("/Home", "Accueil");
            this._breadcrumb.Add("/LegalNotice", "Mentions légales");
        }
    }
}
