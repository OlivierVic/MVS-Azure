using MVS.Business;
using MVS.Common.Interfaces;
using MVS.Common;
using MVS.Common.Models;
using MVS.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smartclause.SDK;
using System.Security.Claims;
using MVS.EmailSender.Sender;
using MVS.Common.Specifications;
using MVS.Common.Enum;
using Smartclause.SDK.DTO;
using static SmartClause.SDK.DTO.UpdateElementsResponse;
using MVS.Web.Helpers.Volontes;
using System.Diagnostics.Contracts;
using SmartClause.SDK.DTO;
using System.Net;

namespace MVS.Web.Pages.Vault
{
    public class VolontesModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly IVaultService _folderService;
        private readonly IVaultPersonalInfoService _vaultPersonalInfoService;
        private readonly IVaultFamilyInfoService _vaultFamilyInfoService;
        private readonly IEmailSender _emailSender;
        private readonly IAccessService<Common.Models.Vault> _accessService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Client _client;
        private readonly IVaultContractService _folderContractService;
        private readonly IVaultDocumentService _folderDocumentService;
        private readonly AzureFileHelper _azureFileHelper;

        private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public string _folderId { get; set; }
        public Common.Models.Vault _folder { get; set; }
        public VaultPersonalInfo _personalSituation { get; set; }
        public VaultFamilyInfo _familySituation { get; set; }
        public Dictionary<string, string> _breadcrumb { get; set; }
        public Dictionary<string, string> _folderInfoHeader { get; set; }
        public int _folderPaymentNumber { get; set; }
        public int _folderAskDeleteNumber { get; set; }
        public ContractDto _PersonnalSituationContract { get; set; }
        public ContractDto _FamilySituationContract { get; set; }
        public ContractDto _HabitsContract { get; set; }
        public List<VaultDocument> _documents { get; set; }

        public VolontesModel(IVaultService folderService, IConfiguration configuration, IEmailSender emailSender, Client client, UserManager<ApplicationUser> userManager, IVaultContractService folderContractVolontesService, IVaultPersonalInfoService vaultPersonalInfo, IVaultFamilyInfoService folderFamilyInfo, IVaultDocumentService folderDocumentService)
        {
            this._folderService = folderService;
            this._configuration = configuration;
            this._accessService = new AccessService<Common.Models.Vault>(this._configuration);
            this._emailSender = emailSender;
            this._client = client;
            this._userManager = userManager;
            this._folderContractService = folderContractVolontesService;
            this._vaultPersonalInfoService = vaultPersonalInfo;
            this._vaultFamilyInfoService = folderFamilyInfo;
            this._folderDocumentService = folderDocumentService;
            this._azureFileHelper = new AzureFileHelper(this._configuration);
        }

        public async Task OnGetAsync(string folderId)
        {
            await this._accessService.CheckAccess(folderId, this._userId, this.User.IsInRole("SuperAdmin"));

            List<Common.Models.Vault> folderAskDelete = await this._folderService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = folderAskDelete.Count;

            this._folderId = folderId;

            Specification<Common.Models.Vault> specFolder = new(f => f.Id == folderId);
            specFolder.Includes.Add(f => f.VaultContracts);
            specFolder.Includes.Add(f => f.VaultDocuments);
            this._folder = await this._folderService.Get(specFolder);

            this._personalSituation = await this._vaultPersonalInfoService.Get(new Specification<VaultPersonalInfo>(fpi => fpi.VaultId == folderId));
            this._familySituation = await this._vaultFamilyInfoService.Get(new Specification<VaultFamilyInfo>(ffi => ffi.VaultId == folderId));

            this._breadcrumb = new Dictionary<string, string>();
            this._breadcrumb.Add("/Vault", "Mes dossiers");
            this._breadcrumb.Add(this.Url.Page("/Vault/Vault", new { folderId }), $"Dossier N°{this._folder.Title}");

            ApplicationUser user = await this._userManager.GetUserAsync(this.User);

            //Check PersonnalSituation
            VaultContract PersonnalSituation = this._folder.VaultContracts.FirstOrDefault(f => f.ContractType == (int)FolderContractType.VolontePersonnalSituation);
            if (PersonnalSituation == null)
            {
                string contractId = null;
                contractId = await VolonteSituationPersonnelleHelper.GenerateVolonteSituationPersonnelle(this._configuration, this._client, this._folder, user, this._personalSituation);
                PersonnalSituation = await this.CreateFolderContractsVolontes(folderId, contractId, (int)FolderContractType.VolontePersonnalSituation);
            }
            this._PersonnalSituationContract = await this._client.GetContract(PersonnalSituation.ContractId, this._configuration.GetValue<string>($"SCM:TenantId"));

            //Check FamilySituation
            VaultContract FamilySituation = this._folder.VaultContracts.FirstOrDefault(f => f.ContractType == (int)FolderContractType.VolonteFamilySituation);
            if (FamilySituation == null)
            {
                string contractId = null;
                contractId = await VolontesSituationFamilialeHelper.GenerateVolontesSituationFamily(this._configuration, this._client, this._folder, user, this._familySituation);
                FamilySituation = await this.CreateFolderContractsVolontes(folderId, contractId, (int)FolderContractType.VolonteFamilySituation);
            }
            this._FamilySituationContract = await this._client.GetContract(FamilySituation.ContractId, this._configuration.GetValue<string>($"SCM:TenantId"));

            await this.OnPostCreateEmptyDocument(folderId, "Situation personnelle", (int)FolderDocumentTypeEnum.VolontesTMP);
            await this.OnPostCreateEmptyDocument(folderId, "Situation familiale", (int)FolderDocumentTypeEnum.VolontesTMP);
            await this.OnPostCreateEmptyDocument(folderId, "Habitudes de vie", (int)FolderDocumentTypeEnum.VolontesTMP);
            await this.OnPostCreateEmptyDocument(folderId, "Santé", (int)FolderDocumentTypeEnum.VolontesTMP);
            await this.OnPostCreateEmptyDocument(folderId, "Patrimoine", (int)FolderDocumentTypeEnum.VolontesTMP);
            await this.OnPostCreateEmptyDocument(folderId, "Budget", (int)FolderDocumentTypeEnum.VolontesTMP);
            await this.OnPostCreateEmptyDocument(folderId, "Mesures d'anticipations", (int)FolderDocumentTypeEnum.VolontesTMP);
            await this.OnPostCreateEmptyDocument(folderId, "Choix futurs", (int)FolderDocumentTypeEnum.VolontesTMP);
        }

        public async Task<FileContentResult> OnGetDownloadPersonnalSituation(string contractId)
        {
            ContractDto missionLetter = await this._client.GetContract(contractId, this._configuration.GetValue<string>("SCM:TenantId"));
            byte[] pdfBytes = await this._client.GetContractAsPDFAsByteArray(contractId);

            return this.File(pdfBytes, "application/pdf", $"{missionLetter.Title}.pdf");
        }

        public async Task<FileContentResult> OnGetDownloadFamilySituation(string contractId)
        {
            ContractDto missionLetter = await this._client.GetContract(contractId, this._configuration.GetValue<string>("SCM:TenantId"));
            byte[] pdfBytes = await this._client.GetContractAsPDFAsByteArray(contractId);

            return this.File(pdfBytes, "application/pdf", $"{missionLetter.Title}.pdf");
        }

        private async Task<VaultContract> CreateFolderContractsVolontes(string folderId, string contractId, int type)
        {
            VaultContract folderContractsVolontes = new()
            {
                Id = Guid.NewGuid().ToString(),
                VaultId = folderId,
                ContractId = contractId,
                ContractType = type
            };

            folderContractsVolontes = await this._folderContractService.Add(folderContractsVolontes);
            return folderContractsVolontes;
        }

        public async Task<IActionResult> OnGetSendToSignature(string contractId, string folderId)
        {
            Common.Models.Vault folder = await this._folderService.Get(new Specification<Common.Models.Vault>(f => f.Id == folderId));

            ContractDto contract = await this._client.GetContract(contractId);

            SignContractRequest signRequest = new()
            {
                Files = new List<SignContractRequest.FileToSign>(),
                EmailTitle = "[MVS] Vous avez un document à signer",
                EmailSender = "nepasrepondre@accompagne.alix.care",
                ReturnUrl = this.Url.Action("GoBackToDashboard", "Home", null, this.Request.Scheme),
                Contacts = new List<UserContact>(),
                SendEnvelopeDirectly = false,
                SignatureType = (int)SignatureTypeEnum.Simple,
            };

            // Add contract to request
            signRequest.Files.Add(new SignContractRequest.FileToSign
            {
                Id = contractId,
                FileName = contract.Title,
                AddAnchor = false,
            });

            // Ask Signer to request
            UserContact userContact = new()
            {
                FirstName = folder.FirstName,
                LastName = folder.LastName,
                SCMContainerAnchorTag = $"Signature_{folder.Email}",
                Tel = folder.PhoneNumber,
                Title = $"{folder.FirstName} {folder.LastName}",
                Email = folder.Email,
            };
            signRequest.Contacts.Add(userContact);

            SignContractResult signResult = await this._client.SignContract(signRequest, this._configuration.GetValue<string>($"SCM:TenantId"));

            UpdateContractSignInfoRequest updateContractSignInfo = new()
            {
                EnvelopeId = signResult.EnvelopeId,
                SentForSignature = true,
                Sign = false,
            };
            await this._client.UpdateContractEnvelopeId(contractId, updateContractSignInfo);

            return this.RedirectToPage($"/Vault/Volontes", new { folderId });
        }

        public async Task<JsonResult> OnGetDocuments(string folderId, int currentPage, int nbElem = 8)
        {
            int nbDocuments = await this._folderDocumentService.Count(new Specification<VaultDocument>(fd => fd.VaultId == folderId && fd.TypeName == "Volontes"));
            int nbPages = (nbDocuments / nbElem) + (nbDocuments % nbElem == 0 ? 0 : 1);

            Specification<VaultDocument> spec = new(f => f.VaultId == folderId && f.TypeName == "Volontes");
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

        public async Task<IActionResult> OnPostCreateEmptyDocument(string folderId, string name, int type, bool canBeMultiple = true)
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
                        TypeName = "Volontes",
                        Name = name,
                    };

                    await this._folderDocumentService.Add(folderDocument);
                }
            }

            return this.StatusCode((int)HttpStatusCode.OK, null);
        }

        public async Task<IActionResult> OnPostDeleteDocument(string folderId, string documentName, int type, string documentTypeName, string documentId, string documentUrl)
        {
            VaultDocument folderDocument = await this._folderDocumentService.Get(new Specification<VaultDocument>(fd => fd.Type == type && fd.VaultId == folderId && fd.Name == documentName && fd.TypeName == documentTypeName && fd.Id == documentId && fd.Url == documentUrl));

            if (folderDocument != null)
            {
                await this._folderDocumentService.Delete(folderDocument);
            }

            return this.RedirectToPage("/Vault/Volontes", new { folderId = folderId });
        }

        public async Task<FileContentResult> OnGetDownloadDocument(string folderId, string fileName, int type)
        {
            byte[] bytes = await this._azureFileHelper.GetUserFolderDocument(folderId, fileName, type);

            return this.File(bytes, "application/pdf", fileName);
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

            return this.RedirectToPage("/Vault/Volontes", new { folderId = folderId });
        }
    }
}
