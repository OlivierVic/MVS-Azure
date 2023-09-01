// <copyright file="MotionForProtectiveChange.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Common;
using MVS.Common.Enum;
using MVS.Common.Models;
using Smartclause.SDK;
using SmartClause.SDK.DTO;

namespace MVS.Web.Helpers;

public class MotionForProtectiveChangeHelper
{
    public static async Task<string> GenerateMotionForProtectiveChange(IConfiguration configuration, Client client, ApplicationUser user, Vault folder, VaultFamilyInfo folderFamilyInfo, VaultPersonalInfo folderPersonalInfo, string nameContactProtector, VaultContact contact = null)
    {
        string refName = "Requête changement protecteure";

        string templateId = configuration.GetValue<string>($"SCM:Templates:{refName}");
        string tenantId = configuration.GetValue<string>("SCM:TenantId");

        ReferenceDTO reference = await SCMHelper.CheckTemplateReference(client, refName, tenantId, templateId);

        ReferenceDTO refElement = (await client.SearchReferenceElements(reference.Id, new Dictionary<string, string>() { { "VaultId", folder.Id } }, tenantId)).FirstOrDefault();

        if (refElement == null)
        {
            await CreateMotionForProtectiveChangeReferenceElem(client, folder, contact, folderFamilyInfo, folderPersonalInfo, nameContactProtector, reference.Id);

            refElement = (await client.SearchReferenceElements(reference.Id, new Dictionary<string, string>() { { "VaultId", folder.Id } }, tenantId)).FirstOrDefault();
        }

        return await SCMHelper.GenerateContractWithReference(configuration, client, refElement.Id, templateId, $"Requête changement protecteure - {folder.FirstName} {folder.LastName}", user, tenantId);
    }

    public static async Task CreateMotionForProtectiveChangeReferenceElem(Client client, Vault folder, VaultContact contact, VaultFamilyInfo folderFamilyInfo, VaultPersonalInfo folderPersonalInfo, string nameContactProtector, string refId)
    {
        string dateNow = DateTime.Now.ToString("dd MMMM yyyy");

        Dictionary<string, string> newElem = new()
        {
            { "VaultId", folder.Id },
            { "​NomPrénomDemandeur", $"{contact.LastName} {contact.FirstName}" },
            { "​Adresse", contact.Addres },
            { "​NomPrénomPersonneProtégée", $"{folder.LastName} {folder.FirstName}" },
            { "​dateNaissancePersonneProtégée", folder.BirthDate.ToString(/*"dd/MM/yyyy"*/) },
            { "​LieuNaissancePersonneProtégée", folder.BirthLocation },
            { "​AdressePersonneProtégée", $"{folder.Address}, {folder.ZipceCodeAndCity}, {folder.Country}" },
            { "​MesureDeProtection", "A préciser" },
            { "​NomEtPrénomsDesProtecteurs", nameContactProtector },
            { "​LaPersonneEtLesBiens", "" },
            { "​LieuDeVie", folderPersonalInfo.LivingEnvironment },
            { "​ContexteActuel", "" },
            { "​SituationFamiliale", folderFamilyInfo.FamilialSituation },
            { "​CausePrincipaleDeLaDemande", "A préciser" },
            { "​FaitLe", dateNow }
        };

        if (contact.Other == null)
        {
            newElem["Qualité"] = contact.OtherFamilyTies;
        }
        else
        {
            newElem["Qualité"] = contact.Other;
        }

        if(folder.Sex == (int)Gender.Man)
        {
            newElem["GenreProtégée"] = "M";
        }
        else if(folder.Sex == (int)Gender.Woman)
        {
            newElem["GenreProtégée"] = "Mme";
        }
        else
        {
            newElem["GenreProtégée"] = "";
        }

        ReferenceElementDTO referenceElement = new() { ReferenceId = refId, Data = JsonHelper.GetJsonString(newElem) };
        await client.CreateReferenceElement(referenceElement);

    }
}
