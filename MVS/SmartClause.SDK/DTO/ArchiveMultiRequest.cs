using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartclause.SDK.DTO
{
    public class ArchiveMultiRequest
    {
        public List<string> FileIds { get; set; }
        public List<string> ContractIds { get; set; }
        public List<string> FolderIds { get; set; }
    }
}
