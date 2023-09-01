using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartclause.SDK.DTO
{
    public class ContractMultipleFiles
    {
        public string Name { get; set; }
        public Stream Content { get; set; }
    }
}
