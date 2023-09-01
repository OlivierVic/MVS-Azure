CREATE TABLE [dbo].[VaultCategory] (
    [Id]           INT          IDENTITY (1, 1) NOT NULL,
    [CategoryName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_VaultCategory] PRIMARY KEY CLUSTERED ([Id] ASC)
);

