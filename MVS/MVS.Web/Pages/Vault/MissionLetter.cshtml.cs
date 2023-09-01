using MVS.Business;
using MVS.Common;
using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Models;
using MVS.Common.Specifications;
using MVS.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Smartclause.SDK;
using Smartclause.SDK.DTO;
using System.Security.Claims;

namespace MVS.Web.Pages.Vault;

[Authorize]
public class MissionLetterModel : PageModel
{
    private IConfiguration _configuration;
    private readonly IAccessService<Common.Models.Vault> _accessService;
    //private readonly ICounterVaultCreateService _counterVaultCreateService;
    private readonly IVaultService _folderService;
    private readonly IVaultContactService _contactService;
    private readonly IVaultContractService _folderContractService;
    private readonly Client _client;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJobProfessionelService _jobProfessionelService;

    public int _counterFacture { get; set; }
    public Common.Models.Vault _folder { get; set; }
    public Dictionary<string, string> _breadcrumb { get; set; }
    public string _MissionLetterId { get; set; }
    private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
    public string _folderId { get; set; }
    //public CounterVaultCreate _nbFolder { get; set; }
    public List<Common.Models.VaultContact> _contacts { get; set; }
    public ContractDto _MissionLetterContract { get; set; }
    public ContractDto _BillContract { get; set; }
    public ContractDto _SEPAContract { get; set; }
    public List<JobProfessionel> _jobs { get; set; }
    public Dictionary<int, string> _Kinships { get; set; }
    public Dictionary<string, string> _folderInfoHeader { get; set; }
    public int _folderPaymentNumber { get; set; }
    public int _folderAskDeleteNumber { get; set; }

    public MissionLetterModel(IVaultService folderService, IConfiguration configuration,
                            IVaultContractService folderContractService, IVaultContactService contactService,
                            Client client, UserManager<ApplicationUser> userManager, IJobProfessionelService jobService/*, ICounterVaultCreateService counterFolderCreateService*/)
    {
        this._configuration = configuration;
        this._accessService = new AccessService<Common.Models.Vault>(this._configuration);
        this._folderService = folderService;
        this._folderContractService = folderContractService;
        this._client = client;
        this._userManager = userManager;
        this._contactService = contactService;
        this._jobProfessionelService = jobService;
        //this._counterVaultCreateService = counterFolderCreateService;
    }

    public async Task<IActionResult> OnGetAsync(string folderId)
    {
        await this._accessService.CheckAccess(folderId, this._userId, this.User.IsInRole("SuperAdmin"));


        List<Common.Models.Vault> folderAskDelete = await this._folderService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
        this._folderAskDeleteNumber = folderAskDelete.Count;

        this._folderId = folderId;
        Specification<Common.Models.Vault> spec = new(f => f.Id == folderId);
        spec.Includes.Add(f => f.VaultContracts);
        this._folder = await this._folderService.Get(spec);

        this._breadcrumb = new Dictionary<string, string>();
        this._breadcrumb.Add("/Vault", "Mes dossiers");
        this._breadcrumb.Add(this.Url.Page("/Vault/Vault", new { folderId }), $"Dossier N°{this._folder.Title}");

        this._folderInfoHeader = VaultInfosHelper.GetFolderInfoHeader(this._folder);

        this._contacts = await this.GetFolderContacts(this._folder);

        this._jobs = await this._jobProfessionelService.Search(new Specification<JobProfessionel>(j => true));
        this._Kinships = Enum.GetValues(typeof(Kinship))
           .Cast<Kinship>()
           .ToDictionary(t => (int)t, t => EnumHelper.GetDescription(t));

        //Check Sepa
        VaultContract sepa = this._folder.VaultContracts.FirstOrDefault(f => f.ContractType == (int)FolderContractType.SEPA);
        if (sepa != null)
        {
            this._SEPAContract = await this._client.GetContract(sepa.ContractId, this._configuration.GetValue<string>($"SCM:TenantId"));
        }

        ApplicationUser user = await this._userManager.GetUserAsync(this.User);
        //Check MissionLetter
        VaultContract missionLetter = this._folder.VaultContracts.FirstOrDefault(f => f.ContractType == (int)FolderContractType.MissionLetter);
        if (missionLetter == null)
        {

            string contractId = await MissionLetterHelper.GenerateMissionLetter(this._configuration, this._client, this._folder, user, this._contacts.FirstOrDefault());
            missionLetter = await this.CreateFolderContract(folderId, contractId, (int)FolderContractType.MissionLetter);

        }
        this._MissionLetterContract = await this._client.GetContract(missionLetter.ContractId, this._configuration.GetValue<string>($"SCM:TenantId"));

        await this._folderService.Update(this._folder);


        // Check Bill
        VaultContract bill = this._folder.VaultContracts.FirstOrDefault(f => f.ContractType == (int)FolderContractType.Bill);
        if (this._MissionLetterContract.Sign ?? false)
        {
            if (bill == null)
            {
                DateTime date = DateTime.Now;

                //this._nbFolder = await this._counterVaultCreateService.Get(new Specification<CounterVaultCreate>(f => true));

                int nbBillNumberMonthYear = (await this._folderContractService.Count(new Specification<VaultContract>(fc => fc.BillNumber == Int32.Parse($"{date.Month}{date.Year}")))) + this._counterFacture;
                string nbBillNumberMonthYearString = null;
                int nbMonth = Int32.Parse($"{date.Month}");
                string nbMonthString = null;
                if (nbBillNumberMonthYear > 100)
                {
                    string tmpNbBillNumberMonthYear = nbBillNumberMonthYear.ToString();
                    nbBillNumberMonthYearString = $"{tmpNbBillNumberMonthYear}";
                }
                else if (nbBillNumberMonthYear > 10)
                {
                    string tmpNbBillNumberMonthYear = nbBillNumberMonthYear.ToString();
                    nbBillNumberMonthYearString = $"0{tmpNbBillNumberMonthYear}";
                }
                else
                {
                    string tmpNbBillNumberMonthYear = nbBillNumberMonthYear.ToString();
                    nbBillNumberMonthYearString = $"00{tmpNbBillNumberMonthYear}";
                }

                if (nbMonth < 10)
                {
                    string tmpNbMonth = nbMonth.ToString();
                    nbMonthString = $"0{tmpNbMonth}";
                }

                int billNumber = Int32.Parse($"{date.Year}{nbMonthString}{nbBillNumberMonthYearString}");

                string contractId = await BillHelper.GenerateBill(this._configuration, this._client, this._folder, this._contacts.FirstOrDefault(), user, billNumber);
                bill = await this.CreateFolderContract(folderId, contractId, (int)FolderContractType.Bill, billNumber);
            }

            this._BillContract = await this._client.GetContract(bill.ContractId, this._configuration.GetValue<string>($"SCM:TenantId"));
        }

        return this.Page();
    }

    public async Task<FileContentResult> OnGetDownloadContract(string contractId)
    {
        ContractDto missionLetter = await this._client.GetContract(contractId, this._configuration.GetValue<string>("SCM:TenantId"));
        byte[] pdfBytes = await this._client.GetContractAsPDFAsByteArray(contractId);

        return this.File(pdfBytes, "application/pdf", $"{missionLetter.Title}.pdf");
    }

    private async Task<VaultContract> CreateFolderContract(string folderId, string contractId, int type, int? billNumber = null)
    {
        VaultContract folderContract = new()
        {
            Id = Guid.NewGuid().ToString(),
            VaultId = folderId,
            ContractId = contractId,
            ContractType = type,
            BillNumber = billNumber
        };

        folderContract = await this._folderContractService.Add(folderContract);
        return folderContract;
    }

    private async Task<List<Common.Models.VaultContact>> GetFolderContacts(Common.Models.Vault folder)
    {
        List<Common.Models.VaultContact> contacts = new();

        Common.Models.VaultContact contact = await this._contactService.Get(new Specification<Common.Models.VaultContact>(c => (c.IsFutuAgent ?? false) && c.VaultId == folder.Id));
        contacts.Add(contact);


        return contacts;
    }
}
