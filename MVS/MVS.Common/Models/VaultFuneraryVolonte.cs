using System;
using System.Collections.Generic;

namespace MVS.Common.Models
{
    public partial class VaultFuneraryVolonte
    {
        public string Id { get; set; }
        public string VaultId { get; set; }
        public bool CompletedForm { get; set; }
        public string FuneralType { get; set; }
        public string Cimetery { get; set; }
        public string Crematorium { get; set; }
        public string City { get; set; }
        public string AshDestination { get; set; }
        public string FamilialBurialCity { get; set; }
        public string BurialPlotNumber { get; set; }
        public DateTime? BurialPlotExpiry { get; set; }
        public string FuneralConcessionaire { get; set; }
        public string FuneralConcessionaireDetails { get; set; }
        public string BurialChoice { get; set; }
        public string Details { get; set; }

        public virtual Vault Vault { get; set; }
    }
}
