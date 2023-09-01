using MVS.Business;
using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Specifications;
using MVS.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smartclause.SDK;

namespace MVS.Web.Pages.Home
{
    public class MaintenanceModel : PageModel
    {
        private readonly IVaultService _folderService;
        public int _folderPaymentNumber { get; set; }
        public int _folderAskDeleteNumber { get; set; }

        public MaintenanceModel(IVaultService folderService)
        {
            this._folderService = folderService;
        }

        public async Task<IActionResult> OnGetAsync()
        {

            List<Common.Models.Vault> folderAskDelete = await this._folderService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = folderAskDelete.Count;

            return this.Page();
        }
    }
}
