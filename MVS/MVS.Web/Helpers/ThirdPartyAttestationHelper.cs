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

public static class ThirdPartyAttestationHelper
{
    public static async Task<string> GenerateThirdPartyAttestation(IConfiguration configuration, Client client, Vault folder, ApplicationUser user)
    {
        string refName = "ThirdPartyAttestation";

        string templateId = configuration.GetValue<string>($"SCM:Templates:{refName}");
        string tenantId = configuration.GetValue<string>("SCM:TenantId");

        ReferenceDTO reference = await SCMHelper.CheckTemplateReference(client, refName, tenantId, templateId);
        ReferenceDTO refElement = (await client.SearchReferenceElements(reference.Id, new Dictionary<string, string>() { { "VaultId", folder.Id } }, tenantId)).FirstOrDefault();

        string refElementId = refElement?.Id;
        if (refElement == null)
        {
            ReferenceElementDTO newRefElement = await CreateThirdPartyAttestationReferenceElem(client, folder, reference.Id);
            refElementId = newRefElement.Id;
        }

        return await SCMHelper.GenerateContractWithReference(configuration, client, refElementId, templateId, $"Attestation tiers - {folder.FirstName} {folder.LastName}", user, tenantId);
    }

    public static async Task<ReferenceElementDTO> CreateThirdPartyAttestationReferenceElem(Client client, Vault folder, string refId)
    {
        // Create Vault's ReferenceElem in MissionLetterReference
        Dictionary<string, string> newElem = new()
        {
            { "VaultId", folder.Id },
            { "Nom", folder.LastName },
            { "Prénom", folder.FirstName },
        };
        ReferenceElementDTO referenceElement = new() { ReferenceId = refId, Data = JsonHelper.GetJsonString(newElem) };
        return await client.CreateReferenceElement(referenceElement);
    }
}
