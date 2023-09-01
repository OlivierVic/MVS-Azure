using System.Collections.Generic;

namespace SmartClause.SDK.DTO.Notification
{
    public class UpdateReadNotificationsRequest
    {
        public List<string> NotificationIds { get; set; }
        public string UserId { get; set; }
        public bool IsRead { get; set; }
        public string UserEmail { get; set; }
        public string Lang { get; set; }
    }
}