using Smartclause.SDK.DTO;
using SmartClause.SDK.DTO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Smartclause.SDK
{
    public partial class Client
    {
        public async Task<List<ReferenceDTO>> GetReferences(string tenantId = null)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest("/api/Reference", "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await this.GetResponseAsObject<List<ReferenceDTO>>(request);
        }

        public async Task<ReferenceDTO> GetReference(string refId, string tenantId = null)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Reference/{refId}", "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await this.GetResponseAsObject<ReferenceDTO>(request);
        }

        public async Task<ReferenceDTO> SearchReference(string name, string tenantId = null)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Reference/Search/{name}", "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await this.GetResponseAsObject<ReferenceDTO>(request);
        }

        /**
         *  Example how to use SearchReferenceElements :
         *
         *  var searchItems = new Dictionary<string, string>();
         *  searchItems.Add("Entity", "Bei Lao Zi (Shanghai) Food Trading Co., Ltd.ddddddddddd");
         *  var refElements = await _client.SearchReferenceElements(refId, searchItems);
         * */
        public async Task<List<ReferenceDTO>> SearchReferenceElements(string refId, Dictionary<string, string> searchItems, string tenantId = null)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Reference/{refId}/Elements/Search", "POST", bodyObject: new { search = searchItems });
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await this.GetResponseAsObject<List<ReferenceDTO>>(request);
        }

        public async Task UpdateReference(ReferenceDTO request)
        {
            HttpWebRequest webRequest = await this.CreateHttpWebRequest($"/api/Reference/{request}", "PUT", bodyObject: request);
            await this.GetResponseAsObject<ReferenceDTO>(webRequest);
        }

        public async Task<ReferenceDTO> CreateReference(ReferenceDTO request)
        {
            HttpWebRequest webRequest = await this.CreateHttpWebRequest($"/api/Reference", "POST", bodyObject: request);
            return await this.GetResponseAsObject<ReferenceDTO>(webRequest);
        }

        public async Task BulkCreateReferenceElements(List<ReferenceElementDTO> request)
        {
            HttpWebRequest webRequest = await this.CreateHttpWebRequest($"/api/Reference/Elements", "POST", bodyObject: request);
            await webRequest.GetResponseAsync();
        }

        public async Task DeleteReference(string refId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Reference/{refId}", "DELETE");
            await request.GetResponseAsync();
        }

        public async Task DeleteReferenceElements(string refId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Reference/{refId}/Elements", "DELETE");
            await request.GetResponseAsync();
        }

        public async Task<ReferenceElementDTO> GetReferenceElement(string refId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Reference/Element/{refId}", "GET");
            return await this.GetResponseAsObject<ReferenceElementDTO>(request);
        }

        public async Task<ReferenceElementDTO> UpdateReferenceElement(ReferenceElementDTO referenceElement, bool updateContractRefElems = false)
        {
            HttpWebRequest webRequest = await this.CreateHttpWebRequest($"/api/Reference/Element/{referenceElement.Id}", "PUT", bodyObject: new { referenceElement, updateContractRefElems });
            return await this.GetResponseAsObject<ReferenceElementDTO>(webRequest);
        }

        public async Task<ReferenceElementDTO> UpdateReferencedName(string refId, string referentialName)
        {
            HttpWebRequest webRequest = await this.CreateHttpWebRequest($"/api/Reference/{refId}/Name", "POST", bodyObject: referentialName);
            return await this.GetResponseAsObject<ReferenceElementDTO>(webRequest);
        }

        public async Task<ReferenceElementDTO> UpdateReferenceKeyFieldName(string refId, string referentialKey)
        {
            HttpWebRequest webRequest = await this.CreateHttpWebRequest($"/api/Reference/{refId}/KeyFieldName", "POST", bodyObject: referentialKey);
            return await this.GetResponseAsObject<ReferenceElementDTO>(webRequest);
        }

        public async Task<ReferenceElementDTO> CreateReferenceElement(ReferenceElementDTO request)
        {
            HttpWebRequest webRequest = await this.CreateHttpWebRequest($"/api/Reference/Element", "POST", bodyObject: request);
            return await this.GetResponseAsObject<ReferenceElementDTO>(webRequest);
        }

        public async Task DeleteReferenceElement(string refId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Reference/Element/{refId}", "DELETE");
            await request.GetResponseAsync();
        }

        public async Task<TemplateReferenceCompletionsResponseDto> GetReferenceCompletions(string referenceId)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/Reference/{referenceId}/Completions", "GET");
            return await GetResponseAsObject<TemplateReferenceCompletionsResponseDto>(request);
        }
    }
}
