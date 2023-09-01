using SmartClause.SDK.DTO;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Smartclause.SDK
{
    public partial class Client
    {
        public async Task<List<InvitationResult>> GetInvitations(string tenantId = null)
        {
            HttpWebRequest request = await CreateHttpWebRequest("/api/Invitation/List", "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await GetResponseAsObject<List<InvitationResult>>(request);
        }

        public async Task<InvitationResult> CompleteInvitation(string invitationId)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/Invitation/{invitationId}/Complete", "POST");
            return await GetResponseAsObject<InvitationResult>(request);
        }

        public async Task<InvitationResult> GetInvitationById(string invitationId)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/Invitation/{invitationId}", "GET");
            return await GetResponseAsObject<InvitationResult>(request);
        }
    }
}