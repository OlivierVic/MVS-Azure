using SmartClause.SDK.DTO;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Smartclause.SDK
{
    public partial class Client
    {
        public async Task<List<ContractRefElementDto>> GetContractsRefs(string contractId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Reference/Contract/{contractId}/Elements", "GET");
            return await this.GetResponseAsObject<List<ContractRefElementDto>>(request);
        }

    }
}
