using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using SmartClause.SDK.DTO;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Smartclause.SDK
{
    public partial class Client
    {
        public async Task<TermSheetElementResponse> GetContractTermSheetElement(int elementId)
        {
            var endpoint = $"/api/Contract/TermSheet/Element/Get/{elementId}";
            HttpWebRequest request = await this.CreateHttpWebRequest(endpoint, "GET");

            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            var sr = new StreamReader(responseStream);
            var responseString = await sr.ReadToEndAsync();

            return JsonConvert.DeserializeObject<TermSheetElementResponse>(responseString);
        }

        public async Task<string> GetContractTermSheetHtml(string contractId, string endUserId, string lang, string tenantId = null)
        {
            var baseEndpoint = $"/api/Contract/TermSheet/Html/Get/{lang}/{contractId}";
            var endpoint = QueryHelpers.AddQueryString(baseEndpoint, "endUserId", endUserId);
            HttpWebRequest request = await this.CreateHttpWebRequest(endpoint, "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            WebResponse response = await request.GetResponseAsync();
            Stream responseStream = response.GetResponseStream();
            var sr = new StreamReader(responseStream);
            var responseString = await sr.ReadToEndAsync();

            return responseString;
        }

        public async Task AddOrUpdateCommonTermSheetElement(AddOrUpdateCommonTermSheetElementBody body, string tenantId = null)
        {
            string endpoint = body.ContractId != null
                ? "/api/Contract/TermSheet/Element/AddOrUpdate"
                : "/api/File/TermSheet/Element/AddOrUpdate";
            HttpWebRequest request = await CreateHttpWebRequest(endpoint,
                "POST",
                bodyObject: body);
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            await request.GetResponseAsync();
        }

        public async Task DeleteTermSheetElement(string contractId, string fileId, int elementId, string tenantId)
        {
            string endpointName = contractId != null ? "Contract" : "File";
            string id = contractId ?? fileId;
            string endpoint = $"/api/{endpointName}/TermSheet/Element/{id}/Delete/{elementId}";
            HttpWebRequest request = await this.CreateHttpWebRequest(endpoint, "DELETE");
            if (!string.IsNullOrEmpty(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            await request.GetResponseAsync();
        }

        public async Task<ExportTermSheetResponse> ExportTermSheet(string contractId, string fileId, string contractStatus, string tenantId = null)
        {
            string endpointName = contractId != null ? "Contract" : "File";
            string id = contractId ?? fileId;
            string endpoint = $"/api/{endpointName}/TermSheet/Export/{id}/{contractStatus ?? ""}";

            HttpWebRequest request = await CreateHttpWebRequest(endpoint, "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await GetResponseAsObject<ExportTermSheetResponse>(request);
        }

        public async Task<List<TermsheetElementTemplate>> GetTermSheetElementTemplates(string tenantId, string query = "")
        {
            string endpoint = $"/api/Contract/Termsheet/TenantElements";
            HttpWebRequest request = await CreateHttpWebRequest(endpoint,
               "POST",
               bodyObject: query);
            request.Headers.Add("TenantId", tenantId);
            return await GetResponseAsObject<List<TermsheetElementTemplate>>(request);
        }

    }
}