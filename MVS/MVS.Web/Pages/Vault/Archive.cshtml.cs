using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MVS.Web.Pages.Vault
{
    public class ArchiveModel : PageModel
    {
        private readonly IVaultService _folderService;
        private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public int _folderPaymentNumber { get; set; }
        public int _folderAskDeleteNumber { get; set; }


        public ArchiveModel(IVaultService folderService) => this._folderService = folderService;

        public async Task<IActionResult> OnGetAsync()
        {
            List<Common.Models.Vault> folderAskDelete = await this._folderService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = folderAskDelete.Count;

            return this.Page();
        }

        public async Task<JsonResult> OnGetFolders(int currentPage, int nbElem = 8, int sort = (int)FolderSort.Name)
        {
            Specification<Common.Models.Vault> spec = new(f => f.UserId == this._userId && (bool)f.IsArchived);

            //Compute nbPages
            int nbFolders = await this._folderService.Count(spec);
            int nbPages = (nbFolders / nbElem) + (nbFolders % nbElem == 0 ? 0 : 1);

            //Paging
            int skip = (currentPage - 1) * nbElem;
            spec.ApplyPaging(skip, nbElem);

            //Sort
            switch (sort)
            {
                case (int)FolderSort.NameDesc:
                    spec.ApplyOrderByDescending(f => f.LastName);
                    break;
                case (int)FolderSort.CreationDate:
                    spec.ApplyOrderByDescending(f => f.CreationDate);
                    break;
                default:
                    spec.ApplyOrderBy(f => f.LastName);
                    break;
            }

            List<Common.Models.Vault> folders = await this._folderService.Search(spec);

            return new JsonResult(new
            {
                folders,
                nbPages
            });
        }
    }
}
