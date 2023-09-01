using MVS.Common;
using MVS.Common.Interfaces;
using MVS.Common.Models;
using MVS.Common.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MVS.Web.Pages.Home
{
    public class PasswordResetNewUserAdminModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAspNetUserService _userService;

        [BindProperty]
        public string email { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public AspNetUser _user { get; set; }

        [BindProperty]
        public string token { get; set; }
        [BindProperty]
        public string ConfirmEmailtoken { get; set; }
        public string PassWordhash { get; set; }

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
        public bool _errorCGU { get; set; }

        [BindProperty]
        public bool _acceptCGU { get; set; }

        public PasswordResetNewUserAdminModel(UserManager<ApplicationUser> userManager, IAspNetUserService userService)
        {
            this._userManager = userManager;
            this._userService = userService;
        }

        public async Task OnGetAsync(string email, string firstName, string lastName)
        {
            this.email = email;
            this.firstName = firstName;
            this.lastName = lastName;

            this._user = await this._userService.Get(new Specification<AspNetUser>(u => u.Email == this.email && u.FirstName == this.firstName && u.LastName == this.lastName));

            this.PassWordhash = _user.PasswordHash;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            this._passwordError = false;
            this._errorCGU = false;

            ApplicationUser user = await this._userManager.FindByEmailAsync(this.email);
            token = await this._userManager.GeneratePasswordResetTokenAsync(user);
            ConfirmEmailtoken = await this._userManager.GenerateEmailConfirmationTokenAsync(user);
            this.email = user.Email;
            this.firstName = user.FirstName;
            this.lastName = user.LastName;

            if (this.ModelState.IsValid)
            {
                if (!this._acceptCGU)
                {
                    this._errorCGU = true;
                    return this.Page();
                }
                else if (user != null)
                {
                    if (this._acceptCGU)
                    {
                        await this._userManager.ConfirmEmailAsync(user, ConfirmEmailtoken);
                    }

                    IdentityResult result = await this._userManager.ResetPasswordAsync(user, token, _password);

                    if (result.Succeeded)
                    {
                        this._passwordReset = true;
                        return this.RedirectToPage("/Home/PasswordResetNewUserAdmin", new { user.Email, user.FirstName, user.LastName });
                    }
                }

                this._passwordError = true;
            }

            return this.Page();
        }
    }
}
