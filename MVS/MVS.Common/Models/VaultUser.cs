using System;
using System.Collections.Generic;

namespace MVS.Common.Models
{
    public partial class VaultUser
    {
        public string Id { get; set; }
        public string VaultId { get; set; }
        public string UserId { get; set; }

        public virtual AspNetUser User { get; set; }
        public virtual Vault Vault { get; set; }
    }
}
