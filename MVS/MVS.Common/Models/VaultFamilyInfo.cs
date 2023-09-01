using System;
using System.Collections.Generic;

namespace MVS.Common.Models
{
    public partial class VaultFamilyInfo
    {
        public string Id { get; set; }
        public string VaultId { get; set; }
        public string FamilialSituation { get; set; }
        public string CoupleSituation { get; set; }
        public string MatrimonialSituation { get; set; }
        public bool? LivingDonation { get; set; }
        public bool? Children { get; set; }
        public int? NbChildren { get; set; }
        public bool? BlendedFamily { get; set; }
        public string FamilialSituationDetails { get; set; }
        public string FamilyRelationships { get; set; }
        public string FamilyRelationshipsDetails { get; set; }
        public bool? Meditation { get; set; }
        public bool? CanShareInfos { get; set; }
        public bool CompletedForm { get; set; }

        public virtual Vault Vault { get; set; }
    }
}
