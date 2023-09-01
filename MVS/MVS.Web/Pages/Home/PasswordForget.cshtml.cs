using MVS.Common;
using MVS.EmailSender.Sender;
using MVS.EmailSender.Templates.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MVS.Web.Pages.Home
{
    public class PasswordForgetModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        [BindProperty]
        [Required(ErrorMessage = "Veuillez renseigner votre email")]
        [EmailAddress(ErrorMessage = "L'email n'est pas valide")]
        public string _email { get; set; }

        public bool _emailSend { get; set; }
        public bool _errorUser { get; set; }

        public PasswordForgetModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IConfiguration configuration)
        {
            this._userManager = userManager;
            this._emailSender = emailSender;
            this._configuration = configuration;
        }

        public IActionResult OnGet() => this.User.Identity.IsAuthenticated ? new RedirectToPageResult("/Home/Index") : this.Page();

        public async Task<IActionResult> OnPostAsync()
        {
            if (this.ModelState.IsValid)
            {
                ApplicationUser user = await this._userManager.FindByEmailAsync(this._email);

                if (user == null)
                {
                    this._errorUser = true;
                    return this.Page();
                }

                string token = await this._userManager.GeneratePasswordResetTokenAsync(user);

                //Todo SendEmail ResetPassword
                string resetPasswordUrl = this.Url.PageLink("PasswordReset", "Home", new { email = this._email, resetPasswordToken = token }, this.Request.Scheme);
                string plateformUrl = this.Url.PageLink("Index", "Home", null, this.Request.Scheme);

                await this._emailSender.SendEmailAsync(this._email, new ResetPasswordTemplate
                {
                    Model = new ResetPasswordTemplateModel
                    {
                        ResetPasswordUrl = resetPasswordUrl,
                        PlateformUrl = plateformUrl
                    }
                });

                this._emailSend = true;
            }
            return this.Page();
        }
    }
}
