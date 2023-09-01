using MVS.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MVS.Web.Pages.Home
{
    public class PasswordResetModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        [BindProperty]
        public string _email { get; set; }

        [BindProperty]
        public string _token { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Veuillez renseigner votre mot de passe")]
        [DataType(DataType.Password)]
        public string _password { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Veuillez renseigner la confirmation de mot de passe")]
        [Compare("_password", ErrorMessage = "Les mots de passe ne sont pas identiques")]
        [DataType(DataType.Password)]
        public string _confirmPassword { get; set; }

        public bool _passwordReset { get; set; }

        public bool _passwordError { get; set; }

        public PasswordResetModel(UserManager<ApplicationUser> userManager) => _userManager = userManager;

        public void OnGet(string email, string resetPasswordToken)
        {
            this._email = email;
            this._token = resetPasswordToken;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            this._passwordError = false;

            if (this.ModelState.IsValid)
            {
                ApplicationUser user = await this._userManager.FindByEmailAsync(this._email);
                if (user != null)
                {
                    IdentityResult result = await this._userManager.ResetPasswordAsync(user, _token, _password);

                    if (result.Succeeded)
                    {
                        this._passwordReset = true;
                        return this.Page();
                    }
                }

                this._passwordError = true;
            }

            return this.Page();
        }
    }
}
