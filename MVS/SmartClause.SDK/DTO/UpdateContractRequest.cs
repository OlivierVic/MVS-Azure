namespace SmartClause.SDK.DTO
{
    public class UpdateContractMetadataRequest
    {
        public string Language { get; set; }
        public string Project { get; set; }
        public string Country { get; set; }
        public string EnveloppeId { get; set; }
    }

    public class UpdateContractSignInfoRequest
    {
        public string EnvelopeId { get; set; }
        public bool SentForSignature { get; set; }
        public bool Sign { get; set; }
    }
}