using Microsoft.AspNetCore.WebUtilities;
using Smartclause.SDK.DTO;
using SmartClause.SDK.DTO;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Smartclause.SDK
{
    public partial class Client
    {
        public async Task<List<RemindersPerMonthBreakdownResponse>> GetRemindersPerMonth(string endUserId, int year)
        {
            Dictionary<string, string> parameters = this.GetParametersDictionary(new
            {
                endUserId,
                year
            });
            string endpoint = QueryHelpers.AddQueryString("api/Reminder/ForYear/Breakdown", parameters);
            HttpWebRequest request = await this.CreateHttpWebRequest(endpoint, "GET");
            return await GetResponseAsObject<List<RemindersPerMonthBreakdownResponse>>(request);
        }

        public async Task<List<ReminderDto>> ListRemindersForDeadline(ListRemindersForDeadlineRequestDto requestDto)
        {
            Dictionary<string, string> parameters = this.GetParametersDictionary(requestDto);
            string endpoint = QueryHelpers.AddQueryString("api/Reminder/ListForDeadline", parameters);
            HttpWebRequest request = await this.CreateHttpWebRequest(endpoint, "GET");
            return await this.GetResponseAsObject<List<ReminderDto>>(request);
        }

        public async Task<List<ReminderForMonthResponse>> GetRemindersOfMonth(string endUserId, int year, int month)
        {
            Dictionary<string, string> parameters = this.GetParametersDictionary(new
            {
                endUserId,
                year,
                month
            });
            string endpoint = QueryHelpers.AddQueryString("api/Reminder/ListForMonth", parameters);
            HttpWebRequest request = await this.CreateHttpWebRequest(endpoint, "GET");
            return await this.GetResponseAsObject<List<ReminderForMonthResponse>>(request);
        }

        public async Task<ReminderDto> GetReminder(string reminderId)
        {
            string endpoint = $"api/Reminder/{reminderId}";
            HttpWebRequest request = await this.CreateHttpWebRequest(endpoint, "GET");
            return await this.GetResponseAsObject<ReminderDto>(request);
        }

        public async Task<ReminderForMonthResponse> UpdateReminderIsDone(
            UpdateReminderIsDoneRequest updateReminderIsDoneRequest)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest("/api/Reminder/UpdateIsDone", "PUT",
                bodyObject: updateReminderIsDoneRequest);
            return await GetResponseAsObject<ReminderForMonthResponse>(request);
        }

        public async Task<ReminderDto> AddReminder(AddReminderRequest addReminderRequest)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest("/api/Reminder/", "POST",
                bodyObject: addReminderRequest);
            return await GetResponseAsObject<ReminderDto>(request);
        }

        public async Task<List<string>> GetReminderSharedUsers(string reminderId)
        {
            string endpoint = $"api/Reminder/{reminderId}/SharedUsers";
            HttpWebRequest request = await this.CreateHttpWebRequest(endpoint, "GET");
            return await this.GetResponseAsObject<List<string>>(request);
        }

        public async Task<ReminderDto> UpdateReminder(string reminderId, UpdateReminderRequest updateReminderRequest)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Reminder/{reminderId}", "POST",
                bodyObject: updateReminderRequest);
            return await GetResponseAsObject<ReminderDto>(request);
        }

        public async Task DeleteReminder(string reminderId, DeleteReminderRequest deleteReminderRequest)
        {
            HttpWebRequest request = await this.CreateHttpWebRequest($"/api/Reminder/{reminderId}", "DELETE",
                bodyObject: deleteReminderRequest);
            await request.GetResponseAsync();
        }
    }
}