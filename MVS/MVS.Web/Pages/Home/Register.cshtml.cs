using MVS.Common;
using MVS.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MVS.Web.Pages.Home
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        [Required(ErrorMessage = "Veuillez renseigner votre email")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide")]
        public string _email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Veuillez renseigner votre mot de passe")]
        [DataType(DataType.Password)]
        public string _password { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Veuillez renseigner votre confirmation de mot de passe")]
        [DataType(DataType.Password)]
        [Compare("_password", ErrorMessage = "Les mots de passe ne sont pas identiques")]
        public string _passwordConfirmation { get; set; }

        [BindProperty]
        public bool _acceptCGU { get; set; }

        public bool _errorCGU { get; set; }
        public bool _errorEmail { get; set; }
        public bool _errorPassword { get; set; }

        public RegisterModel(UserManager<ApplicationUser> userManager) => this._userManager = userManager;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (this.ModelState.IsValid)
            {
                ApplicationUser user = await this._userManager.FindByEmailAsync(this._email);
                if (user != null)
                {
                    this._errorEmail = true;
                    return this.Page();
                }

                PasswordValidator<ApplicationUser> passwordValidator = new();
                IdentityResult result = await passwordValidator.ValidateAsync(this._userManager, null, this._password);
                if (!result.Succeeded)
                {
                    this._errorPassword = true;
                    return this.Page();
                }

                if (!this._acceptCGU)
                {
                    this._errorCGU = true;
                    return this.Page();
                }

                NewUserModel model = new()
                {
                    Email = this._email,
                    Password = this._password,
                };
                return this.RedirectToPage("/Home/Register2", model);
            }

            return this.Page();
        }
    }
}
