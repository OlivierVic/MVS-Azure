using System;
using System.Collections.Generic;

namespace MVS.Common.Models
{
    public partial class VaultPersonalInfo
    {
        public string Id { get; set; }
        public string VaultId { get; set; }
        public bool? FrenchMotherTongue { get; set; }
        public string OtherLanguage { get; set; }
        public string UnderstandFrench { get; set; }
        public string SpeakFrench { get; set; }
        public string ProfessionalSituation { get; set; }
        public string LevelOfStudy { get; set; }
        public string LevelOfStudyDetails { get; set; }
        public string Job { get; set; }
        public string JobDetails { get; set; }
        public bool? Cv { get; set; }
        public string ProtectiveSupervision { get; set; }
        public string LivingEnvironment { get; set; }
        public string TypeOfHousing { get; set; }
        public string HousingLaw { get; set; }
        public bool? CoOwnership { get; set; }
        public bool? OngoingLitigation { get; set; }
        public int? Nblitigations { get; set; }
        public int? NbLawyerFirms { get; set; }
        public bool? CanShareInfos { get; set; }
        public bool CompletedForm { get; set; }
        public string PrecisionHousing { get; set; }
        public string PrecisionLitigation { get; set; }

        public virtual Vault Vault { get; set; }
    }
}
