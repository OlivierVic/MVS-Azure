using MVS.Common;
using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Models;
using MVS.Common.Specifications;
using MVS.EmailSender.Sender;
using MVS.EmailSender.Templates.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MVS.Web.Pages.ContactByEmail
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IVaultService _folderService;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;

        public Dictionary<string, string> _breadcrumb { get; set; }
        public int _folderPaymentNumber { get; set; }
        public int _folderAskDeleteNumber { get; set; }


        public IndexModel(UserManager<ApplicationUser> userManager, IEmailSender emailSender, IConfiguration configuration, IVaultService folderService)
        {
            this._userManager = userManager;
            this._emailSender = emailSender;
            this._configuration = configuration;
            this._folderService = folderService;
        }

        public async Task OnGetAsync()
        {

            List<Common.Models.Vault> folderAskDelete = await this._folderService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
            this._folderAskDeleteNumber = folderAskDelete.Count;

            this._breadcrumb = new Dictionary<string, string>
            {
                { "/Home", "Accueil" },
                { "/ContactByEmail", "VaultContact" }
            };
        }
        public async Task OnPostSendEmail(string subject, string folderNumber, string question)
        {
            string dqContactEmail = this._configuration.GetValue<string>("DQContactEmail");
            ApplicationUser user = await this._userManager.GetUserAsync(this.User);
            string plateformUrl = this.Url.PageLink("Index", "Home", null, this.Request.Scheme);

            await this._emailSender.SendEmailAsync(dqContactEmail, new ContactByEmailTemplate
            {
                Subject = $"Message {user.DisplayName} Plateforme Alix Accompagne",
                Model = new ContactByEmailTemplateModel
                {
                    Name = user.DisplayName,
                    Email = user.Email,
                    Subject = subject,
                    FolderNumber = folderNumber,
                    Question = question,
                    PlateformUrl = plateformUrl
                }
            });
        }
    }
}
