using System;

namespace Smartclause.SDK.DTO
{
    public class FolderDto
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
        public bool? Disabled { get; set; }
        public DateTime? LastAccessDate { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool? Frozen { get; set; }
        public bool? FrozenContent { get; set; }
        public bool? Confidential { get; set; }
        public bool? Archived { get; set; }
        public DateTime? ArchivedTime { get; set; }
        public Nullable<bool> Access { get; set; }
        public Nullable<DateTime> AccessRequestDate { get; set; }
        public Nullable<bool> CanShared { get; set; }
        public string OwnerName { get; set; }
    }
}
