// unset

using Smartclause.SDK.DTO;
using System.Collections.Generic;

namespace SmartClause.SDK.DTO
{
    public class QuerySearchResult : SearchResult
    {
        public List<FolderDto> Folders { get; set; }
    }
}