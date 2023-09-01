using MVS.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MVS.Web.Pages.Home;

public class LoginModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    [BindProperty]
    [Required(ErrorMessage = "Veuillez renseigner votre email")]
    [EmailAddress(ErrorMessage = "L'email n'est pas valide")]
    public string _email { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Veuillez renseigner votre mot de passe")]
    [DataType(DataType.Password)]
    public string _password { get; set; }

    [BindProperty]
    public bool _rememberMe { get; set; }

    [BindProperty]
    public bool _errorConnect { get; set; }

    public LoginModel(SignInManager<ApplicationUser> signInManager) => this._signInManager = signInManager;

    public IActionResult OnGet() => this.User.Identity.IsAuthenticated ? new RedirectToPageResult("/Account/Index") : this.Page();

    public async Task<IActionResult> OnPostAsync()
    {
        this._errorConnect = false;

        if (this.ModelState.IsValid)
        {
            Microsoft.AspNetCore.Identity.SignInResult result = await this._signInManager.PasswordSignInAsync(this._email, this._password, this._rememberMe, false);
            if (result.Succeeded)
            {
                return this.RedirectToPage("/Home/Index");
            }

            this._errorConnect = true;
            return this.Page();
        }

        return this.Page();
    }
}
