using System;
using System.Collections.Generic;

namespace MVS.Common.Models
{
    public partial class VaultAdministrativeLife
    {
        public string Id { get; set; }
        public string VaultId { get; set; }
        public bool CompletedForm { get; set; }

        public virtual Vault Vault { get; set; }
    }
}
