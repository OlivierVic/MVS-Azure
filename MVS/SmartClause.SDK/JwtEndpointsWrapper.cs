using SmartClause.SDK.DTO;
using System.Net;
using System.Threading.Tasks;

namespace Smartclause.SDK
{
    public partial class Client
    {
        public async Task<GenerateJwtResponse> GenerateJwt(GenerateJwtRequest requestBody)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest("/api/Jwt/Generate", "POST", bodyObject: requestBody);
            return await this.GetResponseAsObject<GenerateJwtResponse>(request);
        }
    }
}