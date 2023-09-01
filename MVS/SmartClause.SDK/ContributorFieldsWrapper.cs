using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Smartclause.SDK.DTO;
using SmartClause.SDK.DTO;

namespace Smartclause.SDK
{
    public partial class Client
    {
        public async Task<ContributorField> CreateContributorField(string title)
        {
            HttpWebRequest webRequest = await this.CreateHttpWebRequest($"/api/ContributorField/Create/{title}", "POST");
            return await this.GetResponseAsObject<ContributorField>(webRequest);
        }

        public async Task<List<ContributorField>> GetAllContributorField()
        {
            HttpWebRequest webRequest = await this.CreateHttpWebRequest($"/api/ContributorField/GetAll", "GET");
            return await this.GetResponseAsObject<List<ContributorField>>(webRequest);
        }
    }
}