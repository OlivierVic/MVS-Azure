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
using System.Net;

namespace MVS.Web.Pages.Vault
{
    [Authorize]
    public class PutVaultModel : PageModel
    {
        private readonly IVaultService _vaultService;
        private readonly ICounterVaultCreateService _counterVaultCreateService;
        private readonly IVaultContractService _folderContractService;
        private readonly IConfiguration _configuration;
        private readonly Client _client;
        private readonly UserManager<ApplicationUser> _userManager;

        public CounterVaultCreate _nbVault { get; set; }
        public bool _isRecette { get; set; }
        public string _vaultId { get; set; }
        public Common.Models.Vault _vault { get; set; }
        private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public int _folderPaymentNumber { get; set; }
        public int _folderAskDeleteNumber { get; set; }


        public PutVaultModel(IVaultService vaultService, IVaultContractService folderContractService, IConfiguration configuration, Client client, UserManager<ApplicationUser> userManager, ICounterVaultCreateService counterVaultCreateService)
        {
            this._vaultService = vaultService;
            this._folderContractService = folderContractService;
            this._configuration = configuration;
            this._client = client;
            this._userManager = userManager;
            this._counterVaultCreateService = counterVaultCreateService;
        }

        public async Task OnGetAsync(string vaultId)
        {
            this._isRecette = _configuration.GetValue<bool>("IsRecette");

            List<Common.Models.Vault> folderAskDelete = await this._vaultService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = folderAskDelete.Count;

            this._vaultId = vaultId;
            this._vault = await this._vaultService.Get(new Specification<Common.Models.Vault>(v => v.Id == vaultId));
        }

        public async Task<IActionResult> OnPostAsync(Common.Models.Vault vault)
        {
            await this._vaultService.Update(vault);
            //return this.RedirectToPage("/Vault/Folder", new { vaultId = vault.Id });
            return this.RedirectToPage("/Vault");
        }

        public async Task<IActionResult> OnPostSaveInfos(Common.Models.Vault vault)
        {
            if (vault.Id != null)
            {
                await this._vaultService.Update(vault);
            }

            return this.StatusCode((int)HttpStatusCode.OK, null);
        }
    }
}
