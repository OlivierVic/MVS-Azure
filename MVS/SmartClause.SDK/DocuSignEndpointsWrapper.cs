using Microsoft.AspNetCore.WebUtilities;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SmartClause.SDK.DTO;
using System.Collections.Generic;
using Smartclause.SDK.DTO;

namespace Smartclause.SDK
{
    public partial class Client
    {
        public async Task<ContractDto> GetContractByEnvelope(string envelopeId)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/Contract/GetByEnvelope/{envelopeId}", "GET");
            return await GetResponseAsObject<ContractDto>(request);
        }

        public async Task SCMDocuSignHook(string body)
        {
            HttpWebRequest request = await CreateHttpWebHookRequest($"/api/Docusign/Hook", bodyObject: body);
            await request.GetResponseAsync();
        }
    }
}