// <copyright file="JsonHelper.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

using MVS.Common;
using MVS.Common.Models;
using Newtonsoft.Json;
using Smartclause.SDK;
using Smartclause.SDK.DTO;
using SmartClause.SDK.DTO;
using System.Dynamic;

namespace MVS.Web.Helpers;

public static class SCMHelper
{
    public static async Task<string> GenerateContractWithReference(IConfiguration configuration, Client client, string refElementId, string templateId,
                                                            string contractTitle, ApplicationUser user, string tenantId)
    {
        ReferenceElementDTO referenceElementDTO = await client.GetReferenceElement(refElementId);
        Dictionary<string, string> referenceElementValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(referenceElementDTO.Data);

        //Dictionary<string, string> test = JsonConvert.DeserializeObject<Dictionary<string, string>>(referenceElementDTO.Data);

        referenceElementValues.Add("__uid", referenceElementDTO.Id);
        referenceElementValues.Add("RefId", referenceElementDTO.ReferenceId);

        /*dynamic d = new ExpandoObject();
        d.data = referenceElementDTO.Data;*/
        //d.selectedOptions = 

        PrepareContractResult result = await client.PrepareContract(templateId, configuration.GetValue<string>($"SCM:Email"), null,
                contractTitle, "61df29ca-de8d-4e82-96fe-fdfd80190f77"/*string.Empty*/, "", null, null,
                new List<string> { JsonConvert.SerializeObject(referenceElementValues) }, user.Id, null, user.Email, tenantId: tenantId);

        return result.ContractId;
    }

    public static async Task<string> GenerateContractWithReferenceAndOption(IConfiguration configuration, Client client, string refElementId, string templateId,
                                                            string contractTitle, ApplicationUser user, string tenantId)
    {
        ReferenceElementDTO referenceElementDTO = await client.GetReferenceElement(refElementId);
        Dictionary<string, string> referenceElementValues = JsonConvert.DeserializeObject<Dictionary<string, string>>(referenceElementDTO.Data);

        //Dictionary<string, string> test = JsonConvert.DeserializeObject<Dictionary<string, string>>(referenceElementDTO.Data);

        referenceElementValues.Add("__uid", referenceElementDTO.Id);
        referenceElementValues.Add("RefId", referenceElementDTO.ReferenceId);

        dynamic d = new ExpandoObject();
        d.data = referenceElementDTO.Data;
        //d.selectedOptions = 

        PrepareContractResult result = await client.PrepareContract(templateId, configuration.GetValue<string>($"SCM:Email"), /*null*/JsonConvert.SerializeObject(d),
                contractTitle, "61df29ca-de8d-4e82-96fe-fdfd80190f77"/*string.Empty*/, "", null, null,
                /*new List<string> { JsonConvert.SerializeObject(referenceElementValues) }*/null, user.Id, null, user.Email, tenantId: tenantId);

        return result.ContractId;
    }

    public static async Task<string> GenerateContract(IConfiguration configuration, Client client, string templateId, string tenantId, string contractTitle, ApplicationUser user)
    {
        PrepareContractResult result = await client.PrepareContract(templateId, configuration.GetValue<string>($"SCM:Email"), null,
                contractTitle, string.Empty, "", null, null,
                new List<string> {}, user.Id, null, user.Email, tenantId: tenantId);

        return result.ContractId;
    }

    public static async Task<ReferenceDTO> CheckTemplateReference(Client client, string refName, string tenantId, string templateId)
    {
        ReferenceDTO reference = await client.SearchReference(refName, tenantId);
        if (reference == null)
        {
            //Create MissionLetterReference
            reference = new()
            {
                Name = refName,
                ValueFieldName = refName,
                TenantId = tenantId
            };
            reference = await client.CreateReference(reference);
        }
        return reference;
    }
}
