using System;
using System.Collections.Generic;

namespace MVS.Common.Models
{
    public partial class VaultAnswersPersonal
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string VaultId { get; set; }
        public string Data { get; set; }
        public string Comment { get; set; }
        public int? Job { get; set; }
        public bool? Selected { get; set; }

        public virtual JobParticular JobNavigation { get; set; }
        public virtual Vault Vault { get; set; }
    }
}
