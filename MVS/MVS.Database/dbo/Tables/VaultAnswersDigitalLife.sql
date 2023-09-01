CREATE TABLE [dbo].[VaultAnswersDigitalLife] (
    [Id]         INT             IDENTITY (1, 1) NOT NULL,
    [VaultId] NVARCHAR(450)             NOT NULL,
    [ReseauSocial]   NVARCHAR (10)  NULL,
    [OtherReseauSocial]       NVARCHAR (450) NULL,
    [ProfileUrl]    NVARCHAR (2048) NULL,
    [IdentifiantProfile]        NVARCHAR(450)             NULL,
    [Legataire]   BIT             NULL,
    [LegataireFirstLastName] NVARCHAR(450) NULL, 
    [ProfileUrlLegataire] NVARCHAR(2048) NULL, 
    CONSTRAINT [PK_VaultAnswersDigitalLife] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_VaultAnswersDigitalLife_Vaults] FOREIGN KEY ([VaultId]) REFERENCES [dbo].[Vaults] ([Id])
);
