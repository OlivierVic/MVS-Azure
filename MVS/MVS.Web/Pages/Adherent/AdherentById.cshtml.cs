using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVS.Common.Interfaces;
using MVS.Common.Models;
using MVS.Common.Specifications;
using MVS.Web.Controllers;
using MVS.Web.Models;
using Newtonsoft.Json;
using Smartclause.SDK;
using System.Security.Claims;
using static NuGet.Packaging.PackagingConstants;

namespace MVS.Web.Pages.Adherent
{
    public class AdherentByIdModel : PageModel
    {
        /// /////////////
        Uri baseAdress = new Uri("https://portail.am2is.com/api");
        private readonly HttpClient _client;
        ///////////
        private string _userId => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        public Models.AdherentViewModel _test { get; set; }

        public AdherentByIdModel(IVaultService vaultService)
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAdress;
        }

        public async Task OnGetAsync(string id)
        {
            //https://localhost:7199/Adherent/AdherentById?folderId=f7c25194-d9d4-40b1-90bf-207765728177&id=000000001
            this._test = await AdherentById("id=" + id);
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
