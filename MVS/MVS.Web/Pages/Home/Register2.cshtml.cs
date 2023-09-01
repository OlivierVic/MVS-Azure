using MVS.Common;
using MVS.Common.Enum;
using MVS.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MVS.Web.Pages.Home
{
    public class Register2Model : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        [Required(ErrorMessage = "Vous devez renseigner votre nom")]
        public string _lastName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Veuillez renseigner votre prénom")]
        public string _firstName { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Veuillez renseigner votre date de naissance")]
        public DateTime _birthdate { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Veuillez renseigner si vous êtes adhérent mutac")]
        public int? _adhemutac { get; set; }

        [BindProperty]
        public string _numMutac { get; set; }

        [BindProperty]
        public int? _case { get; set; }

        [BindProperty]
        public string _company { get; set; }

        [BindProperty]
        public string _beneficiaryLink { get; set; }

        [BindProperty]
        public string _email { get; set; }

        [BindProperty]
        public string _password { get; set; }

        public bool _errorCase { get; set; }
        public bool _errorCompany { get; set; }
        public bool _errorNumMutac { get; set; }
        public bool _errorLink { get; set; }

        public bool tmpadhmutac { get; set; }



        public void OnGet(string email, string password) {
            this._email = email;
            this._password = password;
        }

        public Register2Model(UserManager<ApplicationUser> userManager) => this._userManager = userManager;

        public async Task<IActionResult> OnPostAsync()
        {
            if(this._adhemutac == (int)RegisterMutac.Oui)
            {
                this.tmpadhmutac = true;
            }
            if (this.ModelState.IsValid)
            {
                if (this._adhemutac == (int)RegisterMutac.Oui && this._numMutac == null)
                {
                    this._errorNumMutac = true;
                    return this.Page();
                }

                /*if (this._case == null)
                {
                    this._errorCompany = true;
                    return this.Page();
                }*/

                /*if (this._case == (int)RegisterCase.CaregiverPro && this._company == null)
                {
                    this._errorCompany = true;
                    return this.Page();
                }

                if (this._case == (int)RegisterCase.caregiverParticular && this._beneficiaryLink == null)
                {
                    this._errorLink = true;
                    return this.Page();
                }*/

                ApplicationUser newUser = new()
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = this._email,
                    Email = this._email,
                    FirstName = this._firstName,
                    LastName = this._lastName,
                    PhoneNumberConfirmed = false,
                    BirthDate = this._birthdate,
                    MutacAdh = this.tmpadhmutac,
                    MutacNumber = this._numMutac,
                    //Company = this._company,
                };
                await this._userManager.CreateAsync(newUser, this._password);

                /*if (this._case == (int)RegisterCase.CaregiverPro)
                {
                    await this._userManager.AddToRoleAsync(newUser, "Pro");
                }*/

                return this.RedirectToPage("/Home/Register3", "SendEmail", new { email = this._email });
            }

            return this.Page();
        }
    }
}
