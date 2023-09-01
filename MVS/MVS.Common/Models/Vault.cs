using System;
using System.Collections.Generic;

namespace MVS.Common.Models
{
    public partial class Vault : AccessElem
    {
        public Vault()
        {
            VaultAdministrativeLives = new HashSet<VaultAdministrativeLife>();
            VaultAnswersAnticipationMeasures = new HashSet<VaultAnswersAnticipationMeasure>();
            VaultAnswersDigitalLives = new HashSet<VaultAnswersDigitalLife>();
            VaultAnswersHeritages = new HashSet<VaultAnswersHeritage>();
            VaultAnswersPersonals = new HashSet<VaultAnswersPersonal>();
            VaultAnticipationMeasuresInfos = new HashSet<VaultAnticipationMeasuresInfo>();
            VaultContacts = new HashSet<VaultContact>();
            VaultContracts = new HashSet<VaultContract>();
            VaultDigitalLives = new HashSet<VaultDigitalLife>();
            VaultDocuments = new HashSet<VaultDocument>();
            VaultFamilyInfos = new HashSet<VaultFamilyInfo>();
            VaultFuneraryVolontes = new HashSet<VaultFuneraryVolonte>();
            VaultHeritages = new HashSet<VaultHeritage>();
            VaultPersonalInfos = new HashSet<VaultPersonalInfo>();
            VaultTiersContacts = new HashSet<VaultTiersContact>();
            VaultUsers = new HashSet<VaultUser>();
        }

        public string Id { get; set; }
        public string Title { get; set; }
        public string ContactId { get; set; }
        public int? Sex { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthLocation { get; set; }
        public string Address { get; set; }
        public string ZipceCodeAndCity { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }
        public bool? IsArchived { get; set; }
        public DateTime CreationDate { get; set; }
        public bool? IsDeleteAdmin { get; set; }
        public bool? DocumentDownload { get; set; }
        public string BirthName { get; set; }
        public bool? HaveBirthName { get; set; }
        public string Nationality { get; set; }
        public bool? AcceptedCondition { get; set; }

        public virtual AspNetUser User { get; set; }
        public virtual ICollection<VaultAdministrativeLife> VaultAdministrativeLives { get; set; }
        public virtual ICollection<VaultAnswersAnticipationMeasure> VaultAnswersAnticipationMeasures { get; set; }
        public virtual ICollection<VaultAnswersDigitalLife> VaultAnswersDigitalLives { get; set; }
        public virtual ICollection<VaultAnswersHeritage> VaultAnswersHeritages { get; set; }
        public virtual ICollection<VaultAnswersPersonal> VaultAnswersPersonals { get; set; }
        public virtual ICollection<VaultAnticipationMeasuresInfo> VaultAnticipationMeasuresInfos { get; set; }
        public virtual ICollection<VaultContact> VaultContacts { get; set; }
        public virtual ICollection<VaultContract> VaultContracts { get; set; }
        public virtual ICollection<VaultDigitalLife> VaultDigitalLives { get; set; }
        public virtual ICollection<VaultDocument> VaultDocuments { get; set; }
        public virtual ICollection<VaultFamilyInfo> VaultFamilyInfos { get; set; }
        public virtual ICollection<VaultFuneraryVolonte> VaultFuneraryVolontes { get; set; }
        public virtual ICollection<VaultHeritage> VaultHeritages { get; set; }
        public virtual ICollection<VaultPersonalInfo> VaultPersonalInfos { get; set; }
        public virtual ICollection<VaultTiersContact> VaultTiersContacts { get; set; }
        public virtual ICollection<VaultUser> VaultUsers { get; set; }
    }
}
