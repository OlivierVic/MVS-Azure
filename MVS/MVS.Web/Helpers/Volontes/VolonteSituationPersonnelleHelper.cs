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

public static class VolonteSituationPersonnelleHelper
{
    public static async Task<string> GenerateVolonteSituationPersonnelle(IConfiguration configuration, Client client, Vault folder, ApplicationUser user, VaultPersonalInfo personalInfo)
    {
        string refName = "VolonteSituationPersonnelle";

        string templateId = configuration.GetValue<string>($"SCM:Templates:{refName}");
        string tenantId = configuration.GetValue<string>("SCM:TenantId");

        ReferenceDTO reference = await SCMHelper.CheckTemplateReference(client, refName, tenantId, templateId);

        ReferenceDTO refElement = (await client.SearchReferenceElements(reference.Id, new Dictionary<string, string>() { { "VaultId", folder.Id } }, tenantId)).FirstOrDefault();
        if (refElement == null)
        {
            await CreateVolonteSituationPersonnelleElem(client, folder, personalInfo, reference.Id);

            refElement = (await client.SearchReferenceElements(reference.Id, new Dictionary<string, string>() { { "VaultId", folder.Id } }, tenantId)).FirstOrDefault();
        }

        return await SCMHelper.GenerateContractWithReference(configuration, client, refElement.Id, templateId, $"Volonté Situation Personnelle - {folder.FirstName} {folder.LastName}", user, tenantId);
    }

    public static async Task<ReferenceElementDTO> CreateVolonteSituationPersonnelleElem(Client client, Vault folder, VaultPersonalInfo personalInfo, string refId)
    {
        int tmpInfogenerale = 0;
        int tmpLogement = 0;
        int tmpVide = 1;
        string tmpMaternelLanguage;
        string tmpBirthDate = folder.BirthDate.ToString();

        if (personalInfo.FrenchMotherTongue != null)
        {
            tmpInfogenerale = 1;
        }
        else if (personalInfo.ProfessionalSituation != null)
        {
            tmpInfogenerale = 1;
        }
        else if (personalInfo.LevelOfStudy != null)
        {
            tmpInfogenerale = 1;
        }
        else if (personalInfo.LevelOfStudyDetails != null)
        {
            tmpInfogenerale = 1;
        }

        if (personalInfo.LivingEnvironment != null)
        {
            tmpLogement = 1;
        }
        else if (personalInfo.TypeOfHousing != null)
        {
            tmpLogement = 1;
        }
        else if (personalInfo.HousingLaw != null)
        {
            tmpLogement = 1;
        }
        else if (personalInfo.PrecisionHousing != null)
        {
            tmpLogement = 1;
        }

        if (personalInfo.FrenchMotherTongue != null || personalInfo.OtherLanguage != null || personalInfo.UnderstandFrench != null || personalInfo.SpeakFrench != null || personalInfo.ProfessionalSituation != null || personalInfo.LevelOfStudy != null
            || personalInfo.LevelOfStudyDetails != null || personalInfo.Job != null || personalInfo.JobDetails != null || personalInfo.Cv != null || personalInfo.ProtectiveSupervision != null || personalInfo.LivingEnvironment != null
            || personalInfo.TypeOfHousing != null || personalInfo.HousingLaw != null || personalInfo.CoOwnership != null || personalInfo.OngoingLitigation != null || personalInfo.Nblitigations != null || personalInfo.NbLawyerFirms != null
            || personalInfo.CanShareInfos != null || personalInfo.PrecisionHousing != null || personalInfo.PrecisionLitigation != null)
        {
            tmpVide = 0;
        }

        if(personalInfo.FrenchMotherTongue == true)
        {
            tmpMaternelLanguage = "Français";
        }
        else
        {
            tmpMaternelLanguage = personalInfo.OtherLanguage;
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
            { "LangueMat", tmpMaternelLanguage },
            { "SituationPro", personalInfo.ProfessionalSituation },
            { "NiveauEtude", personalInfo.LevelOfStudy },
            { "PrecisionEtudes", personalInfo.LevelOfStudyDetails },
            { "Profession", personalInfo.Job },
            { "PrecisionCarrierePro", personalInfo.JobDetails },
            { "CadreVie",  personalInfo.LivingEnvironment},
            { "​TypeLogement", personalInfo.TypeOfHousing },
            { "DroitLogement", personalInfo.HousingLaw },
            { "​PrecisionLogement", personalInfo.PrecisionHousing },
            { "​NbLitiges", personalInfo.Nblitigations.ToString() },
            { "​PrécisionLitiges", personalInfo.PrecisionLitigation },
            { "​tmpGenre", folder.Sex.ToString() },
            { "​tmpCV", personalInfo.Cv.ToString() },
            { "​tmpSituationpro", personalInfo.ProfessionalSituation },
            { "​tmpLitige", personalInfo.OngoingLitigation.ToString() },
            { "​tmpInfogenerale", tmpInfogenerale.ToString() },
            { "​tmpLogement", tmpLogement.ToString() },
            { "​tmpVide", tmpVide.ToString() }
        };

        ReferenceElementDTO referenceElement = new() { ReferenceId = refId, Data = JsonHelper.GetJsonString(newElem) };
        return await client.CreateReferenceElement(referenceElement);
    }
}
