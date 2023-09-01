using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MVS.Web.Pages.Home
{
    public class IndexModel : PageModel
    {
        private readonly IVaultService _folderService;
        public int _folderPaymentNumber { get; set; }
        public int _folderAskDeleteNumber { get; set; }
        //public int _folderTraitementNumber { get; set; }

        public IndexModel(IVaultService folderService) => this._folderService = folderService;

        public async Task<IActionResult> OnGetAsync()
        {

            List<Common.Models.Vault> folderAskDelete = await this._folderService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = folderAskDelete.Count;

            return this.Page();
        }
    }
}
