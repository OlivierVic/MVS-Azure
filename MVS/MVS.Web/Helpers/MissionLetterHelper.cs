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

public static class MissionLetterHelper
{
    public static async Task<string> GenerateMissionLetter(IConfiguration configuration, Client client, Vault folder, ApplicationUser user, VaultContact contact = null)
    {
        string refName = "MissionLetterFutureProtection";

        refName = "MissionLetterImmediateProtection";


        string templateId = configuration.GetValue<string>($"SCM:Templates:{refName}");
        string tenantId = configuration.GetValue<string>("SCM:TenantId");

        ReferenceDTO reference = await SCMHelper.CheckTemplateReference(client, refName, tenantId, templateId);

        ReferenceDTO refElement = (await client.SearchReferenceElements(reference.Id, new Dictionary<string, string>() { { "VaultId", folder.Id } }, tenantId)).FirstOrDefault();
        if (refElement == null)
        {
            if (refName == "MissionLetterFutureProtection")
            {
                await CreateFutureMissionLetterReferenceElem(client, folder, reference.Id);
            }
            else if (refName == "MissionLetterImmediateProtection")
            {
                await CreateImmediateMissionLetterReferenceElem(client, folder, contact, reference.Id);
            }
            else
            {
                await CreateImmediateMissionLetterWithoutRequesterReferenceElem(client, folder, reference.Id);
            }

            refElement = (await client.SearchReferenceElements(reference.Id, new Dictionary<string, string>() { { "VaultId", folder.Id } }, tenantId)).FirstOrDefault();
        }

        return await SCMHelper.GenerateContractWithReference(configuration, client, refElement.Id, templateId, $"Lettre de mission - {folder.FirstName} {folder.LastName}", user, tenantId);
    }

    public static async Task CreateFutureMissionLetterReferenceElem(Client client, Vault folder, string refId)
    {
        // Create Vault's ReferenceElem in MissionLetterReference
        Dictionary<string, string> newElem = new()
        {
            { "VaultId", folder.Id },
            { "Nom", folder.LastName },
            { "Prénom", folder.FirstName },
            { "DateNaissance", folder.BirthDate.ToString(/*"dd/MM/yyyy"*/) },
            { "LieuNaissance", folder.BirthLocation },
            { "Adresse", $"{folder.Address}, {folder.ZipceCodeAndCity}, {folder.Country}" },
            { "Mail", folder.Email },
            { "Téléphone", folder.PhoneNumber },
            { "DateJour", DateTime.Now.ToString("dd/MM/yyyy") }
        };
        ReferenceElementDTO referenceElement = new() { ReferenceId = refId, Data = JsonHelper.GetJsonString(newElem) };
        await client.CreateReferenceElement(referenceElement);
    }

    public static async Task CreateImmediateMissionLetterReferenceElem(Client client, Vault folder, VaultContact contact, string refId)
    {

        // Create Vault's ReferenceElem in MissionLetterReference for Immediate Protection
        Dictionary<string, string> newElem = new()
        {
            { "VaultId", folder.Id },
            { "​NomDemandeur", contact.LastName },
            { "​PrénomDemandeur", contact.FirstName },
            { "​DateNaissanceDemandeur", contact.DateOfBirth?.ToString("dd/MM/yyyy") },
            { "​LieuNaissanceDemandeur", contact.PlaceOfBirth },
            { "​AdresseDemandeur", $"{contact.Addres}, {contact.ZipCodeAndCity}, {contact.Country}" },
            { "​EmailDemandeur", contact.Email },
            { "​TéléphoneDemandeur", contact.PhoneNumber },
            { "​NomPersonneProtéger", folder.LastName },
            { "​PrénomPersonneProtéger", folder.FirstName },
            { "​DateNaissancePersonneProtéger", folder.BirthDate.ToString(/*"dd/MM/yyyy"*/) },
            { "​LieuNaissancePersonneProtéger", folder.BirthLocation },
            { "​AdressePersonneProtéger", $"{folder.Address}, {folder.ZipceCodeAndCity}, {folder.Country}" },
            { "​EmailPersonneProtéger", folder.Email },
            { "​TéléphonePersonneProtéger", folder.PhoneNumber },
            { "DateJour", DateTime.Now.ToString("dd/MM/yyyy") }
        };
        ReferenceElementDTO referenceElement = new() { ReferenceId = refId, Data = JsonHelper.GetJsonString(newElem) };
        await client.CreateReferenceElement(referenceElement);
    }

    public static async Task CreateImmediateMissionLetterWithoutRequesterReferenceElem(Client client, Vault folder, string refId)
    {
        // Create Vault's ReferenceElem in MissionLetterReference for Immediate Protection
        Dictionary<string, string> newElem = new()
        {
            { "VaultId", folder.Id },
            { "​Nom", folder.LastName },
            { "​Prénom", folder.FirstName },
            { "​DateNaissance", folder.BirthDate.ToString(/*"dd/MM/yyyy"*/) },
            { "​LieuNaissance", folder.BirthLocation },
            { "​Adresse", $"{folder.Address}, {folder.ZipceCodeAndCity}, {folder.Country}" },
            { "​Email", folder.Email },
            { "​Téléphone", folder.PhoneNumber },
            { "​DateJour", DateTime.Now.ToString("dd/MM/yyyy") }
        };
        ReferenceElementDTO referenceElement = new() { ReferenceId = refId, Data = JsonHelper.GetJsonString(newElem) };
        await client.CreateReferenceElement(referenceElement);
    }
}
