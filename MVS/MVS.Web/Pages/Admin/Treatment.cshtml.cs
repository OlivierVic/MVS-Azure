using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace MVS.Web.Pages.Admin
{
    [Authorize(Roles = "SuperAdmin")]
    public class TreatmentModel : PageModel
    {
        private readonly IVaultService _folderService;

        public int _folderPaymentNumber { get; set; }
        public int _folderAskDeleteNumber { get; set; }

        public TreatmentModel(IVaultService folderService) => this._folderService = folderService;

        public async Task OnGet()
        {
            List<Common.Models.Vault> folderAskDelete = await this._folderService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = folderAskDelete.Count;
        }

        public async Task<JsonResult> OnGetFolders(int currentPage, int nbElem = 8, int sort = (int)FolderSort.Name)
        {
            //Compute nbPages
            int nbFolders = 0/*= await this._folderService.Count(spec)*/;
            int nbPages = (nbFolders / nbElem) + (nbFolders % nbElem == 0 ? 0 : 1);

            //Paging
            int skip = (currentPage - 1) * nbElem;
            /*spec.ApplyPaging(skip, nbElem);*/

            List<Common.Models.Vault> folders = null/*= await this._folderService.Search(spec)*/;

            return new JsonResult(new
            {
                folders,
                nbPages
            });
        }

        /*public async Task OnPostUpdateFolderArchiveStatus(string folderId, bool archive)
        {
            Common.Models.Vault folder = await this._folderService.Get(new Specification<Common.Models.Vault>(f => f.Id == folderId));
            folder.IsArchived = archive;

            await this._folderService.Update(folder);
        }*/

        public async Task<IActionResult> OnPostDeleteFolder(string folderId)
        {
            Common.Models.Vault folder = await this._folderService.Get(new Specification<Common.Models.Vault>(f => f.Id == folderId));

            await this._folderService.DeleteVault(folder.Id);

            return this.StatusCode((int)HttpStatusCode.OK, null);
        }
    }
}
