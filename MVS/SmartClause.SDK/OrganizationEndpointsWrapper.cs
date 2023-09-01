using Elskom.Generic.Libs;
using SmartClause.SDK.DTO;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Smartclause.SDK
{
    public partial class Client
    {
        private const string _privateKey = "SBZ&PL4A1l";

        public async Task<CreateTenantResult> AddTenant(string name)
        {
            try
            {
                HttpWebRequest request =
                await this.CreateHttpWebRequest($"/api/Tenant/{name}/Add", "GET");
                var response = await this.GetResponseAsObject<CreateTenantResponse>(request);
                var UTF8Encoding = new System.Text.UTF8Encoding();
                BlowFish fish = new BlowFish(UTF8Encoding.GetBytes(_privateKey));

                return new CreateTenantResult
                {
                    Tenant = response.Tenant,
                    Email = fish.DecryptCBC(response.EncodedEmail),
                    Password = fish.DecryptCBC(response.EncodedPassword)
                };
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<TenantDTO>> ListTenants()
        {
            HttpWebRequest request =
           await this.CreateHttpWebRequest("/api/Tenant/List", "GET");
            return await this.GetResponseAsObject<List<TenantDTO>>(request);
        }

        public async Task<bool> TenantExists(string name)
        {
            HttpWebRequest request =
           await this.CreateHttpWebRequest($"/api/Tenant/{name}/Exists", "GET");
            return await this.GetResponseAsObject<bool>(request);
        }

    }
}