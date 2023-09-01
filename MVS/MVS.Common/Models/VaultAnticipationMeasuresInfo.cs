using System;
using System.Collections.Generic;

namespace MVS.Common.Models
{
    public partial class VaultAnticipationMeasuresInfo
    {
        public string Id { get; set; }
        public string VaultId { get; set; }
        public bool? AlreadyDrawnUpFutureProtectionMandate { get; set; }
        public string DraftedBy { get; set; }
        public string NecessaryTo { get; set; }
        public string MandateDetails { get; set; }
        public bool? LongTermCareInsurance { get; set; }
        public bool? AnticipatedGuidelines { get; set; }
        public bool? RefusesOrganDonation { get; set; }
        public bool? RefusesOrganDonationProcess { get; set; }
        public bool? ArrangedFuneral { get; set; }
        public bool? BuyFuneralConcession { get; set; }
        public bool? SignedFuneralAgreement { get; set; }
        public bool? GivenBankPower { get; set; }
        public bool? MakesDonationSharing { get; set; }
        public bool? MadeWill { get; set; }
        public bool? CanShareInfos { get; set; }
        public bool CompletedForm { get; set; }

        public virtual Vault Vault { get; set; }
    }
}
