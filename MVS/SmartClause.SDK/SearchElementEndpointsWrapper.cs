using Smartclause.SDK.DTO;
using SmartClause.SDK.DTO;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Smartclause.SDK
{
    public partial class Client
    {
        public async Task<QuerySearchResult> QuerySearch(string query, string endUserId, string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/Search/QuerySearch?query={query}&endUserId={endUserId}", "POST");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await GetResponseAsObject<QuerySearchResult>(request);
        }

        public async Task<QuerySearchResult> QueryDrive(string endUserId, string folderId, string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/Search/QueryDrive?folderId={folderId}&endUserId={endUserId}", "POST");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await GetResponseAsObject<QuerySearchResult>(request);
        }

        public async Task<SearchResult> AdvancedSearch(SearchRequest searchRequest, string tenantId = null)
        {
            HttpWebRequest request =
                await CreateHttpWebRequest($"/api/Search/AdvancedSearch", "POST", bodyObject: searchRequest);

            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }

            return await GetResponseAsObject<SearchResult>(request);
        }

        public async Task<SearchResult> AdvancedSearches(AdvancedSearchRequest searchRequests)
        {
            HttpWebRequest request =
              await CreateHttpWebRequest($"/api/Search/AdvancedSearches", "POST", bodyObject: searchRequests);
            return await GetResponseAsObject<SearchResult>(request);
        }


        public async Task<List<string>> GetLanguages()
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Search/GetLanguages", "GET");
            return await GetResponseAsObject<List<string>>(request);
        }

        public async Task<List<Template>> GetContractTypes()
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Search/GetContractTypes", "GET");
            return await GetResponseAsObject<List<Template>>(request);
        }

        public async Task<List<string>> GetCountries()
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Search/GetCountries", "GET");
            return await GetResponseAsObject<List<string>>(request);
        }
    }
}