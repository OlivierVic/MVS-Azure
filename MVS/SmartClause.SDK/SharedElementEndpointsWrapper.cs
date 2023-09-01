using SmartClause.SDK.DTO;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Smartclause.SDK
{
    public partial class Client
    {
        public async Task RequestAccess(string elementId, string userId, List<string> emails, string msg, bool canShared)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/SharedElement/CreateRequest/{userId}/{elementId}", "POST", bodyObject: new { emails, requestMessage = msg, canShared });
            await request.GetResponseAsync();
        }

        public async Task CancelRequestAccess(string elementId, string userId, List<string> emails)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/SharedElement/CancelRequest/{userId}/{elementId}", "POST", bodyObject: emails);
            await request.GetResponseAsync();
        }

        public async Task ValidRequest(string sharedElementId, List<string> emails, string notifierEndUserId)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/SharedElement/ValidRequest/{sharedElementId}/{notifierEndUserId}", "POST", bodyObject: emails);
            await request.GetResponseAsync();
        }

        public async Task DenyRequest(string sharedElementId, List<string> emails, string notifierEndUserId)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/SharedElement/DenyRequest/{sharedElementId}/{notifierEndUserId}", "POST", bodyObject: emails);
            await request.GetResponseAsync();
        }

        public async Task<List<SharedElementDTO>> GetAllSharedElements(string elementId)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/SharedElement/GetAllSharedElements/{elementId}", "GET");
            return await GetResponseAsObject<List<SharedElementDTO>>(request);
        }

        public async Task RemoveUserShared(string userId, string elementId)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/SharedElement/RemoveUserShared/{userId}/{elementId}", "POST");
            await request.GetResponseAsync();
        }

        public async Task<CheckAccess_Result> CheckAccess(string userId, string userEmail, string elementId)
        {

            HttpWebRequest request = await CreateHttpWebRequest($"/api/SharedElement/CheckAccess", "POST", bodyObject: new { userId = userId, userEmail = userEmail, elementId = elementId });
            return await GetResponseAsObject<CheckAccess_Result>(request);
        }

        public async Task<List<GetAccessList_Result>> GetAccessList(string elementId)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/SharedElement/GetAccessList/{elementId}", "GET");
            return await GetResponseAsObject<List<GetAccessList_Result>>(request);
        }

        public async Task<UpdateElementsResponse> UpdateSharedElements(string userId, string elementId, UpdateElementsRequest updateRequest)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/SharedElement/UpdateElements/{userId}/{elementId}", "POST", bodyObject: updateRequest);
            return await GetResponseAsObject<UpdateElementsResponse>(request);
        }


        public async Task AddSharedElements(string userId, string elementId, int access, bool canShared)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/SharedElement/AddSharedElement/{userId}/{elementId}", "POST", bodyObject: new { access, canShared });
            await request.GetResponseAsync();
        }

        public async Task DeleteSharedElement(string userId, string elementId)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/SharedElement/DeleteSharedElement/{userId}/{elementId}", "POST");
            await request.GetResponseAsync();
        }

        public async Task<bool> GetDriveAccess(string userId, string elementId)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/SharedElement/DriveCanAccess/{userId}/{elementId}", "GET");
            return await GetResponseAsObject<bool>(request);
        }

        public async Task<Dictionary<string, bool>> DriveCanAccessItems(string userId, List<string> elementIds)
        {
            HttpWebRequest request = await CreateHttpWebRequest($"/api/SharedElement/DriveCanAccessItems/{userId}", "POST", bodyObject: elementIds);
            return await GetResponseAsObject<Dictionary<string, bool>>(request);
        }
    }
}