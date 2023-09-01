using System;
using System.Collections.Generic;

namespace MVS.Common.Models
{
    public partial class VaultDigitalLife
    {
        public string Id { get; set; }
        public string VaultId { get; set; }
        public string ReseauSocial { get; set; }
        public string OtherReseauSocial { get; set; }
        public string ProfileUrl { get; set; }
        public string IdentifiantProfile { get; set; }
        public bool? Legataire { get; set; }
        public string LegataireFirstLastName { get; set; }
        public string ProfileUrlLegataire { get; set; }
        public bool CompletedForm { get; set; }

        public virtual Vault Vault { get; set; }
    }
}
