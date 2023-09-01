CREATE TABLE [dbo].[VaultAnticipationMeasuresInfos] (
    [Id]                                    NVARCHAR (450) NOT NULL,
    [VaultId]                              NVARCHAR (450) NOT NULL,
    [AlreadyDrawnUpFutureProtectionMandate] BIT            NULL,
    [DraftedBy]                             NVARCHAR (128) NULL,
    [NecessaryTo]                           NVARCHAR (128) NULL,
    [MandateDetails]                        NVARCHAR (MAX) NULL,
    [LongTermCareInsurance]                 BIT            NULL,
    [AnticipatedGuidelines]                 BIT            NULL,
    [RefusesOrganDonation]                  BIT            NULL,
    [RefusesOrganDonationProcess]           BIT            NULL,
    [ArrangedFuneral]                       BIT            NULL,
    [BuyFuneralConcession]                  BIT            NULL,
    [SignedFuneralAgreement]                BIT            NULL,
    [GivenBankPower]                        BIT            NULL,
    [MakesDonationSharing]                  BIT            NULL,
    [MadeWill]                              BIT            NULL,
    [CanShareInfos]                         BIT            NULL,
    [CompletedForm]                         BIT            NOT NULL,
    CONSTRAINT [PK_VaultAnticipationMeasuresInfos] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_VaultAnticipationMeasuresInfos_Vaults] FOREIGN KEY ([VaultId]) REFERENCES [dbo].[Vaults] ([Id])
);

