namespace SmartClause.SDK.DTO
{
    public class CreateTenantResult
    {
        public TenantDTO Tenant { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }
    }

    public class CreateTenantResponse
    {
        public TenantDTO Tenant { get; set; }
        public string EncodedEmail { get; set; }
        public string EncodedPassword { get; set; }
    }

    public class TenantDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PrimaryAccountId { get; set; }
    }
}
