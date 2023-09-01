namespace SmartClause.SDK.DTO.Contract
{
    public class AddContributorToWorkflowStepRequest
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FieldId { get; set; }
        public int Rights { get; set; }
        public string ContractId { get; set; }
        public int CurrentStep { get; set; }
        public string CreatorEndUserId { get; set; }
        public string CreatorEndUserEmail { get; set; }
    }
}