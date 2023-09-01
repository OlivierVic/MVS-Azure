using System;
using System.Collections.Generic;

namespace MVS.Common.Models
{
    public partial class VaultContract
    {
        public string Id { get; set; }
        public string VaultId { get; set; }
        public string ContractId { get; set; }
        public int ContractType { get; set; }
        public bool? Signed { get; set; }
        public int? BillNumber { get; set; }

        public virtual Vault Vault { get; set; }
    }
}
