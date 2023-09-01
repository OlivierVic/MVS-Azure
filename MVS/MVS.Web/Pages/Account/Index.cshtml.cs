// <copyright file="Index.cshtml.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Business;
using MVS.Common;
using MVS.Common.Enum;
using MVS.Common.Interfaces;
using MVS.Common.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Reflection;
using System.Security.Claims;

namespace MVS.Web.Pages.Account;

[Authorize]
public class IndexModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IVaultService _folderService;

    public ApplicationUser _currentUser { get; set; }
    public Dictionary<string, string> _breadcrumb { get; set; }
    public int _folderPaymentNumber { get; set; }
    public int _folderAskDeleteNumber { get; set; }

    public IndexModel(UserManager<ApplicationUser> userManager, IVaultService folderService)
    {
        this._userManager = userManager;
        this._folderService = folderService;
    }

    public async Task OnGetAsync(string userId)
    {
        if (userId != null && !this.User.IsInRole("SuperAdmin"))
        {
            throw new UnauthorizedAccessException("Vous n'avez pas accès à la donnée que vous voulez récuprer");
        }
        else if (userId == null)
        {
            userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }


        List<Common.Models.Vault> folderAskDelete = await this._folderService.Search(new Specification<Common.Models.Vault>(f => f.IsDeleteAdmin == true));
        this._folderAskDeleteNumber = folderAskDelete.Count;

        this._currentUser = await this._userManager.FindByIdAsync(userId);

        this._breadcrumb = new Dictionary<string, string>();
        this._breadcrumb.Add("/Home", "Accueil");
        this._breadcrumb.Add("/Account", "Mon compte");
    }

    public async Task OnPostUpdateNotifSettings(string userId, bool notifAnswerQuestions, bool notifAppointment, bool notifFileProgress, bool notifOpinion)
    {
        this._currentUser = await this._userManager.FindByIdAsync(userId);

        await this._userManager.UpdateAsync(this._currentUser);
    }

    public async Task OnPostUpdateProperty(string userId, string value, string propertyName)
    {
        this._currentUser = await this._userManager.FindByIdAsync(userId);

        Type applicationUser = typeof(ApplicationUser);
        PropertyInfo myPropInfo = applicationUser.GetProperty(propertyName);
        myPropInfo.SetValue(this._currentUser, value);

        await this._userManager.UpdateAsync(this._currentUser);
    }

    public async Task OnPostUpdateIdentity(string userId, string firstname, string lastname)
    {
        this._currentUser = await this._userManager.FindByIdAsync(userId);

        this._currentUser.FirstName = firstname;
        this._currentUser.LastName = lastname;

        await this._userManager.UpdateAsync(this._currentUser);
    }

    public async Task<IActionResult> OnPostUpdateEmail(string userId, string email, string password)
    {
        this._currentUser = await this._userManager.FindByIdAsync(userId);

        if (await this._userManager.CheckPasswordAsync(this._currentUser, password))
        {
            await this._userManager.SetEmailAsync(this._currentUser, email);
            return this.StatusCode((int)HttpStatusCode.OK, null);
        }

        return this.StatusCode((int)HttpStatusCode.BadRequest, "password error");
    }

    public async Task<IActionResult> OnPostUpdatePassword(string userId, string oldPassword, string newPassword)
    {
        this._currentUser = await this._userManager.FindByIdAsync(userId);

        if (await this._userManager.CheckPasswordAsync(this._currentUser, oldPassword))
        {
            IdentityResult result = await this._userManager.ChangePasswordAsync(this._currentUser, oldPassword, newPassword);
            if (result.Succeeded)
            {
                return this.StatusCode((int)HttpStatusCode.OK, null);
            }
            else
            {
                return this.StatusCode((int)HttpStatusCode.BadRequest, "newPassword");
            }
        }

        return this.StatusCode((int)HttpStatusCode.BadRequest, "oldPassword");
    }
}