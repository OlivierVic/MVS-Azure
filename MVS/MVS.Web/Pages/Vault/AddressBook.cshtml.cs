using MVS.Business;
using MVS.Common;
using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Models;
using MVS.Common.Specifications;
using MVS.EmailSender.Sender;
using MVS.EmailSender.Templates.Models;
using MVS.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smartclause.SDK;
using System.Net;
using System.Security.Claims;

namespace MVS.Web.Pages.Vault
{
    public class AddressBookModel : PageModel
    {
        private readonly IAccessService<Common.Models.Vault> _accessService;
        private readonly IVaultService _folderService;
        private readonly IVaultContactService _contactService;
        private readonly IJobProfessionelService _jobProfessionelService;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Client _client;
        private readonly IVaultDocumentService _folderDocumentService;
        public string _folderId { get; set; }
        public Common.Models.Vault folder { get; set; }
        public Common.Models.Vault _foldertest { get; set; }
        public bool _isImmediateProtection { get; set; }
        public List<Common.Models.VaultContact> _contactsPro { get; set; }
        public Common.Models.Vault _folderCompletedContact { get; set; }
        public List<Common.Models.VaultContact> _contactsParticular { get; set; }
        public List<JobProfessionel> _jobs { get; set; }
        public Dictionary<int, string> _kinships { get; set; }

        public string _returnUrl { get; set; }
        public Dictionary<string, string> _folderInfoHeader { get; set; }
        public Dictionary<string, string> _breadcrumb { get; set; }
        private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public int _folderPaymentNumber { get; set; }
        public int _folderAskDeleteNumber { get; set; }


        public AddressBookModel(IVaultService folderService, IVaultContactService contactService, IJobProfessionelService jobService, IConfiguration configuration,
                                    IEmailSender emailSender, UserManager<ApplicationUser> userManager, Client client, IVaultDocumentService folderDocumentService)
        {
            this._folderService = folderService;
            this._contactService = contactService;
            this._jobProfessionelService = jobService;
            this._configuration = configuration;
            this._accessService = new AccessService<Common.Models.Vault>(this._configuration);
            this._emailSender = emailSender;
            this._userManager = userManager;
            this._client = client;
            this._folderDocumentService = folderDocumentService;
        }

        public async Task<IActionResult> OnGetAsync(string vaultId)
        {
            await this._accessService.CheckAccess(vaultId, this._userId, this.User.IsInRole("SuperAdmin"));

            this._folderPaymentNumber = 0;

            List<Common.Models.Vault> folderAskDelete = await this._folderService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = folderAskDelete.Count;

            await this.CheckContactAdvice(vaultId);
            this._folderId = vaultId;

            Common.Models.Vault folder = await this._folderService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));
            this.folder = await this._folderService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));

            this._breadcrumb = new Dictionary<string, string>();
            this._breadcrumb.Add("/Vault", "Mes dossiers");
            this._breadcrumb.Add(this.Url.Page("/Vault/Vault", new { vaultId }), $"Dossier N°{this.folder.Title}");
            this._breadcrumb.Add(this.Url.Page("/Vault/AddressBook", new { vaultId }), "Carnet d'adresses");

            this._folderInfoHeader = VaultInfosHelper.GetFolderInfoHeader(this.folder);
            
            this._foldertest = await this._folderService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));

            Specification<Common.Models.VaultContact> specPro = new(c => c.VaultId == vaultId && c.Ispro);
            specPro.ApplyPaging(0, 5);
            this._contactsPro = await this._contactService.Search(specPro);

            Specification<Common.Models.VaultContact> specParticular = new(c => c.VaultId == vaultId && !c.Ispro);
            specParticular.ApplyPaging(0, 5);
            this._contactsParticular = await this._contactService.Search(specParticular);

            this._jobs = await this._jobProfessionelService.Search(new Specification<JobProfessionel>(j => true));

            this._returnUrl = this.Url.Page("/Vault/Vault", new { vaultId });

            this._kinships = Enum.GetValues(typeof(Kinship))
               .Cast<Kinship>()
               .ToDictionary(t => (int)t, t => EnumHelper.GetDescription(t));

            return this.Page();
        }

        public async Task<IActionResult> OnPostCompleted(string vaultId, bool completedContactProfessional, bool completedContactProche)
        {
            this._folderCompletedContact = await this._folderService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));

            await this._folderService.Update(_folderCompletedContact);

            return this.StatusCode((int)HttpStatusCode.OK, null);
        }

        public async Task<IActionResult> OnPostCreateEmptyContactPro(int jobId, string otherJob, string vaultId, int typeContact = 0)
        {
            Common.Models.VaultContact contact = new();

            contact.Id = Guid.NewGuid().ToString();
            contact.VaultId = vaultId;
            contact.Accompaniment = 0;
            contact.CompletedForm = false;
            contact.Ispro = true;
            contact.Job = jobId;
            contact.OtherJob = otherJob;
            contact.TypeContact = typeContact;

            await this._contactService.Add(contact);

            return this.StatusCode((int)HttpStatusCode.OK, null);
        }

        public async Task<IActionResult> OnPostCreateEmptyContactProRequired(int jobId, string otherJob, string vaultId, bool canBeMultiple = true)
        {
            if (!await this._contactService.Any(new Specification<Common.Models.VaultContact>(cpo => cpo.Job == jobId && cpo.VaultId == vaultId)) || canBeMultiple)
            {
                if (await this._contactService.Any(new Specification<Common.Models.VaultContact>(cpo => cpo.Job == jobId && cpo.VaultId == vaultId && cpo.OtherJob == otherJob)))
                {
                    return this.StatusCode((int)HttpStatusCode.OK, null);
                }
                else
                {
                    Common.Models.VaultContact contact = new();

                    contact.Id = Guid.NewGuid().ToString();
                    contact.VaultId = vaultId;
                    contact.Accompaniment = 0;
                    contact.CompletedForm = false;
                    contact.Ispro = true;
                    contact.Job = jobId;
                    contact.OtherJob = otherJob;
                    contact.TypeContact = 0;

                    await this._contactService.Add(contact);
                }
            }

            return this.StatusCode((int)HttpStatusCode.OK, null);
        }

        public async Task<IActionResult> OnPostCreateEmptyContactParticular(int kinship, string other, string vaultId)
        {
            Common.Models.VaultContact contactP = new();

            contactP.Id = Guid.NewGuid().ToString();
            contactP.VaultId = vaultId;
            contactP.Accompaniment = 0;
            contactP.CompletedForm = false;
            contactP.Ispro = false;
            contactP.Job = 30;
            contactP.Kinship = kinship;
            contactP.Other = other;
            contactP.RelationshipQuality = 3;
            contactP.RelationshipFrequencies = 4;
            contactP.TypeContact = 0;

            await this._contactService.Add(contactP);

            return this.StatusCode((int)HttpStatusCode.OK, null);
        }

        public async Task<IActionResult> OnPostCreateEmptyContactParticularRequired(int kinship, string other, string vaultId, bool canBeMultiple = true)
        {
            if (!await this._contactService.Any(new Specification<Common.Models.VaultContact>(cpo => cpo.Kinship == kinship && cpo.VaultId == vaultId)) || canBeMultiple)
            {
                if (await this._contactService.Any(new Specification<Common.Models.VaultContact>(cpo => cpo.Kinship == kinship && cpo.VaultId == vaultId && cpo.Other == other)))
                {
                    return this.StatusCode((int)HttpStatusCode.OK, null);
                }
                else
                {
                    Common.Models.VaultContact contactP = new();

                    contactP.Id = Guid.NewGuid().ToString();
                    contactP.VaultId = vaultId;
                    contactP.Accompaniment = 0;
                    contactP.CompletedForm = false;
                    contactP.Ispro = false;
                    contactP.Kinship = kinship;
                    contactP.Other = other;
                    contactP.Job = 30;
                    contactP.RelationshipQuality = 3;
                    contactP.RelationshipFrequencies = 4;
                    contactP.TypeContact = 0;

                    await this._contactService.Add(contactP);
                }
            }

            return this.StatusCode((int)HttpStatusCode.OK, null);
        }

        public async Task<IActionResult> OnPostSendAdvice(string contactId)
        {
            Common.Models.VaultContact contact = await this._contactService.Get(new Specification<Common.Models.VaultContact>(c => c.Id == contactId));
            Common.Models.Vault folder = await this._folderService.Get(new Specification<Common.Models.Vault>(f => f.Id == contact.VaultId));
            ApplicationUser user = await this._userManager.GetUserAsync(this.User);

            string contractId = await ContactAdviceHelper.GenerateContractAdvice(this._configuration, this._client, folder, user);
            contact.AdviceContractId = contractId;
            contact.AdviceStatus = (int)ContactAdviceStatus.Send;
            await this._contactService.Update(contact);

            VaultDocument documentToCreate = new()
            {
                Id = Guid.NewGuid().ToString(),
                VaultId = contact.VaultId,
                Type = (int)FolderDocumentTypeEnum.ContactAdvice,
                TypeName = "Avis contact",
                Name = $"Avis - {contact.FirstName} {contact.LastName}",
                ContractId = contractId,
            };
            await this._folderDocumentService.Add(documentToCreate);

            string fullName = $"{folder.LastName} {folder.FirstName}";
            string fullNameRequester = fullName;
            
            string formUrl = this.Url.PageLink("/SCM/Index", "", new { scmContractId = contractId, showForm = true }, this.Request.Scheme);
            string plateformUrl = this.Url.PageLink("Index", "Home", null, this.Request.Scheme);

            await this._emailSender.SendEmailAsync(contact.Email, new ContactAdviceTemplate
            {
                Subject = $"Avis entourage {fullName}",
                Model = new ContactAdviceTemplateModel
                {
                    FullName = fullName,
                    FullNameRequester = fullNameRequester,
                    FormUrl = formUrl,
                    PlateformUrl = plateformUrl
                }
            });

            return this.StatusCode((int)HttpStatusCode.OK, null);
        }

        private async Task CheckContactAdvice(string vaultId)
        {
            List<Common.Models.VaultContact> contacts = await this._contactService.Search(new Specification<Common.Models.VaultContact>(c => c.AdviceStatus == (int)ContactAdviceStatus.Send));
            foreach (var contact in contacts)
            {
                Smartclause.SDK.DTO.ContractDto contract = await this._client.GetContract(contact.AdviceContractId);
                if (contract.Sign ?? false)
                {
                    contact.AdviceStatus = (int)ContactAdviceStatus.Given;
                    await this._contactService.Update(contact);
                }
            }
        }
    }
}
