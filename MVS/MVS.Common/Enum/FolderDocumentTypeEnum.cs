// <copyright file="FolderFieldEnum.cs" company="Seraphin.Legal">
// Copyright (c) Seraphin.Legal. All rights reserved.
// </copyright>

namespace MVS.Common.Enum;

public enum FolderDocumentTypeEnum
{

    //Création dossier
    RequesterIdCard = 4,
    ProtectorIdCard = 5,

    //Capacités
    MedicalCertificateNonReturnHome = 6,

    //Info perso
    IdCard = 0,
    ConcernedFamilyRecordBook = 1,
    AccountStatements = 2,
    IncomeTaxNotice = 3,
    Cv = 41,
    JudgeDecision = 7,
    ElectricityBill = 42,
    PhoneBill = 43,
    InternetBill = 44,
    MultiriskHomeInsurance = 45,
    HousingTax = 46,
    ContractAccommodationRetirementHome = 8,
    PropertyTaxNotice = 9,
    ResidentialLease = 10,
    GeneralAssemblyReport = 47,
    LastLoadCalls = 48,
    LitigationDocument = 49,

    //Info famille
    DeedGiftToLastLivingPerson = 50,
    MarriageCertificate = 16,
    CohabitationCertificate = 18,
    PacsContract = 17,
    FamilyRecordBook = 19,
    MarriageContractExtract = 74,
    DivorceJudgment = 15,
    SpouseIdCard = 11,
    SpouseBirthCertificate = 13,
    SpouseDeathCertificate = 14,
    SpouseFamilyRecordBook = 12,

    //Habitudes
    AnimalPassport = 51,
    WorkContract = 22,
    LastWorkReport = 23,
    HumanServicesContract = 52,
    LastHumanServicesInvoice = 53,
    HomeHelpServiceContract = 20,
    HomeHelpServiceBill = 21,

    //Anticipation
    InsuranceContractDependency = 55,
    InsuranceContract = 30,
    AdvanceDirectives = 54,
    FuneralOrganizationDocument = 72,
    FuneralConcessionContract = 56,
    FuneralAgreement = 57,
    DeedOfGiftSharing = 58,
    FutureProtectionMandate = 24,
    BankPowerAttorney = 25,

    //Santé
    DisabilityMobilityCard = 27,
    SupportingDocumentHealth = 73,
    TrustworthyPersonDesignation = 26,

    //Patrimoine
    RIB = 29,
    InsuranceContractLife = 75,
    KbisExtract = 31,
    AssociationArticlesOrShareholderAccount = 32,
    OwnershipTitleMainResidence = 28,
    OwnershipTitleSecondaryResidence = 76,
    OwnershipTitleOtherProperty = 77,
    SupportingDocumentHeritage = 78,
    VehicleRegistrationDocument = 33,

    //Budget
    CertificateOfMutualInsurance = 59,
    CertificateOfInsurance = 80,
    LastPaySlip = 35,
    APAAssistancePlanAccepted = 36,
    SupportingDocumentBudget = 60,
    LatestPensionStatements = 34,

    //VaultContact
    ProtectionJudgment = 37,
    ProtectorsIdCardProposedFutureAgent = 38,
    CriminalRecordOfProposedProtectorFutureAgent = 39,
    TaxNoticeOfProposedProtectorFutureAgent = 40,
    ProtectorsIdCardProposedController = 81,
    CriminalRecordOfProposedProtectorController = 82,
    TaxNoticeOfProposedProtectorController = 83,
    ProtectorsIdCardProposedObserver = 65,
    CriminalRecordOfProposedProtectorObserver = 66,
    TaxNoticeOfProposedProtectorFObserver = 67,
    ProtectorsIdCardProposedDesignated = 68,
    CriminalRecordOfProposedProtectorDesignated = 69,
    TaxNoticeOfProposedProtectorDesignated = 70,
    CopyIdentificationDocument = 71,
    ContactAdvice = 65,

    //Général
    SupportingDocument = 79,
    Other = 61,
    MandatSigned = 62,
    MandatProof = 63,
    MandatRecording = 64,

    //Temporaire
    VolontesTMP = 150,
}
