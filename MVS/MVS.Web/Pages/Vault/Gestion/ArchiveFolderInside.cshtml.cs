using MVS.Business;
using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Security.Claims;

namespace MVS.Web.Pages.Vault
{
    public class ArchiveFolderInsideModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IVaultService _folderService;
        private readonly IAccessService<Common.Models.Vault> _accessService;

        private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public ArchiveFolderInsideModel(IVaultService folderService, IConfiguration configuration)
        {
            this._configuration = configuration;
            this._accessService = new AccessService<Common.Models.Vault>(this._configuration);
            this._folderService = folderService;
        }

        public async Task<IActionResult> OnGet(string vaultId)
        {
            await this._accessService.CheckAccess(vaultId, this._userId, this.User.IsInRole("SuperAdmin"));

            Common.Models.Vault folder = await this._folderService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));
            folder.IsArchived = true;

            await this._folderService.Update(folder);

            return this.RedirectToPage("/Vault/Index");
        }
    }
}
