CREATE TABLE [dbo].[VaultDigitalLife] (
    [Id]                               NVARCHAR (450) NOT NULL,
    [VaultId]                         NVARCHAR (450) NOT NULL,
    [ReseauSocial]   NVARCHAR (10)  NULL,
    [OtherReseauSocial]       NVARCHAR (450) NULL,
    [ProfileUrl]    NVARCHAR (2048) NULL,
    [IdentifiantProfile]        NVARCHAR(450)             NULL,
    [Legataire]   BIT             NULL,
    [LegataireFirstLastName] NVARCHAR(450) NULL, 
    [ProfileUrlLegataire] NVARCHAR(2048) NULL, 
    [CompletedForm]                    BIT            NOT NULL,
    CONSTRAINT [PK_VaultDigitalLife] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_VaultDigitalLife_Vaults] FOREIGN KEY ([VaultId]) REFERENCES [dbo].[Vaults] ([Id])
);

