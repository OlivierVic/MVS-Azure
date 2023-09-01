using System;

namespace Smartclause.SDK.DTO
{
    public class FileDto
    {
        public string Id { get; set; }
        public string Country { get; set; }
        public string Contractor { get; set; }
        public string Language { get; set; }
        public string Name { get; set; }
        public string Project { get; set; }
        public string TemplateId { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime LastAccessDateTime { get; set; }
        public Nullable<DateTime> ArchivedTime { get; set; }
        public Nullable<bool> Archived { get; set; }
        public Nullable<bool> Disabled { get; set; }
        public bool? Sign { get; set; }
        public DateTime? SignedTime { get; set; }

        public int? Type { get; set; }
        public Nullable<bool> Access { get; set; }
        public Nullable<DateTime> AccessRequestDate { get; set; }
        public Nullable<bool> CanShared { get; set; }
        public string UserId { get; set; }
    }
}