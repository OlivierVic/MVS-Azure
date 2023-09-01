// <copyright file="JsonHelper.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Common;
using MVS.Common.Enum;
using MVS.Common.Models;
using Newtonsoft.Json;
using Smartclause.SDK;
using Smartclause.SDK.DTO;
using SmartClause.SDK.DTO;

namespace MVS.Web.Helpers;

public static class RequestImmediateProtectionHelper
{
    public static async Task<string> GenerateRequestImmediateProtection(IConfiguration configuration, Client client, Vault folder, ApplicationUser user)
    {
        string refName = "RequestImmediateProtection";

        string templateId = configuration.GetValue<string>($"SCM:Templates:{refName}");
        string tenantId = configuration.GetValue<string>("SCM:TenantId");

        return await SCMHelper.GenerateContract(configuration, client, templateId ,tenantId, $"Requete Protection Immediate - {folder.FirstName} {folder.LastName}",user);
    }
}
