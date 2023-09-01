CREATE TABLE [dbo].[VaultAdministrativeLife] (
    [Id]                               NVARCHAR (450) NOT NULL,
    [VaultId]                         NVARCHAR (450) NOT NULL,
    [CompletedForm]                    BIT            NOT NULL,
    CONSTRAINT [PK_VaultAdministrativeLife] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_VaultAdministrativeLife_Vaults] FOREIGN KEY ([VaultId]) REFERENCES [dbo].[Vaults] ([Id])
);

