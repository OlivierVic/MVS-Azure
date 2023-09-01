using MVS.Business;
using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Models;
using MVS.Common.Specifications;
using MVS.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smartclause.SDK;
using Smartclause.SDK.DTO;
using System.IO.Compression;
using System.Net;
using System.Security.Claims;

namespace MVS.Web.Pages.Vault
{
    public class DocumentsModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly AzureFileHelper _azureFileHelper;
        private readonly IVaultService _folderService;
        private readonly IVaultDocumentService _folderDocumentService;
        private readonly IAccessService<Common.Models.Vault> _accessService;
        private readonly Client _client;

        private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public Common.Models.Vault Vault { get; set; }
        public string _returnUrl { get; set; }

        public List<VaultDocument> _documents { get; set; }
        public Common.Models.Vault _folderCompleted { get; set; }
        public string _folderId { get; set; }
        public Dictionary<string, string> _folderInfoHeader { get; set; }
        public Dictionary<string, string> _breadcrumb { get; set; }
        public int _folderPaymentNumber { get; set; }
        public int _folderAskDeleteNumber { get; set; }


        public DocumentsModel(IConfiguration configuration, IVaultService folderService, IVaultDocumentService folderDocumentService, Client client)
        {
            this._configuration = configuration;
            this._azureFileHelper = new AzureFileHelper(this._configuration);
            this._folderService = folderService;
            this._folderDocumentService = folderDocumentService;
            this._accessService = new AccessService<Common.Models.Vault>(this._configuration);
            this._client = client;
        }

        public async Task<IActionResult> OnGetAsync(string folderId)
        {
            await this._accessService.CheckAccess(folderId, this._userId, this.User.IsInRole("SuperAdmin"));

            List<Common.Models.Vault> folderAskDelete = await this._folderService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = folderAskDelete.Count;

            Specification<Common.Models.Vault> spec = new(f => f.Id == folderId);
            this.Vault = await this._folderService.Get(spec);

            this._breadcrumb = new Dictionary<string, string>();
            this._breadcrumb.Add("/Vault", "Mes dossiers");
            this._breadcrumb.Add(this.Url.Page("/Vault/Vault", new { folderId }), $"Dossier N°{this.Vault.Title}");
            this._breadcrumb.Add(this.Url.Page("/Vault/Documents", new { folderId }), "Documents");

            this._folderInfoHeader = VaultInfosHelper.GetFolderInfoHeader(this.Vault);

            this._returnUrl = this.Url.Page("/Vault/Vault", new { folderId });

            await this.OnPostCreateEmptyDocument(folderId, "Copie des deux côtés de la pièce d’identité de la personne concernée", (int)FolderDocumentTypeEnum.IdCard, "Informations personnelles");
            await this.OnPostCreateEmptyDocument(folderId, "Copie du livret de famille de la personne concernée", (int)FolderDocumentTypeEnum.ConcernedFamilyRecordBook, "Informations personnelles");
            //await this.OnPostCreateEmptyDocument(folderId, "Copie des relevés de compte des 3 derniers mois de la personne concernée", (int)FolderDocumentTypeEnum.AccountStatements, "Informations personnelles");
            await this.OnPostCreateEmptyDocument(folderId, "Avis d’imposition sur le revenu de la personne concernée", (int)FolderDocumentTypeEnum.IncomeTaxNotice, "Informations personnelles");



            this._folderId = folderId;

            return this.Page();
        }

        public async Task<JsonResult> OnGetDocuments(string folderId, int currentPage, int nbElem = 15)
        {
            int nbDocuments = await this._folderDocumentService.Count(new Specification<VaultDocument>(fd => fd.VaultId == folderId));
            int nbPages = (nbDocuments / nbElem) + (nbDocuments % nbElem == 0 ? 0 : 1);

            Specification<VaultDocument> spec = new(f => f.VaultId == folderId);
            spec.ApplyOrderBy(fd => fd.Type);

            int skip = (currentPage - 1) * nbElem;
            spec.ApplyPaging(skip, nbElem);

            List<VaultDocument> documents = await this._folderDocumentService.Search(spec);

            return new JsonResult(new
            {
                documents,
                nbPages
            });
        }

        public async Task<IActionResult> OnPostAsync(string folderId, IFormFile file, int type, string typeName, string folderDocumentId, string name = null)
        {
            VaultDocument folderDocument = new();

            if (folderDocumentId != null)
            {
                folderDocument = await this._folderDocumentService.Get(new Specification<VaultDocument>(fd => fd.Id == folderDocumentId));
            }

            string documentUrl = await this._azureFileHelper.UploadFolderDocument(folderId, file.FileName, file.OpenReadStream(), type);
            folderDocument.Url = documentUrl;
            folderDocument.FileName = file.FileName;

            if (name != null)
            {
                folderDocument.Name = name;
            }

            if (folderDocument.Id == null)
            {
                folderDocument.Id = Guid.NewGuid().ToString();
                folderDocument.VaultId = folderId;
                folderDocument.Type = type;
                folderDocument.TypeName = typeName;
                await this._folderDocumentService.Add(folderDocument);
            }
            else
            {
                await this._folderDocumentService.Update(folderDocument);
            }

            return this.RedirectToPage("/Vault/Documents", new { folderId = folderId });
        }

        public async Task<IActionResult> OnPostCompleted(string folderId, bool completedDocument)
        {
            this._folderCompleted = await this._folderService.Get(new Specification<Common.Models.Vault>(f => f.Id == folderId));

            await this._folderService.Update(_folderCompleted);

            return this.StatusCode((int)HttpStatusCode.OK, null);
        }

        public async Task<FileContentResult> OnGetDownloadDocument(string folderId, string fileName, int type)
        {
            byte[] bytes = await this._azureFileHelper.GetUserFolderDocument(folderId, fileName, type);

            return this.File(bytes, "application/pdf", fileName);
        }

        public async Task<FileContentResult> OnGetDownloadContract(string contractId)
        {
            ContractDto contract = await this._client.GetContract(contractId);
            byte[] bytes = await this._client.GetContractAsPDFAsByteArray(contractId);

            return this.File(bytes, "application/pdf", $"{contract.Title}.pdf");
        }

        public async Task<IActionResult> OnGetDownloadAllFolderDocument(string folderId)
        {
            Specification<Common.Models.Vault> spec = new(f => f.Id == folderId);
            spec.Includes.Add(f => f.VaultDocuments);
            Common.Models.Vault folder = await this._folderService.Get(spec);

            using MemoryStream zipStream = new();
            using (ZipArchive zip = new(zipStream, ZipArchiveMode.Create, true))
            {
                foreach (VaultDocument doc in folder.VaultDocuments)
                {
                    if (doc.FileName != null)
                    {
                        byte[] docData = await this._azureFileHelper.GetUserFolderDocument(folderId, doc.FileName, doc.Type);

                        ZipArchiveEntry zipEntry = zip.CreateEntry($"{doc.Name}.pdf");
                        using Stream entryStream = zipEntry.Open();

                        using StreamWriter streamWriter = new StreamWriter(entryStream);
                        streamWriter.BaseStream.Write(docData, 0, docData.Length);
                    }
                }
            }

            return this.File(zipStream.ToArray(), "application/zip", $"Documents - {folder.FirstName} {folder.LastName}.zip");
        }

        public async Task<IActionResult> OnPostCreateEmptyDocument(string folderId, string name, int type, string typeName, bool canBeMultiple = false)
        {
            if (!await this._folderDocumentService.Any(new Specification<VaultDocument>(fd => fd.Type == type && fd.VaultId == folderId)) || canBeMultiple)
            {
                if (await this._folderDocumentService.Any(new Specification<VaultDocument>(fd => fd.Type == type && fd.VaultId == folderId && fd.Name == name)))
                {
                    return this.StatusCode((int)HttpStatusCode.OK, null);
                }
                else
                {
                    VaultDocument folderDocument = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        VaultId = folderId,
                        Type = type,
                        TypeName = typeName,
                        Name = name,
                    };

                    await this._folderDocumentService.Add(folderDocument);
                }
            }

            return this.StatusCode((int)HttpStatusCode.OK, null);
        }

        public async Task<IActionResult> OnPostAddNewEmptyDocument(string folderId, string documentName, int type, string documentTypeName, bool canBeMultiple = true)
        {
            if (!await this._folderDocumentService.Any(new Specification<VaultDocument>(fd => fd.Type == type && fd.VaultId == folderId)) || canBeMultiple)
            {
                if (await this._folderDocumentService.Any(new Specification<VaultDocument>(fd => fd.Type == type && fd.VaultId == folderId && fd.Name == documentName)))
                {
                    return this.StatusCode((int)HttpStatusCode.OK, null);
                }
                else
                {
                    VaultDocument folderDocument = new()
                    {
                        Id = Guid.NewGuid().ToString(),
                        VaultId = folderId,
                        Type = type,
                        TypeName = documentTypeName,
                        Name = documentName,
                    };

                    await this._folderDocumentService.Add(folderDocument);
                }
            }

            return this.RedirectToPage("/Vault/Documents", new { folderId = folderId });
        }

        public async Task<IActionResult> OnPostDeleteDocument(string folderId, string documentName, int type, string documentTypeName, string documentId, string documentUrl)
        {
            VaultDocument folderDocument = await this._folderDocumentService.Get(new Specification<VaultDocument>(fd => fd.Type == type && fd.VaultId == folderId && fd.Name == documentName && fd.TypeName == documentTypeName && fd.Id == documentId && fd.Url == documentUrl));

            if (folderDocument != null)
            {
                await this._folderDocumentService.Delete(folderDocument);
            }

            return this.RedirectToPage("/Vault/Documents", new { folderId = folderId });
        }

        public async Task<FileContentResult> OnGetDownloadListFolderDocumentPdf(string folderId)
        {
            string title = "Liste des documents";

            this._documents = await this._folderDocumentService.Search(new Specification<VaultDocument>(fd => fd.VaultId == folderId));

            Specification<Common.Models.Vault> spec = new(f => f.Id == folderId);
            spec.Includes.Add(f => f.VaultPersonalInfos);
            Common.Models.Vault folder = await this._folderService.Get(spec);

            string listdocument = this.CreateStringFromListDocument(this._documents);
            return this.File(VaultInfosHelper.BuildFolderInfosPdf(folder, listdocument, title), "application/pdf", $"Liste documents - {folder.FirstName} {folder.LastName}.pdf");
        }

        private string CreateStringFromListDocument(List<VaultDocument> documents)
        {
            string listString = string.Empty;

            if (documents != null)
            {
                listString += "Documents :\n\n";

                foreach (var document in documents)
                {
                    listString += "   - ";
                    listString += document.Name;
                    listString += "\n";
                }
            }

            return listString;
        }
    }
}