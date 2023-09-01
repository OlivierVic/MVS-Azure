using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SmartClause.SDK.DTO
{
    public class InvitationResult
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum StatusEnum
        {
            Sent,
            Completed,
        }

        public string Id { get; set; }
        public string Token { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public StatusEnum Status { get; set; }
        public string TenantId { get; set; }
    }
}