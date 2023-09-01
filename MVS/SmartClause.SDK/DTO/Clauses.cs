using System;

namespace Smartclause.SDK.DTO
{
    public class Clauses
    {
        public string Id { get; set; }
        public string PreviousVersionId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Regulations { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string Comment { get; set; }
        public string VaultId { get; set; }
        public string AzureUrl { get; set; }
        public bool isLastVersion { get; set; }
        public bool isSmartClause { get; set; }
        public Nullable<bool> Archived { get; set; }
    }
}
