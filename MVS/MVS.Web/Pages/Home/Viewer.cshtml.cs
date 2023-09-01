using MVS.Common.Interfaces;
using MVS.Common.Models;
using MVS.Common.Specifications;
using MVS.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smartclause.SDK;

namespace MVS.Web.Pages.Home
{
    public class ViewerViewModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IVaultDocumentService _folderDocumentService;
        private readonly AzureFileHelper _azureFileHelper;
        private readonly Client _client;

        public ViewerViewModel(Client client, IVaultDocumentService folderDocumentService, IConfiguration configuration)
        {
            this._client = client;
            this._folderDocumentService = folderDocumentService;
            this._configuration = configuration;
            this._azureFileHelper = new AzureFileHelper(this._configuration);
        }

        public async Task<IActionResult> OnGet(string contractId = null, string folderDocumentId = null, string doctorDepartment = null)
        {
            if (contractId != null)
            {
                byte[] pdfContent = await this._client.GetContractAsPDFAsByteArray(contractId);
                return this.File(pdfContent, "application/pdf");
            }

            VaultDocument folderDocument = await this._folderDocumentService.Get(new Specification<VaultDocument>(fd => fd.Id == folderDocumentId));
            if (folderDocument != null)
            {
                byte[] pdfContent = await this._azureFileHelper.GetUserFolderDocument(folderDocument.VaultId, folderDocument.FileName, folderDocument.Type);
                return this.File(pdfContent, "application/pdf");
            }

            if (doctorDepartment != null)
            {
                byte[] pdfContent = await this._azureFileHelper.GetDoctorsList(doctorDepartment);
                return this.File(pdfContent, "application/pdf");
            }

            throw new ArgumentException("Le document n'existe pas");
        }
    }
}
