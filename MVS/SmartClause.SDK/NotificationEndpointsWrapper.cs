using Microsoft.AspNetCore.WebUtilities;
using SmartClause.SDK.DTO.Notification;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Smartclause.SDK
{
    public partial class Client
    {
        public async Task<List<NotificationResponse>> GetNotifications(string lang, string userId, string userEmail, string tenantId = null)
        {
            string endpoint = QueryHelpers.AddQueryString("api/Notification/GetNotifications", new Dictionary<string, string>
            {
                {"lang", lang},
                {"userId", userId},
                {"userEmail", userEmail}
            });
            HttpWebRequest request =
                await this.CreateHttpWebRequest(endpoint, "GET");
            if (!string.IsNullOrWhiteSpace(tenantId))
            {
                request.Headers.Add("TenantId", tenantId);
            }
            return await this.GetResponseAsObject<List<NotificationResponse>>(request);
        }

        public async Task<List<NotificationResponse>> UpdateReadNotifications(UpdateReadNotificationsRequest updateRequest)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest("api/Notification/UpdateReadNotifications", "POST",
                bodyObject: updateRequest);
            return await this.GetResponseAsObject<List<NotificationResponse>>(request);
        }

        public async Task<List<CollaborationInvitationResponse>> GetCollaborationInvitations(string contractId)
        {
            string endpoint = QueryHelpers.AddQueryString("api/Notification/GetCollaborationInvitations",
                new Dictionary<string, string>
                {
                    { "contractId", contractId },
                });
            HttpWebRequest request = await this.CreateHttpWebRequest(endpoint, "GET");
            return await this.GetResponseAsObject<List<CollaborationInvitationResponse>>(request);
        }

        public async Task<List<NotificationDelegationDto>> GetReceiverDelegations(string userId, string userEmail)
        {
            string endpoint = QueryHelpers.AddQueryString("api/Notification/GetReceiverDelegations", new Dictionary<string, string>
            {
                {"userId", userId},
                {"userEmail", userEmail}
            });
            HttpWebRequest request =
                await this.CreateHttpWebRequest(endpoint, "GET");
            return await this.GetResponseAsObject<List<NotificationDelegationDto>>(request);
        }

        public async Task<NotificationDelegationDto> GetDelegation(string userId, string userEmail)
        {
            string endpoint = QueryHelpers.AddQueryString("api/Notification/Delegation", new Dictionary<string, string>
            {
                {"userId", userId},
                {"userEmail", userEmail}
            });
            HttpWebRequest request =
                await this.CreateHttpWebRequest(endpoint, "GET");
            return await this.GetResponseAsObject<NotificationDelegationDto>(request);
        }


        public async Task<NotificationDelegationDto> AddOrUpdateDelegation(NotificationDelegationDto addRequest)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest("api/Notification/Delegation/AddOrUpdate", "POST",
                bodyObject: addRequest);
            return await this.GetResponseAsObject<NotificationDelegationDto>(request);
        }

        public async Task<PostNotificationResponseDto> PostNotification(PostNotificationRequestDto requestDto)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest("api/Notification/PostNotification", "POST",
                bodyObject: requestDto);
            return await this.GetResponseAsObject<PostNotificationResponseDto>(request);
        }
    }
}
