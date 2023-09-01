namespace SmartClause.SDK.DTO.Notification
{
    public class NotificationDelegationDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ReceiverId { get; set; }
        public bool IsActivated { get; set; }
        public string UserEmail { get; set; }
        public string ReceiverEmail { get; set; }
    }
}
