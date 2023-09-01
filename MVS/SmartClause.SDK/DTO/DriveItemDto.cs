using System;

namespace SmartClause.SDK.DTO
{
    public class DriveItemDTO
    {
        public string Type { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public Nullable<bool> Frozen { get; set; }
        public Nullable<bool> FrozenContent { get; set; }
        public Nullable<bool> Confidential { get; set; }
        public string TemplateId { get; set; }
        public string Owner { get; set; }
        public string OwnerName { get; set; }
        public Nullable<DateTime> CreationDate { get; set; }
        public Nullable<DateTime> LastAccessDate { get; set; }
        public bool Deleted { get; set; }
        public Nullable<DateTime> DeletionDate { get; set; }
        public Nullable<DateTime> SignedDate { get; set; }
        public string Country { get; set; }
        public string FinancialAmount { get; set; }
        public Nullable<int> TotalRows { get; set; }
    }
}
