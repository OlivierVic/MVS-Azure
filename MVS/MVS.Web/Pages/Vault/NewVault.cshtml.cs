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
using SmartClause.SDK.DTO;
using System.Security.Claims;

namespace MVS.Web.Pages.Vault
{
    [Authorize]
    public class NewVaultModel : PageModel
    {
        private readonly IVaultService _folderService;
        private readonly ICounterVaultCreateService _counterVaultCreateService;
        private readonly IVaultContractService _folderContractService;
        private readonly IConfiguration _configuration;
        private readonly Client _client;
        private readonly UserManager<ApplicationUser> _userManager;

        public CounterVaultCreate _nbVault { get; set; }
        public bool _isRecette { get; set; }
        private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public int _folderPaymentNumber { get; set; }
        public int _folderAskDeleteNumber { get; set; }


        public NewVaultModel(IVaultService folderService, IVaultContractService folderContractService, IConfiguration configuration, Client client, UserManager<ApplicationUser> userManager, ICounterVaultCreateService counterVaultCreateService)
        {
            this._folderService = folderService;
            this._folderContractService = folderContractService;
            this._configuration = configuration;
            this._client = client;
            this._userManager = userManager;
            this._counterVaultCreateService = counterVaultCreateService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            this._isRecette = _configuration.GetValue<bool>("IsRecette");

            List<Common.Models.Vault> folderAskDelete = await this._folderService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = folderAskDelete.Count;

            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync(Common.Models.Vault vault, bool IsRequester = false)
        {
            string vaultName = "";
            DateTime dateNow = DateTime.Now;
            this._nbVault = await this._counterVaultCreateService.Get(new Specification<CounterVaultCreate>(f => true));

            if (this._nbVault == null)
            {
                this._nbVault = await this._counterVaultCreateService.Add(new CounterVaultCreate() { NbVault = 0 });
            }
            int counterNbFolder = this._nbVault.NbVault;

            counterNbFolder++;
            this._nbVault.NbVault = counterNbFolder;
            await this._counterVaultCreateService.Update(this._nbVault);

            vaultName = dateNow.Year.ToString() + "C" + counterNbFolder;

            vault.Id = Guid.NewGuid().ToString();

            vault.Title = $"{vaultName}";

            vault.UserId = this._userId;

            vault.IsArchived = false;
            vault.CreationDate = DateTime.Now;

            vault.VaultUsers = new List<VaultUser>()
            {
                new VaultUser()
                {
                    Id = Guid.NewGuid().ToString(),
                    VaultId = vault.Id,
                    UserId = this._userId,
                }
            };

            vault = await this._folderService.Add(vault);
            return this.RedirectToPage("/Vault/Folder", new { vaultId = vault.Id });
        }
    }
}
