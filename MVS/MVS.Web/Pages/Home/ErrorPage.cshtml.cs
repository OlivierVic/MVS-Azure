using MVS.Business;
using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MVS.Web.Pages.Home
{
    public class ErrorPageModel : PageModel
    {
        private readonly IVaultService _folderService;

        public int _folderPaymentNumber { get; set; }

        public ErrorPageModel(IVaultService folderService)
        {
            this._folderService = folderService;
        }

        public async Task OnGet()
        {
        }
    }
}
