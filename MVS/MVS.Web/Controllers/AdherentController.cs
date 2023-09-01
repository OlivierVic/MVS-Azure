using MVS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MVS.Web.Controllers
{
    public class AdherentController : Controller
    {
        // https://portail.am2is.com/api/MutuApi/id?id=000000002
        Uri baseAdress = new Uri("https://portail.am2is.com/api");
        private readonly HttpClient _client;

        public AdherentController()
        {
            _client = new HttpClient();
            _client.BaseAddress = baseAdress;
        }

        [HttpGet]
        public IActionResult Adherent(List<AdherentViewModel> jsonConverter)
        {
            List<AdherentViewModel> listeAdherent = new List<AdherentViewModel>();
            HttpResponseMessage reponse = _client.GetAsync(_client.BaseAddress + "/MutuApi").Result;
            if(reponse.IsSuccessStatusCode)
            {
                string data = reponse.Content.ReadAsStringAsync().Result;
                listeAdherent = JsonConvert.DeserializeObject<List<AdherentViewModel>>(data);
            }
            return View(listeAdherent);
        }

        [HttpGet]
        public IActionResult AdherentById(string id)
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
            return View(TheAdherent);
        }

    }
}
