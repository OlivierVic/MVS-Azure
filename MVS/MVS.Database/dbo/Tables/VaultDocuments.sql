CREATE TABLE [dbo].[VaultDocuments] (
    [Id]         NVARCHAR (450) NOT NULL,
    [VaultId]   NVARCHAR (450) NOT NULL,
    [Name]       NVARCHAR (450) NULL,
    [FileName]   NVARCHAR (450) NULL,
    [TypeName]   NVARCHAR (450) NOT NULL,
    [Type]       INT            NOT NULL,
    [Url]        NVARCHAR (450) NULL,
    [ContactId]  NVARCHAR (450) NULL,
    [ContractId] NVARCHAR (128) NULL,
    CONSTRAINT [PK_VaultDocuments] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_VaultDocuments_VaultContacts] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[VaultContacts] ([Id]),
    CONSTRAINT [FK_VaultDocuments_Vaults] FOREIGN KEY ([VaultId]) REFERENCES [dbo].[Vaults] ([Id])
);

