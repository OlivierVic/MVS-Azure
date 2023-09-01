using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartclause.SDK.DTO
{
    public class SignContractResult
    {
        public string EnvelopeId { get; set; }
        public string SignatureUrl { get; set; }
        public string FileUrl { get; set; }
        public string Provider { get; set; }
    }
}
