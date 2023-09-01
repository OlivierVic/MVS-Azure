CREATE TABLE [dbo].[VaultFamilyInfos] (
    [Id]                         NVARCHAR (450) NOT NULL,
    [VaultId]                   NVARCHAR (450) NOT NULL,
    [FamilialSituation]          NVARCHAR (128) NULL,
    [CoupleSituation]            NVARCHAR (128) NULL,
    [MatrimonialSituation]       NVARCHAR (128) NULL,
    [LivingDonation]             BIT            NULL,
    [Children]                   BIT            NULL,
    [NbChildren]                 INT            NULL,
    [BlendedFamily]              BIT            NULL,
    [FamilialSituationDetails]   NVARCHAR (MAX) NULL,
    [FamilyRelationships]        NVARCHAR (128) NULL,
    [FamilyRelationshipsDetails] NVARCHAR (MAX) NULL,
    [Meditation]                 BIT            NULL,
    [CanShareInfos]              BIT            NULL,
    [CompletedForm]              BIT            NOT NULL,
    CONSTRAINT [PK_VaultFamilyInfos] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_VaultFamilyInfos_Vaults] FOREIGN KEY ([VaultId]) REFERENCES [dbo].[Vaults] ([Id])
);

