using System.Collections.Generic;

namespace SmartClause.SDK.DTO.Notification
{
    public class PostNotificationRequestDto
    {
        public NotificationResponse.NotificationTypeEnum Type { get; set; }
        public string NotifierUserId { get; set; }
        public List<string> NotifiedUserIds { get; set; }
        public List<string> NotifiedUserEmails { get; set; }
        public string ContractId { get; set; }
        public string TemplateId { get; set; }
        public string DeadlineId { get; set; }
        public string ReminderId { get; set; }
        public string WorkflowContractStepId { get; set; }
        public string WorkflowContractStepName { get; set; }
        public string SharedElementId { get; set; }
        public int? QuestionId { get; set; }
        public string QuestionName { get; set; }
    }
}
