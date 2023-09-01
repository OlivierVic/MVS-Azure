using System.Collections.Generic;

namespace Smartclause.SDK.DTO
{
    public class FlattenHierarchyResponseDto
    {
        public List<ContractDto> Contracts { get; set; }
        public List<FileDto> Files { get; set; }
        public List<FolderDto> Folders { get; set; }
    }
}