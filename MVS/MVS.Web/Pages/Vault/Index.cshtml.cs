using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Specifications;
using MVS.Web.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Security.Claims;
using MVS.Common.Models;
using Newtonsoft.Json;
using MVS.Business;
using MVS.Web.Models;
using Smartclause.SDK;

namespace MVS.Web.Pages.Vault
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IVaultService _vaultService;
        private readonly IAspNetUserService _aspNetUserService;
        private readonly ICounterVaultCreateService _counterVaultCreateService;

        // Pour Call API
        Uri baseAdress = new Uri("https://portail.am2is.com/api");
        private readonly HttpClient _client;
        //

        public int _folderAskDeleteNumber { get; set; }
        public int _HaveVault { get; set; }
        public List<Common.Models.Vault> _vaults { get; set; }

        private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public CounterVaultCreate _nbVault { get; set; }
        public AdherentViewModel _adherentById { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public JsonResult test { get; set; }

        public IndexModel(IVaultService vaultService, ICounterVaultCreateService counterVaultCreateService, IAspNetUserService aspNetUserService)
        {
            this._vaultService = vaultService;
            this._counterVaultCreateService = counterVaultCreateService;
            this._aspNetUserService = aspNetUserService;

            //Pour Call Api
            this._client = new HttpClient();
            this._client.BaseAddress = this.baseAdress;
            //
        }

        public async Task<IActionResult> OnGetAsync()
        {
            List<Common.Models.Vault> folderAskDelete = await this._vaultService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = folderAskDelete.Count;

            AspNetUser infoUser = await this._aspNetUserService.Get(new Specification<Common.Models.AspNetUser>(u => u.Id == this._userId));

            List<Common.Models.Vault> HaveVault = await this._vaultService.Search(new Specification<Common.Models.Vault>(v => v.UserId == this._userId));
            this._HaveVault = HaveVault.Count;

            this._vaults = await this._vaultService.GetVaults(this._userId);

            if (this._HaveVault == 0)
            {
                await OnPostNewVault(infoUser);
            }

            return this.Page();
        }

        public async Task<JsonResult> OnGetVaults(int currentPage, int nbElem = 8, int sort = (int)FolderSort.Name)
        {
            if (this.User.IsInRole("SuperAdmin"))
            {
                Specification<Common.Models.Vault> spec = new(f => (bool)!f.IsArchived);

                //Compute nbPages
                int nbFolders = await this._vaultService.Count(spec);
                int nbPages = (nbFolders / nbElem) + (nbFolders % nbElem == 0 ? 0 : 1);

                //Paging
                int skip = (currentPage - 1) * nbElem;

                List<Common.Models.Vault> vaults = await this._vaultService.Search(spec);
                switch (sort)
                {
                    case (int)FolderSort.Name:
                        vaults = vaults.OrderBy(f => f.LastName + ' ' + f.FirstName).ToList();
                        break;
                    case (int)FolderSort.NameDesc:
                        vaults = vaults.OrderByDescending(f => f.LastName + ' ' + f.FirstName).ToList();
                        break;
                    case (int)FolderSort.CreationDate:
                        vaults = vaults.OrderByDescending(f => f.CreationDate).ToList();
                        break;
                }

                vaults = vaults.Skip(skip).Take(nbElem).ToList();

                return new JsonResult(new
                {
                    vaults,
                    nbPages
                });
            }
            else
            {
                Specification<Common.Models.Vault> spec = new(f => (f.UserId == this._userId || f.VaultUsers.Any(fu => fu.UserId == this._userId)) && f.IsArchived == false && !(f.IsDeleteAdmin ?? false));
                spec.Includes.Add(f => f.VaultUsers);

                //Compute nbPages
                int nbVaults = await this._vaultService.Count(spec);
                int nbPages = (nbVaults / nbElem) + (nbVaults % nbElem == 0 ? 0 : 1);

                //Paging
                int skip = (currentPage - 1) * nbElem;

                List<Common.Models.Vault> vaults = await this._vaultService.GetVaults(this._userId);
                switch (sort)
                {
                    case (int)FolderSort.Name:
                        vaults = vaults.OrderBy(f => f.LastName + ' ' + f.FirstName).ToList();
                        break;
                    case (int)FolderSort.NameDesc:
                        vaults = vaults.OrderByDescending(f => f.LastName + ' ' + f.FirstName).ToList();
                        break;
                    case (int)FolderSort.CreationDate:
                        vaults = vaults.OrderByDescending(f => f.CreationDate).ToList();
                        break;
                }

                vaults = vaults.Skip(skip).Take(nbElem).ToList();

                return new JsonResult(new
                {
                    vaults,
                    nbPages
                });
            }
        }

        public async Task OnPostUpdateVaultArchiveStatus(string vaultId, bool archive)
        {
            Common.Models.Vault vault = await this._vaultService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));
            vault.IsArchived = archive;

            await this._vaultService.Update(vault);
        }

        public async Task<IActionResult> OnPostDeleteVault(string vaultId)
        {
            Common.Models.Vault vault = await this._vaultService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));

            await this._vaultService.DeleteVault(vault.Id);


            return this.StatusCode((int)HttpStatusCode.OK, null);
        }

        public async Task<IActionResult> OnGetLaunchEval(string vaultId)
        {
            Common.Models.Vault vault = await this._vaultService.Get(new Specification<Common.Models.Vault>(f => f.Id == vaultId));
            await this._vaultService.Update(vault);

            return this.StatusCode((int)HttpStatusCode.OK, null);
        }

        public async Task<IActionResult> OnGetTestUserForIdMutac(AspNetUser userInfo, AdherentViewModel adherentById)
        {
            if (userInfo == null)
            {
                return this.StatusCode((int)HttpStatusCode.NotFound, null);
            }
            else if (adherentById == null)
            {
                return this.StatusCode((int)HttpStatusCode.NotFound, null);
            }

            if (userInfo.LastName != adherentById.last_name && userInfo.FirstName != adherentById.first_name)
            {
                return this.StatusCode((int)HttpStatusCode.NonAuthoritativeInformation, null);
            }

            return this.StatusCode((int)HttpStatusCode.OK, null);
        }

        public async Task<IActionResult> OnPostNewVault(AspNetUser infoUser)
        {
            ObjectResult statusCode = null;

            if (infoUser.MutacAdh == true)
            {
                this._adherentById = await AdherentById("id=" + infoUser.MutacNumber);
                statusCode = (ObjectResult)await this.OnGetTestUserForIdMutac(infoUser, this._adherentById);
            }

            //statusCode = (ObjectResult)await this.OnGetTestUserForIdMutac(infoUser, this._adherentById);
            if (infoUser.MutacAdh == true)
            {
                if (statusCode.StatusCode == 203)
                {
                    return this.StatusCode((int)HttpStatusCode.NonAuthoritativeInformation, null);
                }
                else if (statusCode.StatusCode == 200)
                {
                    string vaultName = "";
                    DateTime dateNow = DateTime.Now;
                    this._nbVault = await this._counterVaultCreateService.Get(new Specification<CounterVaultCreate>(f => true));
                    int tmpSex = 0;

                    if (this._adherentById.gender.ToUpper() == "MASCULIN")
                    {
                        tmpSex = 0;
                    }
                    else if (this._adherentById.gender.ToUpper() == "FÉMININ")
                    {
                        tmpSex = 1;
                    }
                    else if (this._adherentById.gender.ToUpper() == "HOMME")
                    {
                        tmpSex = 0;
                    }
                    else if (this._adherentById.gender.ToUpper() == "FEMME")
                    {
                        tmpSex = 1;
                    }
                    else if (this._adherentById.gender.ToUpper() == "INCONNU")
                    {
                        tmpSex = 2;
                    }

                    if (this._nbVault == null)
                    {
                        this._nbVault = await this._counterVaultCreateService.Add(new CounterVaultCreate() { NbVault = 0 });
                    }
                    int counterNbFolder = this._nbVault.NbVault;

                    counterNbFolder++;
                    this._nbVault.NbVault = counterNbFolder;
                    await this._counterVaultCreateService.Update(this._nbVault);

                    vaultName = dateNow.Year.ToString() + "C" + counterNbFolder;
                    Common.Models.Vault vault = new Common.Models.Vault
                    {
                        Id = Guid.NewGuid().ToString(),
                        Title = $"{vaultName}",
                        Email = infoUser.Email,
                        FirstName = infoUser.FirstName,
                        LastName = infoUser.LastName,
                        BirthDate = infoUser.BirthDate,
                        Address = this._adherentById.address1 + " " + this._adherentById.address2,
                        BirthLocation = this._adherentById.birth_location,
                        ZipceCodeAndCity = this._adherentById.zip_code + " " + this._adherentById.city,
                        PhoneNumber = this._adherentById.phone_number,
                        Country = this._adherentById.country,
                        Sex = tmpSex,

                        UserId = this._userId,
                        IsArchived = false,
                        CreationDate = DateTime.Now,
                    };

                    vault.VaultUsers = new List<VaultUser>()
                {
                    new VaultUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        VaultId = vault.Id,
                        UserId = this._userId,
                    }
                };

                    vault = await this._vaultService.Add(vault);

                    return this.StatusCode((int)HttpStatusCode.OK, null);
                }
                else if (statusCode.StatusCode == 404)
                {
                    return this.StatusCode((int)HttpStatusCode.NotFound, null);
                }
            }
            else
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
                Common.Models.Vault vault = new Common.Models.Vault
                {
                    Id = Guid.NewGuid().ToString(),
                    Title = $"{vaultName}",
                    Email = infoUser.Email,
                    FirstName = infoUser.FirstName,
                    LastName = infoUser.LastName,
                    BirthDate = infoUser.BirthDate,
                    UserId = this._userId,
                    IsArchived = false,
                    CreationDate = DateTime.Now,
                };

                vault.VaultUsers = new List<VaultUser>()
                {
                    new VaultUser()
                    {
                        Id = Guid.NewGuid().ToString(),
                        VaultId = vault.Id,
                        UserId = this._userId,
                    }
                };

                vault = await this._vaultService.Add(vault);

                return this.StatusCode((int)HttpStatusCode.OK, null);
            }

            return this.StatusCode((int)HttpStatusCode.Forbidden, null);

        }

        [HttpGet]
        public async Task<AdherentViewModel> AdherentById(string id)
        {
            // Il faut avoir dans l'url : 
            // https://localhost:44334/Adherent/AdherentById/id=000000001
            AdherentViewModel TheAdherent = new AdherentViewModel();
            HttpResponseMessage reponse = _client.GetAsync(_client.BaseAddress + "/MutuApi/id?" + id).Result;
            if (reponse.IsSuccessStatusCode)
            {
                string data = reponse.Content.ReadAsStringAsync().Result;
                TheAdherent = JsonConvert.DeserializeObject<AdherentViewModel>(data);
            }
            return TheAdherent;
        }
    }
}
