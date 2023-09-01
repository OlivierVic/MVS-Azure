using SmartClause.SDK.DTO;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Smartclause.SDK
{
    public partial class Client
    {
        public async Task<AvailableTemplateCountResult> GetTemplateAvailableCount(StatisticsRequest statisticsRequest, string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Statistics/Template/Available/Count", "POST",
                bodyObject: statisticsRequest);
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await GetResponseAsObject<AvailableTemplateCountResult>(request);
        }

        public async Task<GeneratedCountResult> GetGeneratedCount(StatisticsRequest statisticsRequest, string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Statistics/Contract/Generated/Count", "POST",
                bodyObject: statisticsRequest);
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await GetResponseAsObject<GeneratedCountResult>(request);
        }

        public async Task<PercentageSignedResult> GetPercentageSigned(StatisticsRequest statisticsRequest, string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Statistics/Contract/Signed/Percentage", "POST",
                bodyObject: statisticsRequest);
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await GetResponseAsObject<PercentageSignedResult>(request);
        }

        public async Task<AverageNegotiationTimeResult> GetAverageNegotiationTime(StatisticsRequest statisticsRequest, string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Statistics/Contract/NegotiationTime/Average",
                "POST",
                bodyObject: statisticsRequest);
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await GetResponseAsObject<AverageNegotiationTimeResult>(request);
        }

        public async Task<List<BreakdownPairResponse<string>>> GetContractTypeBreakdown(StatisticsRequest statisticsRequest, string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Statistics/Contract/Type/Breakdown",
                "POST",
                bodyObject: statisticsRequest);
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await GetResponseAsObject<List<BreakdownPairResponse<string>>>(request);
        }

        public async Task<AverageResult> GetAverageFinancialAmount(StatisticsRequest statisticsRequest, string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Statistics/Contract/FinancialAmount/Average",
                "POST",
                bodyObject: statisticsRequest);
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await GetResponseAsObject<AverageResult>(request);
        }

        public async Task<List<BreakdownPairResponse<string>>> GetContractCountryBreakdown(StatisticsRequest statisticsRequest, string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Statistics/Contract/Country/Breakdown",
                "POST",
                bodyObject: statisticsRequest);

            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await GetResponseAsObject<List<BreakdownPairResponse<string>>>(request);
        }

        public async Task<List<BreakdownPairResponse<ContractStatusEnum>>> GetContractStatusBreakdown(StatisticsRequest statisticsRequest, string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Statistics/Contract/Status/Breakdown",
                "POST",
                bodyObject: statisticsRequest);

            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await GetResponseAsObject<List<BreakdownPairResponse<ContractStatusEnum>>>(request);
        }
    }
}