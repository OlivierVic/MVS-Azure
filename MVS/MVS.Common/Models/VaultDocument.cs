using System;
using System.Collections.Generic;

namespace MVS.Common.Models
{
    public partial class VaultDocument
    {
        public string Id { get; set; }
        public string VaultId { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public string TypeName { get; set; }
        public int Type { get; set; }
        public string Url { get; set; }
        public string ContactId { get; set; }
        public string ContractId { get; set; }

        public virtual VaultContact Contact { get; set; }
        public virtual Vault Vault { get; set; }
    }
}
