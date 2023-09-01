using Smartclause.SDK.DTO;
using SmartClause.SDK.DTO;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Smartclause.SDK
{
    public partial class Client
    {
        public async Task<ContractAnnexDto> GetContractAnnex(string annexId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/ContractAnnexes/Annex/{annexId}", "GET");
            return await this.GetResponseAsObject<ContractAnnexDto>(request);
        }
        public async Task<List<ContractAnnexDto>> GetContractAnnexes(string contractId)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/ContractAnnexes/{contractId}", "GET");
            return await this.GetResponseAsObject<List<ContractAnnexDto>>(request);
        }

        public async Task<ContractAnnexDto> AddContractAnnex(AddContractAnnexesRequest contractAnnexesRequest)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/ContractAnnexes/Add", "POST", bodyObject: contractAnnexesRequest);
            return await this.GetResponseAsObject<ContractAnnexDto>(request);
        }

        public async Task DeleteAnnex(string id)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/ContractAnnexes/Delete/{id}", "POST");
            await request.GetResponseAsync();
        }

        public async Task Update(ContractAnnexDto annex)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/ContractAnnexes/Update", "PUT", bodyObject: annex);
            await request.GetResponseAsync();
        }

        public async Task<List<FileDto>> SearchFilesForAddingAnnex(string tenantId, SearchFilesForAddingAnnexRequest requestBody)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/ContractAnnexes/SearchFilesForAddingAnnex", "POST", bodyObject: requestBody);
            request.Headers.Add("TenantId", tenantId);
            return await this.GetResponseAsObject<List<FileDto>>(request);
        }
    }
}
