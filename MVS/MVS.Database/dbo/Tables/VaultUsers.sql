CREATE TABLE [dbo].[VaultUsers] (
    [Id]       NVARCHAR (450) NOT NULL,
    [VaultId] NVARCHAR (450) NOT NULL,
    [UserId]   NVARCHAR (450) NOT NULL,
    CONSTRAINT [PK_VaultUsers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_VaultUsers_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_VaultUsers_Vaults] FOREIGN KEY ([VaultId]) REFERENCES [dbo].[Vaults] ([Id])
);

