CREATE TABLE [dbo].[VaultFuneraryVolonte] (
    [Id]                           NVARCHAR (450) NOT NULL,
    [VaultId]                      NVARCHAR (450) NOT NULL,
    [CompletedForm]                BIT            NOT NULL,
    [FuneralType]                  NVARCHAR (128) NULL,
    [Cimetery]                     NVARCHAR (128) NULL,
    [Crematorium]                  NVARCHAR (MAX) NULL,
    [City]                         NVARCHAR (128) NULL,
    [AshDestination]               NVARCHAR (50)  NULL,
    [FamilialBurialCity]           NVARCHAR (128) NULL,
    [BurialPlotNumber]             NVARCHAR (128) NULL,
    [BurialPlotExpiry]             DATETIME2 (7)  NULL,
    [FuneralConcessionaire]        NVARCHAR (128) NULL,
    [FuneralConcessionaireDetails] NVARCHAR (MAX) NULL,
    [BurialChoice]                 NVARCHAR (50)  NULL,
    [Details]                      NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_VaultFuneraryVolonte] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_VaultFuneraryVolonte_Vaults] FOREIGN KEY ([VaultId]) REFERENCES [dbo].[Vaults] ([Id])
);



