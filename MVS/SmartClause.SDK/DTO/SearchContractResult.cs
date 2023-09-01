using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartclause.SDK.DTO
{
    public class SearchContractResult
    {
        public List<FolderDto> ContractFolders { get; set; }
        public List<ContractDto> Contracts { get; set; }
    }
}
