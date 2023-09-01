using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Models;
using MVS.Common.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MVS.Web.Pages.Admin
{
    public class AccountManagementModel : PageModel
    {
        private readonly IAspNetUserService _userService;
        private readonly IVaultService _vaultService;

        public int _folderPaymentNumber { get; set; }
        public int _folderAskDeleteNumber { get; set; }

        public AccountManagementModel(IAspNetUserService userService, IVaultService vaultService)
        {
            this._userService = userService;
            this._vaultService = vaultService;
        }

        public async Task OnGet()
        {
            //List<Common.Models.Vault> foldersPayment = await this._vaultService.Search(new Specification<Common.Models.Vault>(v => v.PaymentStatus == (int)PaymentEnum.HasToBeValidated || v.PaymentStatus == (int)PaymentEnum.Payed));
            this._folderPaymentNumber = 0;

            //List<Common.Models.Vault> folderAskDelete = await this._vaultService.Search(new Specification<Common.Models.Vault>(v => v.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = 0;
        }

        public async Task<JsonResult> OnGetUsers(int currentPage, int nbElem = 8, int sort = (int)UserSort.Name, string search = "")
        {
            search = string.IsNullOrEmpty(search) ? string.Empty : search.ToLower();

            Specification<AspNetUser> spec = new(u => u.LastName.Contains(search) || u.FirstName.Contains(search));
            spec.Includes.Add(v => v.Vaults);
            spec.Includes.Add(v => v.Roles);

            //Compute nbPages
            int nbUsers = await this._userService.Count(spec);
            int nbPages = (nbUsers / nbElem) + (nbUsers % nbElem == 0 ? 0 : 1);

            //Paging
            int skip = (currentPage - 1) * nbElem;
            spec.ApplyPaging(skip, nbElem);

            //Sort
            switch (sort)
            {
                case (int)UserSort.NameDesc:
                    spec.ApplyOrderByDescending(u => u.LastName);
                    break;
                case (int)UserSort.NbFolder:
                    spec.ApplyOrderByDescending(u => u.Vaults.Count);
                    break;
                default:
                    spec.ApplyOrderBy(u => u.LastName);
                    break;
            }

            List<AspNetUser> users = await this._userService.Search(spec);

            foreach (AspNetUser user in users)
            {
                foreach (Common.Models.Vault vault in user.Vaults)
                {
                    vault.User = null;
                }

                foreach (AspNetRole role in user.Roles)
                {
                    role.Users = null;
                }
            }

            return new JsonResult(new
            {
                users,
                nbPages
            });
        }
    }
}
