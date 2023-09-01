using MVS.Common;
using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Models;
using MVS.Common.Specifications;
using MVS.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smartclause.SDK;
using Smartclause.SDK.DTO;
using SmartClause.SDK.DTO;
using System.Net;
using System.Security.Claims;

namespace MVS.Web.Pages.SCM
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly Client _client;
        private readonly IAspNetUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVaultContactService _contactService;
        private readonly IVaultService _folderService;

        public int _folderPaymentNumber { get; set; }
        public int _folderAskDeleteNumber { get; set; }
        private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public ContractViewModel _contractViewModel { get; set; }

        public IndexModel (IConfiguration configuration, Client client, IAspNetUserService userService, UserManager<ApplicationUser> userManager, IVaultContactService contactService, IVaultService folderService)
        {
            this._configuration = configuration;
            this._client = client;
            this._userService = userService;
            this._userManager = userManager;
            this._contactService = contactService;
            this._folderService = folderService;
        }

        public async Task<IActionResult> OnGetAsync(string scmContractId, bool showForm = true, string contactId = null)
        {
            LogonResult r = await this._client.LogonExtended();

            ContractDto currentContract = await this._client.GetContractWithConditions(scmContractId, this._configuration.GetValue<string>("SCM:TenantId"), null);

            Workflow contractWorkflow = currentContract.ContractWorkflows.FirstOrDefault();
            int currentStepWorkflow = 0;

            List<Common.Models.Vault> folderAskDelete = await this._folderService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = folderAskDelete.Count;

            Dictionary<string, string> userInfos = new();
            if (contactId != null)
            {
                Common.Models.VaultContact contact = await this._contactService.Get(new Specification<Common.Models.VaultContact>(c => c.Id == contactId));
                userInfos.Add("Id", contact.Id);
                userInfos.Add("Email", contact.Email);
                userInfos.Add("FirstName", contact.FirstName);
                userInfos.Add("LastName", contact.LastName);
            }
            else
            {
                Specification<AspNetUser> spec_user = new Specification<AspNetUser>(u => u.Id == this._userId);
                AspNetUser aspUser = this._userService.Search(spec_user).Result.FirstOrDefault();
                userInfos.Add("Id", aspUser.Id);
                userInfos.Add("Email", aspUser.Email);
                userInfos.Add("FirstName", aspUser.FirstName);
                userInfos.Add("LastName", aspUser.LastName);
            }

            bool isCreator = true;
            string creatorEmail = userInfos["Email"];

            bool isAdmin = true;

            if (contractWorkflow != null)
            {
                currentStepWorkflow = contractWorkflow.Step;
            }

            bool userIsInPreviousWorkflowSteps = currentContract.WorkflowContractSteps.Any(wcs =>
                wcs.StepOrder <= currentStepWorkflow &&
                wcs.WorkflowContractStepUsers.Any(wcsu => wcsu.Email == userInfos["Email"]));

            List<Right> rights = new List<Right>();
            List<ConditionOperator> conditionOperators = new List<ConditionOperator>();
            List<ContributorField> fieldsList = await this._client.GetAllContributorField();

            // We're assuming here that if there is no 403 before, the user must have access to the contract
            if (currentContract.Parties1.All(c => c.PartyEmail != userInfos["Email"]))
            {
                currentContract.Parties1.Add(new Party
                {
                    PartyEmail = userInfos["Email"],
                    PartyFirstName = userInfos["FirstName"],
                    PartyLastName = userInfos["LastName"],
                });
                currentContract.Parties1 = await this._client.SetContractUsers(currentContract.Id, currentContract.Parties1);
            }

            List<Party> partyList = currentContract.Parties1.Select(c =>
            {
                AspNetUser user = this._userService.Get(new Specification<AspNetUser>(x => x.Email == c.PartyEmail)).GetAwaiter().GetResult();
                return new Party()
                {
                    HasAcceptedCGU = c.HasAcceptedCGU,
                    HasSigned = c.HasSigned,
                    HasValidated = c.HasValidated,
                    HasAcceptedDPO = c.HasAcceptedDPO,
                    PartyEmail = c.PartyEmail,
                    PartyFirstName = c.PartyFirstName ?? user.FirstName ?? string.Empty,
                    PartyLastName = c.PartyLastName ?? user.LastName ?? string.Empty,
                    Received = c.Received,
                    Sent = c.Sent,
                    UID = Guid.Parse(c.UID).ToString(),
                };
            }).ToList();

            TemplateAvailableFields availableFields = currentContract.TemplateId != null
                ? await this._client.GetTemplateAvailableFields(currentContract.TemplateId)
                : new TemplateAvailableFields();

            GenerateJwtResponse jwtResponse = await this._client.GenerateJwt(new GenerateJwtRequest
            {
                EndUserId = userInfos["Id"],
                EndUserEmail = userInfos["Email"],
            });

            int startingTab = 1;
            if (!showForm)
            {
                startingTab = 2;
            }

            this._contractViewModel = new ContractViewModel()
            {
                Url = this._configuration.GetValue<string>("SCM:Url"),
                AccessToken = jwtResponse.Token,
                SCMUserId = r.userId,
                Uid = currentContract.Parties1.Find(p => p.PartyEmail == userInfos["Email"]).UID,
                ContractId = scmContractId,
                TemplateTitle = string.Empty,
                ContractTitle = currentContract.Title,
                SentForSignature = currentContract.SentForSignature != null && currentContract.SentForSignature.Value,
                IsSigned = currentContract.Sign != null && currentContract.Sign.Value,
                Parties = currentContract.Parties1 != null
                            ? string.Join(",", currentContract.Parties1.Select(d => d.PartyEmail))
                            : string.Empty,
                PartyList = partyList,
                StartingTab = startingTab,
                WorkflowContractStepsList = currentContract.WorkflowContractSteps,
                UserModel = new WorkflowContractStepUser() { Optional = true },
                RightsEnum = rights,
                RightsSignature = 3,
                ConditionOperatorsEnum = conditionOperators,
                ConditionModel = new Condition(),
                FromFolderId = string.Empty,
                IsAdmin = isAdmin,
                IsCreator = isCreator,
                TemplateAvailableFields = availableFields,
                FieldsList = fieldsList,
                CancelSignError = "Vous devez donner une raison à l'annulation",
                UserId = userInfos["Id"],
                Lang = "fr",
                ViewerUrl = this.Url.Action("Viewer", "Contract"),
                Firstname = userInfos["FirstName"],
                Lastname = userInfos["LastName"],
                Username = userInfos["Email"],
                Email = userInfos["Email"],
                Initials = $"{userInfos["FirstName"].Substring(0, 2)} {userInfos["LastName"].Substring(0, 2)}",
                ReturnUrl = "",
                CreatorEmail = creatorEmail,
            };

            return this.Page();
        }

        public async Task<JsonResult> OnGetGetContractJson(string scmContractId)
        {
            ContractDto contract = await this._client.GetContract(scmContractId);
            return new JsonResult(contract);
        }

        public async Task<IActionResult> OnPostSendToSignature(string contractId)
        {
            ContractDto contract = await this._client.GetContract(contractId);
            Common.Models.VaultContact contact = await this._contactService.Get(new Specification<Common.Models.VaultContact>(c => c.AdviceContractId == contractId));

            SignContractRequest signRequest = new()
            {
                Files = new List<SignContractRequest.FileToSign>(),
                EmailTitle = "[MVS] Vous avez un document à signer",
                EmailSender = "Seraphin.legal",
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

            // Add Signer to request
            UserContact userContact = new()
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                SCMContainerAnchorTag = $"Signature_{contact.Email}",
                Tel = contact.PhoneNumber,
                Title = $"{contact.FirstName} {contact.LastName}",
                Email = contact.Email,
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

            contact.AdviceStatus = (int)ContactAdviceStatus.Send;
            await this._contactService.Update(contact);

            return this.StatusCode((int)HttpStatusCode.OK, null);
        }
    }
}
