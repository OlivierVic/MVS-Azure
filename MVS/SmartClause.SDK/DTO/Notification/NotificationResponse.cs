using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace SmartClause.SDK.DTO.Notification
{
    public class NotificationResponse
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum NotificationTypeEnum
        {
            TemplateUpdate,
            CollaborationInvitation,
            CommentMention,
            ChatMention,
            SignInvitation,
            DocumentSigned,
            DeadlineDate,
            ReminderDate,
            DelegationEnabled,
            DelegationDisabled,
            CollaborationStepInvitation,
            RequestSharedElement,
            ValidSharedElement,
            DenySharedElement,
            CancelSharedElement,
            AddSharedElement,
            RequestSharedElementWithCanShared,
            DocumentUpdated,
            InvitedToQuestion,
            RemovedFromQuestion,
            ClosedQuestion,
            ReopenedQuestion,
            ChatMessage,
            OpenedDocument,
            StartedRevision,
            CommentPosted,
            WorkflowStepUserValidated,
            WorkflowStepUserInvalidated,
        }

        public string Id { get; set; }
        public NotificationTypeEnum Type { get; set; }
        public string ContractId { get; set; }
        public string ContractName { get; set; }
        public string ProjectName { get; set; }
        public string TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string DeadlineId { get; set; }
        public string DeadlineName { get; set; }
        public string ReminderId { get; set; }
        public string ReminderName { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }
        public string NotifierUserId { get; set; }
        public string WorkflowContractStepId { get; set; }
        public string WorkflowContractStepName { get; set; }
        public string SharedElementId { get; set; }
        public string SharedElementElementId { get; set; }
        public SharedElementInfos SharedElementInfos { get; set; }
        public int? QuestionId { get; set; }
        public string QuestionName { get; set; }
    }

    public class SharedElementInfos
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Project { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }
}