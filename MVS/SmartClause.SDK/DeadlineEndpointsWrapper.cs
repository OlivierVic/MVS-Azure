using SmartClause.SDK.DTO;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Smartclause.SDK
{
    public partial class Client
    {
        public async Task<List<DeadlineResponse>> GetDeadlinesOfMonth(int year, int month)
        {
            HttpWebRequest request =
                await this.CreateHttpWebRequest($"/api/Deadline/GetDeadlinesOfMonth/{year}/{month}", "GET");
            return await GetResponseAsObject<List<DeadlineResponse>>(request);
        }

        public async Task<List<DeadlinesPerMonthBreakdownResponse>> GetDeadlinesPerMonth(int year)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Deadline/DeadlinesPerMonth/{year}", "GET");
            return await GetResponseAsObject<List<DeadlinesPerMonthBreakdownResponse>>(request);
        }

        public async Task<DeadlineResponse> UpdateDeadlineIsDone(UpdateDeadlineIsDoneRequest updateDeadlineIsDoneRequest)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest("/api/Deadline/UpdateIsDone", "PUT",
                bodyObject: updateDeadlineIsDoneRequest);
            return await GetResponseAsObject<DeadlineResponse>(request);
        }

        public async Task<DeadlineResponse> GetDeadline(string deadlineId, string tenantId = null)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Deadline/{deadlineId}", "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await GetResponseAsObject<DeadlineResponse>(request);
        }
    }
}