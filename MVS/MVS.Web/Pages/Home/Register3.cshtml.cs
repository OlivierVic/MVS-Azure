using MVS.Common;
using MVS.Web.Controllers;
using MVS.EmailSender.Sender;
using MVS.EmailSender.Templates.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MVS.Web.Models;
using MVS.Web.Pages.Adherent;

namespace MVS.Web.Pages.Home
{
    public class Register3Model : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public bool _emailValid { get; set; }
        public string _email { get; set; }

        public Register3Model(UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            this._userManager = userManager;
            this._emailSender = emailSender;
        }

        public void OnGet(bool emailValid, string email)
        {
            this._emailValid = emailValid;
            this._email = email;
        }
        public async Task<IActionResult> OnGetSendEmail(string email)
        {
            ApplicationUser user = await this._userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return this.RedirectToPage("Home/Login");
            }

            string token = await this._userManager.GenerateEmailConfirmationTokenAsync(user);
            string resetPasswordUrl = this.Url.PageLink("/Home/Register3", "ValidEmail", new { email, token }, this.Request.Scheme);
            string plateformUrl = this.Url.PageLink("Index", "Home", null, this.Request.Scheme);

            await this._emailSender.SendEmailAsync(email, new RegisterTemplate
            {
                Model = new RegisterTemplateModel
                {
                    Name = user.DisplayName,
                    ValidEmailUrl = resetPasswordUrl,
                    PlateformUrl = plateformUrl
                }
            });

            return this.RedirectToPage("/Home/Register3", new { emailValid = false, email });
        }

        public async Task<IActionResult> OnGetValidEmail(string email, string token)
        {
            ApplicationUser user = await this._userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return this.RedirectToPage("Home/Login");
            }

            await this._userManager.ConfirmEmailAsync(user, token);

            return this.RedirectToPage("/Home/Register3", new { emailValid = true, email });
        }
    }
}
