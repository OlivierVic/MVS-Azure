using MVS.Business;
using MVS.Common;
using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Models;
using MVS.Common.Specifications;
using MVS.Web.Helpers;
using Aspose.Words;
using FluentEmail.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Net;
using System.Security.Claims;

namespace MVS.Web.Pages.Admin
{
    [Authorize(Roles = "SuperAdmin")]
    public class AddUserFolderModel : PageModel
    {
        private readonly IVaultService _folderService;
        private readonly IVaultUsersService _folderUsersService;
        private readonly IAspNetUserService _userService;
        public int _folderPaymentNumber { get; set; }
        public int _folderAskDeleteNumber { get; set; }
        private readonly UserManager<ApplicationUser> _userManager;
        private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public List<AspNetUser> _user { get; set; }
        public List<Common.Models.Vault> _folder { get; set; }

        public AddUserFolderModel(IVaultService folderService, IAspNetUserService userService, IVaultUsersService folderUsersService)
        {
            this._folderService = folderService;
            this._userService = userService;
            this._folderUsersService = folderUsersService;
        }

        public async Task OnGetAsync()
        {
            this._folderPaymentNumber = 0;

            List<Common.Models.Vault> folderAskDelete = await this._folderService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = folderAskDelete.Count;

            this._user = await this._userService.Search(new Specification<AspNetUser>(u => true));
            this._folder = await this._folderService.Search(new Specification<Common.Models.Vault>(f => true));
        }

        public async Task<IActionResult> OnPostAddUserOnFolder(string UserId, string VaultId)
        {
            string userId = UserId;
            string vaultId = VaultId;
            VaultUser userExist = await this._folderUsersService.Get(new Specification<VaultUser>(ue => ue.UserId == UserId && ue.VaultId == VaultId));

            if (userExist == null)
            {
                VaultUser folderUser = new VaultUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    VaultId = vaultId,
                    UserId = userId,
                };

                await this._folderUsersService.Add(folderUser);
            }
            else
            {
                return this.StatusCode((int)HttpStatusCode.Conflict, null);
            }

            return this.StatusCode((int)HttpStatusCode.OK, null);
        }
    }
}
