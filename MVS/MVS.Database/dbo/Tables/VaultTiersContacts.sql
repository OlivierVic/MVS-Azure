CREATE TABLE [dbo].[VaultTiersContacts] (
    [Id]                      NVARCHAR (450) NOT NULL,
    [VaultId]                NVARCHAR (450) NULL,
    [LastName]                NVARCHAR (MAX) NULL,
    [FirstName]               NVARCHAR (MAX) NULL,
    [Sex]                     INT            NULL,
    [Company]                 NVARCHAR (MAX) NULL,
    [Ispro]                   BIT            NOT NULL,
    [PhoneNumber]             NVARCHAR (MAX) NULL,
    [ZipCodeAndCity]          NVARCHAR (MAX) NULL,
    [Country]                 NVARCHAR (MAX) NULL,
    [Addres]                  NVARCHAR (MAX) NULL,
    [Accompaniment]           INT            NULL,
    [Email]                   NVARCHAR (MAX) NULL,
    [Job]                     INT            NULL,
    [OtherJob]                NVARCHAR (MAX) NULL,
    [IsFolderAdmin]           BIT            NULL,
    [IsSetJudge]              BIT            NULL,
    [TypeMission]             INT            NULL,
    [OpinionPro]              BIT            NULL,
    [Confidence]              BIT            NULL,
    [MoreInfo]                NVARCHAR (MAX) NULL,
    [Kinship]                 INT            NULL,
    [HelpNeeded]              INT            NULL,
    [OtherFamilyTies]         NVARCHAR (MAX) NULL,
    [Other]                   NVARCHAR (MAX) NULL,
    [DateOfBirth]             DATETIME       NULL,
    [PlaceOfBirth]            NVARCHAR (MAX) NULL,
    [Nationality]             NVARCHAR (MAX) NULL,
    [IsSetAskProtection]      BIT            NULL,
    [FirstLastNameMother]     NVARCHAR (MAX) NULL,
    [UnknownMother]           BIT            NULL,
    [FirstLastNameFather]     NVARCHAR (MAX) NULL,
    [UnknownFather]           BIT            NULL,
    [AdoptedPerson]           BIT            NULL,
    [CloseNotice]             BIT            NULL,
    [RelationshipQuality]     INT            NULL,
    [RelationshipFrequencies] INT            NULL,
    [ContactDetails]          NVARCHAR (MAX) NULL,
    [IsFutuAgent]             BIT            NULL,
    [InfoPro]                 BIT            NULL,
    [AgentMission]            INT            NULL,
    [AdviceStatus]            INT            NULL,
    [CompletedForm]           BIT            NOT NULL,
    [IsProtector]             BIT            NULL,
    [AdviceContractId]        NVARCHAR (128) NULL,
    [RequestCopy]             BIT            NULL,
    [IsDoctor]                BIT            NULL,
    [TypeContact]             INT            NULL,
    [PropertyDetails]         NVARCHAR (MAX) NULL,
    [ProtectPeople] BIT NULL, 
    [ProtectAllProperty] BIT NULL, 
    [ProtectOfCertainGoods] BIT NULL, 
    CONSTRAINT [PK_VaultTiersContacts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_VaultTiersContacts_VaultCategory] FOREIGN KEY ([Accompaniment]) REFERENCES [dbo].[VaultCategory] ([Id]),
    CONSTRAINT [FK_VaultTiersContacts_Vaults] FOREIGN KEY ([VaultId]) REFERENCES [dbo].[Vaults] ([Id])
);
