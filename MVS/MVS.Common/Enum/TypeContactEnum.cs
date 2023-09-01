using System.ComponentModel;

namespace MVS.Common.Enum;
public enum TypeContactEnum
{
    [Description("Variable pour null")]
    Null = 0,
    [Description("Variable pour autre type")]
    Other = 1,

    // Habitues de vie
    [Description("Coiffeur")]
    Barber = 2,
    [Description("Prestataire esthétique")]
    Esthetic = 3,
    [Description("Prestataire de soin bien-être")]
    WellnessCareProvider = 4,
    [Description("Prestataire service à la personne")]
    ServiceToThePerson = 5,

    // Patrimoine
    [Description("Professionnel immobilier Principal")]
    MainResidence = 6,
    [Description("Professionnel immobilier Secondaire")]
    SecondResidence = 7,
    [Description("Professionnel immobilier autre")]
    OtherRealEstateField = 8,
    [Description("Banque Principal")]
    BankAccount = 9,
    [Description("Banque Autre")]
    OtherBankAccount = 10,
    [Description("Prestataire Assurance vie")]
    LifeInsurance = 11,
    [Description("Prestataire Autre assurance vie")]
    OtherLifeInsurance = 12,
    [Description("Prestataire actions")]
    UnitsOrSharesOfCompanies = 13,
    [Description("Prestataire actions")]
    OtherUnitsOrSharesOfCompanies = 14,
    [Description("Prestataire Droit Auteur")]
    Copyright = 15,
    [Description("Prestataire Droit Auteur")]
    OtherCopyright = 16,
    [Description("Prestataire PI")]
    IndustrialProperty = 17,
    [Description("Prestataire PI")]
    OtherIndustrialProperty = 18,
    [Description("Garage")]
    MotorVehicle = 19,
    [Description("Garage")]
    OtherMotorVehicle = 20,
    [Description("Prestataire bien mobilier")]
    ValuablePersonalProperty = 21,
    [Description("Prestataire bien mobilier")]
    OtherValuablePersonalProperty = 22,
    [Description("Prestataire portefeuille actions")]
    PortfolioOfShares = 23,
    [Description("Prestataire portefeuille actions")]
    OtherPortfolioOfShares = 24,
    [Description("Prestataire autre bien")]
    OtherProperty = 25,
    [Description("Créancier")]
    Borrowing = 26,
    [Description("Créancier")]
    OtherBorrowing = 27,

    // Santé
    [Description("Dentiste")]
    DentalMonitoring = 28,
    [Description("Médecin consultation mémoire")]
    MemoryConsultation = 29,
    [Description("Gynécologue")]
    GynecologicalCare = 30,
    [Description("Médecin dépistage cancer sein")]
    BreastCancerScreening = 31,
    [Description("Médecin dépistage cancer colorectal")]
    ColorectalCancerScreening = 32,
    [Description("Médecin dépistage cancer peau")]
    SkinCancerScreening = 33,
    [Description("Médecin autre suivi médical préventif")]
    AddPreventiveMedicalFollowUp = 34,
    [Description("Rhumatologue")]
    Osteoarthritis = 35,
    [Description("Diabétologue")]
    Diabetes = 36,
    [Description("Cardiologue")]
    HeartProblem = 37,
    [Description("Pneumologue")]
    RespiratoryDisease = 38,
    [Description("Psychologue")]
    Depression = 39,
    [Description("Cancérologue")]
    Cancer = 40,
    [Description("Médecin maladie neuro-dégénérative")]
    NeurodegenerativeDisease = 41,
    [Description("Urologue")]
    Incontinence = 42,
    [Description("Médecin ostéoporose")]
    Osteoporosis = 43,
    [Description("Gériatre")]
    RiskOfFalls = 44,
    [Description("ORL")]
    HearingProblem = 45,
    [Description("Ophtalmologue")]
    SightProblem = 46,
    [Description("Ophtalmologue")]
    View = 47,
    [Description("Médecin mobilité")]
    Mobility = 48,
    [Description("ORL")]
    Hearing = 49,
    [Description("Gériatre")]
    Cognitive = 50,
    [Description("Médecin autre handicap")]
    OtherTypeOfDisability = 51,
    [Description("Médecin autre maladie")]
    OtherDisease = 52,
}