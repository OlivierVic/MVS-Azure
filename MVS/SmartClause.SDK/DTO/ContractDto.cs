using System;
using System.Collections.Generic;

namespace Smartclause.SDK.DTO
{
    public class ContractDto
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public Nullable<System.DateTime> ModifiedTime { get; set; }
        public Nullable<System.DateTime> FirstValidTime { get; set; }
        public Nullable<bool> Validate { get; set; }
        public Nullable<bool> Sent { get; set; }
        public Nullable<bool> Filled { get; set; }
        public Nullable<bool> Sign { get; set; }
        public List<Party> Parties1 { get; set; }
        public List<Workflow> ContractWorkflows { get; set; }
        public List<WorkflowContractSteps> WorkflowContractSteps { get; set; }
        public string TemplateId { get; set; }
        public string Parties { get; set; }
        public string UserId { get; set; }
        public Nullable<bool> SentForSignature { get; set; }
        public Nullable<System.DateTime> SentForSignatureTime { get; set; }
        public string Nation { get; set; }
        public string Language { get; set; }
        public string Project { get; set; }
        public string Contractor { get; set; }
        public Nullable<DateTime> ArchivedTime { get; set; }
        public Nullable<bool> Archived { get; set; }
        public Nullable<bool> Disabled { get; set; }
        public string EnvelopeId { get; set; }
        public string ContractType { get; set; }
        public Nullable<bool> Access { get; set; }
        public Nullable<DateTime> AccessRequestDate { get; set; }
        public Nullable<bool> CanShared { get; set; }

        public bool FinalValidation { get; set; }
    }

    public class Party
    {
        public int Id { get; set; }
        public string PartyEmail { get; set; }
        public string PartyFirstName { get; set; }
        public string PartyLastName { get; set; }
        public string UID { get; set; }
        public string Color { get; set; }
        public bool? Sent { get; set; }
        public bool? Received { get; set; }
        public bool? HasValidated { get; set; }
        public bool? HasSigned { get; set; }
        public bool? HasAcceptedCGU { get; set; }
        public bool? HasAcceptedDPO { get; set; }
    }

    public class Workflow
    {
        public int Id { get; set; }
        public string IdContract { get; set; }
        public string IdUser { get; set; }
        public int Step { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Comment { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class WorkflowContractSteps
    {
        public string Id { get; set; }
        public string ContractId { get; set; }
        public string WorkflowTemplateId { get; set; }
        public string ConditionRootId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> BuiltinOption { get; set; }
        public string ParentId { get; set; }
        public int StepOrder { get; set; }
        public Nullable<bool> Disable { get; set; }
        public Nullable<System.DateTime> StartDateTime { get; set; }
        public Nullable<System.DateTime> EndDateTime { get; set; }
        public Nullable<bool> Optional { get; set; }
        public List<WorkflowContractSteps> Children { get; set; }
        public List<WorkflowContractSteps> Parent { get; set; }
        public List<WorkflowContractSteps> WorkflowContractStepsList { get; set; }
        public List<WorkflowContractStepUser> WorkflowContractStepUsers { get; set; }
    }

    public class WorkflowContractStepUser
    {
        public string Id { get; set; }
        public string WorkflowStepId { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Nullable<System.DateTime> ValidatedDateTime { get; set; }
        public Nullable<bool> Validated { get; set; }
        public int Rights { get; set; }
        public Nullable<bool> Optional { get; set; }
        public string FieldId { get; set; }
        public string CreatorEndUserId { get; set; }
        public List<Condition> Conditions { get; set; } = new List<Condition>();
        public ConditionNode ConditionNode { get; set; }
        public bool IsConditionValidated { get; set; }
    }

    public class ContractRights
    {
        // Define if a user can validate a document
        public bool Validate { get; set; }
        // Define if a user can modify a document
        public bool Modify { get; set; }
        // Define if a user can comment a document
        public bool Comment { get; set; }
        // Define if a user can access to a document
        public bool Read { get; set; }
        public bool AccessForm { get; set; }
        public bool AccessRevision { get; set; }
        public bool AccessVersions { get; set; }
        public bool AcceptAllMarkups { get; set; }
        public bool Save { get; set; }
        public bool AskSignatures { get; set; }
    }


    public class ChangeNameModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class ContractWithUserModel
    {
        public string Id { get; set; }
        public string UID { get; set; }
    }

    public class ValidateContractWorkflowStepUserModel
    {
        public string EndUserId { get; set; }

        public string ContractWorkflowStepId { get; set; }

        public string ContractWorkflowStepName { get; set; }

        public string WorkflowContractStepUserId { get; set; }

        public string ContractId { get; set; }

        public bool State { get; set; }

        public string WorkflowContractStepUserEmail { get; set; }
    }

    public class ValidateContractWorkflowStepModel
    {
        public string ContractWorkflowStepId { get; set; }
        public string EndUserEmail { get; set; }
    }


    public class CommentModel
    {
        public string RefId { get; set; }
        public string Comment { get; set; }
        public string IdContract { get; set; }
        public string IdUser { get; set; }
        public string UserName { get; set; }
        public int Type { get; set; }
        public DateTime CreateDate { get; set; }
    }

    public class ContactAnchorResponse
    {
        public string State { get; set; }
        public string Message { get; set; }
        public string Url { get; set; }
    }

    public class UserContact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string SCMContainerAnchorTag { get; set; }
        public string Title { get; set; }
    }

    public class RunContractModel
    {
        public string Id { get; set; }
        public string Uid { get; set; } = String.Empty;
        public string Lang { get; set; } = "fr";
        public bool ShouldShowForms { get; set; } = true;
        public bool ShouldShowRevisions { get; set; } = true;
        public bool ShouldShowVersions { get; set; } = true;
        public bool ShouldShowUserMenu { get; set; } = false;
        public bool ShouldUseRightsSystem { get; set; } = false;
        public bool isDraft { get; set; } = false;

        public int StartingTab { get; set; } = 1;
    }

    public class ConversationMessageDto
    {
        public int Id { get; set; }
        public int IdConversation { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public string AuthorUID { get; set; }
        public DateTime CreateDate { get; set; }
        public string Content { get; set; }
    }
}
