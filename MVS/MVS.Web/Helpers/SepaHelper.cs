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

public static class SepaHelper
{
    public static async Task<string> GenerateSEPA(IConfiguration configuration, Client client, Vault folder, VaultContact contact, string IBAN, string BIC, string City, ApplicationUser user)
    {
        string refName = "SEPA";
        string templateId = configuration.GetValue<string>($"SCM:Templates:{refName}");
        string tenantId = configuration.GetValue<string>("SCM:TenantId");

        ReferenceDTO reference = await SCMHelper.CheckTemplateReference(client, refName, tenantId, templateId);

        ReferenceDTO refElement = (await client.SearchReferenceElements(reference.Id, new Dictionary<string, string>() { { "VaultId", folder.Id } }, tenantId)).FirstOrDefault();
        if (refElement == null)
        {
            await CreateSEPAReferenceImmediateElem(client, folder.Id, contact, IBAN, BIC, City, reference.Id);


            refElement = (await client.SearchReferenceElements(reference.Id, new Dictionary<string, string>() { { "VaultId", folder.Id } }, tenantId)).FirstOrDefault();
        }

        return await SCMHelper.GenerateContractWithReference(configuration, client, refElement.Id, templateId, $"Manda SEPA - {folder.FirstName} {folder.LastName}", user, tenantId);
    }

    public static async Task CreateSEPAReferenceFutureElem(Client client, Vault folder, string IBAN, string BIC, string City, string refId)
    {
        // Create Vault's ReferenceElem in MissionLetterReference
        Dictionary<string, string> newElem = new()
        {
            { "VaultId", folder.Id },
            { "​NomLettreMission", folder.LastName },
            { "PrénomLettreMission", folder.FirstName },
            { "Adresse", $"{folder.Address}, {folder.ZipceCodeAndCity}, {folder.Country}" },
            { "IBAN", IBAN },
            { "BIC", BIC },
            { "Lieu", City },
            { "DateJour", DateTime.Now.ToString("dd/MM/yyyy") }
        };
        ReferenceElementDTO referenceElement = new() { ReferenceId = refId, Data = JsonHelper.GetJsonString(newElem) };
        await client.CreateReferenceElement(referenceElement);
    }

    public static async Task CreateSEPAReferenceImmediateElem(Client client, string folderId, VaultContact contact, string IBAN, string BIC, string City, string refId)
    {
        // Create Vault's ReferenceElem in MissionLetterReference
        Dictionary<string, string> newElem = new()
        {
            { "VaultId", folderId },
            { "​NomLettreMission", contact.LastName },
            { "PrénomLettreMission", contact.FirstName },
            { "Adresse", $"{contact.Addres}, {contact.ZipCodeAndCity}, {contact.Country}" },
            { "IBAN", IBAN },
            { "BIC", BIC },
            { "Lieu", City },
            { "DateJour", DateTime.Now.ToString("dd/MM/yyyy") }
        };
        ReferenceElementDTO referenceElement = new() { ReferenceId = refId, Data = JsonHelper.GetJsonString(newElem) };
        await client.CreateReferenceElement(referenceElement);
    }
}
