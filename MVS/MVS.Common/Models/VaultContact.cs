using System;
using System.Collections.Generic;

namespace MVS.Common.Models
{
    public partial class VaultContact
    {
        public VaultContact()
        {
            VaultDocuments = new HashSet<VaultDocument>();
        }

        public string Id { get; set; }
        public string VaultId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int? Sex { get; set; }
        public string Company { get; set; }
        public bool Ispro { get; set; }
        public string PhoneNumber { get; set; }
        public string ZipCodeAndCity { get; set; }
        public string Country { get; set; }
        public string Addres { get; set; }
        public int? Accompaniment { get; set; }
        public string Email { get; set; }
        public int? Job { get; set; }
        public string OtherJob { get; set; }
        public bool? IsFolderAdmin { get; set; }
        public bool? IsSetJudge { get; set; }
        public int? TypeMission { get; set; }
        public bool? OpinionPro { get; set; }
        public bool? Confidence { get; set; }
        public string MoreInfo { get; set; }
        public int? Kinship { get; set; }
        public int? HelpNeeded { get; set; }
        public string OtherFamilyTies { get; set; }
        public string Other { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PlaceOfBirth { get; set; }
        public string Nationality { get; set; }
        public bool? IsSetAskProtection { get; set; }
        public string FirstLastNameMother { get; set; }
        public bool? UnknownMother { get; set; }
        public string FirstLastNameFather { get; set; }
        public bool? UnknownFather { get; set; }
        public bool? AdoptedPerson { get; set; }
        public bool? CloseNotice { get; set; }
        public int? RelationshipQuality { get; set; }
        public int? RelationshipFrequencies { get; set; }
        public string ContactDetails { get; set; }
        public bool? IsFutuAgent { get; set; }
        public bool? InfoPro { get; set; }
        public int? AgentMission { get; set; }
        public int? AdviceStatus { get; set; }
        public bool CompletedForm { get; set; }
        public bool? IsProtector { get; set; }
        public string AdviceContractId { get; set; }
        public bool? RequestCopy { get; set; }
        public bool? IsDoctor { get; set; }
        public int? TypeContact { get; set; }
        public string PropertyDetails { get; set; }
        public bool? ProtectPeople { get; set; }
        public bool? ProtectAllProperty { get; set; }
        public bool? ProtectOfCertainGoods { get; set; }

        public virtual VaultCategory AccompanimentNavigation { get; set; }
        public virtual Vault Vault { get; set; }
        public virtual ICollection<VaultDocument> VaultDocuments { get; set; }
    }
}
