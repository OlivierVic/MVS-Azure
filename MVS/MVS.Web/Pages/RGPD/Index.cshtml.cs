using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;

namespace MVS.Web.Pages.RGPD
{
    public class IndexModel : PageModel
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IVaultService _folderService;

        public string _fileURL { get; set; }
        public Dictionary<string, string> _breadcrumb { get; set; }
        public int _folderPaymentNumber { get; set; }
        public int _folderAskDeleteNumber { get; set; }

        public IndexModel(IWebHostEnvironment webHostEnvironment, IVaultService folderService)
        {
            this._webHostEnvironment = webHostEnvironment;
            this._folderService = folderService;
        }

        public async Task OnGet()
        {
            this._fileURL = this.Url.PageLink("/RGPD/Index", "File");

            List<Common.Models.Vault> folderAskDelete = await this._folderService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = folderAskDelete.Count;

            this._breadcrumb = new Dictionary<string, string>();
            this._breadcrumb.Add("/Home", "Accueil");
            this._breadcrumb.Add("/RGPD", "RGPD");
        }

        public IActionResult OnGetFile()
        {
            string webRootPath = this._webHostEnvironment.WebRootPath;
            string filePath = Path.Combine(webRootPath, "documents", "20220722 Charte RGPD Alix Care.pdf");
            WebClient webClient = new WebClient();
            byte[] buffer = webClient.DownloadData(filePath);

            return this.File(buffer, "application/pdf");
        }
    }
}
