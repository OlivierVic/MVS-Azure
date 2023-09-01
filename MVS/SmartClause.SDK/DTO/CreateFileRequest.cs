namespace SmartClause.SDK.DTO
{
    public class CreateFileRequest
    {
        public class FileRequest
        {
            public string Name { get; set; }
            public string Contractor { get; set; }
            public string Country { get; set; }
            public string Language { get; set; }
            public string Project { get; set; }
            public string TemplateId { get; set; }
            public string TenantId { get; set; }
        }

        public FileRequest File { get; set; }
        public string VaultId { get; set; }
    }
}