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

public static class BillHelper
{
    public static async Task<string> GenerateBill(IConfiguration configuration, Client client, Vault folder, VaultContact contact, ApplicationUser user, int billNumber)
    {
        string templateId = configuration.GetValue<string>($"SCM:Templates:{/*refName*/client}");
        string tenantId = configuration.GetValue<string>("SCM:TenantId");

        ReferenceDTO reference = await SCMHelper.CheckTemplateReference(client, "Facture", tenantId, templateId);

        ReferenceDTO refElement = (await client.SearchReferenceElements(reference.Id, new Dictionary<string, string>() { { "VaultId", folder.Id } }, tenantId)).FirstOrDefault();
        /*if (refElement == null)
        {
            if (folder.Field == (int)FolderField.FutureProtection)
            {
                await CreateBillReferenceFutureElem(client, folder, billNumber, reference.Id);
            }
            else
            {
                await CreateBillReferenceImmediateElem(client, folder, contact, billNumber, reference.Id);
            }

            refElement = (await client.SearchReferenceElements(reference.Id, new Dictionary<string, string>() { { "VaultId", folder.Id } }, tenantId)).FirstOrDefault();
        }*/

        return await SCMHelper.GenerateContractWithReference(configuration, client, refElement.Id, templateId, $"Facture - {folder.FirstName} {folder.LastName}", user, tenantId);
    }

    public static async Task CreateBillReferenceFutureElem(Client client, Vault folder, int billNumber, string refId)
    {
        // Create Vault's ReferenceElem in MissionLetterReference
        Dictionary<string, string> newElem = new()
        {
            { "VaultId", folder.Id },
            { "Nom", folder.LastName },
            { "Prénom", folder.FirstName },
            { "Adresse", $"{folder.Address}, {folder.ZipceCodeAndCity}, {folder.Country}" },
            { "​NuméroDossier", folder.Title },
            { "DateJour", DateTime.Now.ToString("dd/MM/yyyy") },
            /*{ "_PrenomNomImmediate", "" },
            { "_​AdresseImmediate", "" },
            { "_NomPrenomBeneficiaire", "" },*/
            { "​NuméroAnnéeMoisIncrémenté", billNumber.ToString() }
        };
        ReferenceElementDTO referenceElement = new() { ReferenceId = refId, Data = JsonHelper.GetJsonString(newElem) };
        await client.CreateReferenceElement(referenceElement);
    }

    public static async Task CreateBillReferenceImmediateElem(Client client, Vault folder, VaultContact contact, int billNumber, string refId)
    {
        string _PrenomNomImmediate = "";
        string _​AdresseImmediate = "";
        string _NomPrenomBeneficiaire = "";

        _PrenomNomImmediate = $"{contact.FirstName} {contact.LastName}";
        _​AdresseImmediate = $"{contact.Addres}, {contact.ZipCodeAndCity}, {contact.Country}";
        _NomPrenomBeneficiaire = $"{folder.FirstName} {folder.LastName}";

        // Create Vault's ReferenceElem in MissionLetterReference
        Dictionary<string, string> newElemImmediate = new()
        {
            { "VaultId", folder.Id },
            { "Nom", "" },
            { "Prénom", "" },
            { "Adresse", "" },
            { "​NuméroDossier", folder.Title },
            { "DateJour", DateTime.Now.ToString("dd/MM/yyyy") },
            { "_PrenomNomImmediate", _PrenomNomImmediate },
            { "_​AdresseImmediate", _​AdresseImmediate },
            { "_NomPrenomBeneficiaire", _NomPrenomBeneficiaire },
            { "​NuméroAnnéeMoisIncrémenté", billNumber.ToString() }
        };

        ReferenceElementDTO referenceElement = new() { ReferenceId = refId, Data = JsonHelper.GetJsonString(newElemImmediate) };
        await client.CreateReferenceElement(referenceElement);
    }
}
