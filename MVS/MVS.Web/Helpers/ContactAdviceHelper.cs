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

public static class ContactAdviceHelper
{
    public static async Task<string> GenerateContractAdvice(IConfiguration configuration, Client client, Vault folder, ApplicationUser user)
    {
        string refName = "ContactAdvice";
        string templateId = configuration.GetValue<string>($"SCM:Templates:{refName}");
        string tenantId = configuration.GetValue<string>("SCM:TenantId");

        ReferenceDTO reference = await SCMHelper.CheckTemplateReference(client, "Facture", tenantId, templateId);

        ReferenceDTO refElement = (await client.SearchReferenceElements(reference.Id, new Dictionary<string, string>() { { "VaultId", folder.Id } }, tenantId)).FirstOrDefault();
        if (refElement == null)
        {
            await CreateContactAdviceElem(client, folder, reference.Id);

            refElement = (await client.SearchReferenceElements(reference.Id, new Dictionary<string, string>() { { "VaultId", folder.Id } }, tenantId)).FirstOrDefault();
        }

        return await SCMHelper.GenerateContractWithReference(configuration, client, refElement.Id, templateId, $"Avis - {folder.FirstName} {folder.LastName}", user, tenantId);
    }

    public static async Task CreateContactAdviceElem(Client client, Vault folder, string refId)
    {
        // Create Vault's ReferenceElem in MissionLetterReference
        Dictionary<string, string> newElem = new()
        {
            { "VaultId", folder.Id },
            { "NomBénéficiaire", folder.LastName },
            { "​PrenomBénéficiaire", folder.FirstName },
            { "Adresse", $"{folder.Address}, {folder.ZipceCodeAndCity}, {folder.Country}" },
            { "​NuméroDossier", folder.Title },
            { "DateJour", DateTime.Now.ToString("dd/MM/yyyy") },
        };
        ReferenceElementDTO referenceElement = new() { ReferenceId = refId, Data = JsonHelper.GetJsonString(newElem) };
        await client.CreateReferenceElement(referenceElement);
    }
}
