using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SmartClause.SDK.DTO
{
    public class ContractAnnexDto
    {
        public string Id { get; set; }
        public string ContractId { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public int Order { get; set; }
    }

    public class AddContractAnnexesRequest
    {
        public byte[] FileContent { get; set; }
        public string ContractId { get; set; }
        public string Name { get; set; }
    }
}
