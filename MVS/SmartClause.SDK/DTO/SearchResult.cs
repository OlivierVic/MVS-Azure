using Smartclause.SDK.DTO;
using System.Collections.Generic;

namespace SmartClause.SDK.DTO
{
    public class SearchResult
    {
        public List<FileDto> Files { get; set; }
        public List<ContractDto> Contracts { get; set; }
        public int TotalCount { get; set; }
    }
}