using System.ComponentModel.DataAnnotations;

namespace MVS.Web.Models
{
    public class AdherentViewModel
    {
        public string last_name { get; set; }
        public string first_name { get; set; }
        public string dob { get; set; }  // a definir AAAA/MM/JJ
        public string mutac_id { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string mutac_status { get; set; }
        public string partner_id { get; set; }
        public string birth_location { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string zip_code { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string gender { get; set; }
        public string title { get; set; }
        public string citizenship { get; set; }
        public string marital_status { get; set; }
        public string NIR { get; set; }
    }
}
