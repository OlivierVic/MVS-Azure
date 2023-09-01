using MVS.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MVS.Web.Pages.Home;

public class LogoutModel : PageModel
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public LogoutModel(SignInManager<ApplicationUser> signInManager) => this._signInManager = signInManager;

    public async Task<IActionResult> OnGetAsync()
    {
        await this._signInManager.SignOutAsync();
        return new RedirectToPageResult("/Home/Index");
    }
}
