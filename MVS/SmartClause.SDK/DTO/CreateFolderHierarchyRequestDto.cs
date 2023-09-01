using System;
using System.Collections.Generic;

namespace Smartclause.SDK.DTO
{
    public class CreateFolderHierarchyRequestDto
    {
        public class FolderDto
        {
            public string Name { get; set; }
            public bool Confidential { get; set; }
            public bool Frozen { get; set; }
            public bool FrozenContent { get; set; }
            public List<FolderDto> Children { get; set; }
            public Nullable<bool> Access { get; set; }
            public Nullable<DateTime> AccessRequestDate { get; set; }
        }

        public List<FolderDto> Folders { get; set; }
    }
}