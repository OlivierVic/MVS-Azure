// <copyright file="VolonteSituationPersonnelle.cs" company="DroitsQuotidiens.Legal.Tech">
// Copyright (c) DroitsQuotidiens.Legal.Tech All rights reserved.
// </copyright>

using MVS.Common.Enum;
using MVS.Common.Models;
using MVS.Common;
using SmartClause.SDK.DTO;
using Smartclause.SDK;
using Humanizer;
using static Humanizer.On;
using Aspose.Words;
using NetTopologySuite.Mathematics;

namespace MVS.Web.Helpers.Volontes;

public static class VolontesSituationFamilialeHelper
{
    public static async Task<string> GenerateVolontesSituationFamily(IConfiguration configuration, Client client, Vault folder, ApplicationUser user, VaultFamilyInfo familyInfo)
    {
        string refName = "VolontesSituationFamiliale";

        string templateId = configuration.GetValue<string>($"SCM:Templates:{refName}");
        string tenantId = configuration.GetValue<string>("SCM:TenantId");

        ReferenceDTO reference = await SCMHelper.CheckTemplateReference(client, refName, tenantId, templateId);

        ReferenceDTO refElement = (await client.SearchReferenceElements(reference.Id, new Dictionary<string, string>() { { "VaultId", folder.Id } }, tenantId)).FirstOrDefault();
        if (refElement == null)
        {
            await CreateVolontesSituationFamilialeElem(client, folder, familyInfo, reference.Id);

            refElement = (await client.SearchReferenceElements(reference.Id, new Dictionary<string, string>() { { "VaultId", folder.Id } }, tenantId)).FirstOrDefault();
        }

        return await SCMHelper.GenerateContractWithReference(configuration, client, refElement.Id, templateId, $"Volonté Situation Familiale - {folder.FirstName} {folder.LastName}", user, tenantId);
    }

    public static async Task<ReferenceElementDTO> CreateVolontesSituationFamilialeElem(Client client, Vault folder, VaultFamilyInfo familyInfo, string refId)
    {
        int tmpFamilleRecom = 0;
        int tmpQualit = 0;
        int tmpVide = 1;
        string tmpBirthDate = folder.BirthDate.ToString();

        if (familyInfo.MatrimonialSituation != null || familyInfo.NbChildren != null || familyInfo.FamilialSituationDetails != null || familyInfo.FamilyRelationships != null
            || familyInfo.FamilyRelationshipsDetails != null)
        {
            tmpVide = 0;
        }

        if (familyInfo.FamilyRelationships != null || familyInfo.FamilyRelationships != "NULL" || familyInfo.FamilyRelationshipsDetails != null)
        {
            tmpQualit = 1;
        }

        if (familyInfo.FamilialSituationDetails != null)
        {
            tmpFamilleRecom = 1;
        }


        // Create Vault's ReferenceElem in MissionLetterReference
        Dictionary<string, string> newElem = new()
        {
            { "VaultId", folder.Id },
            { "Nom", folder.LastName },
            { "Prenom", folder.FirstName },
            { "DateNaissance", tmpBirthDate },
            { "LieuNaissance", folder.BirthLocation },
            { "Adresse", folder.Address },
            { "Nationalite", folder.Nationality },

            { "​RegimeMat", familyInfo.MatrimonialSituation },

            { "​NbEnf", familyInfo.NbChildren.ToString() },
            { "​PrecisionFamilleRecomp", familyInfo.FamilialSituationDetails },

            { "​QualiteRelFamil", familyInfo.FamilyRelationships },
            { "​PrecisionQualiteRelFamil", familyInfo.FamilyRelationshipsDetails },

            { "​tmpGenre", folder.Sex.ToString() },
            { "​tmpCouple", familyInfo.CoupleSituation },
            { "​tmpVie", familyInfo.FamilialSituation },
            { "​tmpEnf", familyInfo.Children.ToString() },
            { "​tmpFamilleRecom", tmpFamilleRecom.ToString() },
            { "​tmpQualit", tmpQualit.ToString() },
            { "​tmpVide", tmpVide.ToString() }
        };

        ReferenceElementDTO referenceElement = new() { ReferenceId = refId, Data = JsonHelper.GetJsonString(newElem) };
        return await client.CreateReferenceElement(referenceElement);
    }
}
