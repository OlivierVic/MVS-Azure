using Smartclause.SDK.DTO;
using System.Net;
using System.Threading.Tasks;

namespace Smartclause.SDK
{
    public partial class Client
    {

        public async Task ArchiveMulti(ArchiveMultiRequest archiveMultiRequest, string tenantId = null)
        {
            HttpWebRequest request =
                await CreateHttpWebRequest($"/api/Multi/Archive", "DELETE", bodyObject: archiveMultiRequest);
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            await request.GetResponseAsync();
        }

        public async Task MoveMulti(MoveMultiRequest moveMultiRequest, string tenantId = null)
        {
            HttpWebRequest request =
                await CreateHttpWebRequest($"/api/Multi/Move", "PUT", bodyObject: moveMultiRequest);
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            await request.GetResponseAsync();
        }
    }
}