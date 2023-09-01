CREATE TABLE [dbo].[VaultContracts] (
    [Id]           NVARCHAR (450) NOT NULL,
    [VaultId]     NVARCHAR (450) NOT NULL,
    [ContractId]   NVARCHAR (128) NOT NULL,
    [ContractType] INT            NOT NULL,
    [Signed]       BIT            NULL,
    [BillNumber]   INT            NULL,
    CONSTRAINT [PK_VaultContracts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_VaultContracts_Vaults] FOREIGN KEY ([VaultId]) REFERENCES [dbo].[Vaults] ([Id])
);

