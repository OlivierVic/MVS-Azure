namespace SmartClause.SDK.DTO.Notification
{
    public class CollaborationInvitationResponse
    {
        public enum RightsEnum
        {
            Form = 0,
            Revisions = 1,
            Comments = 2,
            Signing = 3,
            FormAndRevisions = 4,
        }

        public string UserEmail { get; set; }
        public string InvitationId { get; set; }
        public bool IsInPreviousSteps { get; set; }
        public RightsEnum Rights { get; set; }
    }
}