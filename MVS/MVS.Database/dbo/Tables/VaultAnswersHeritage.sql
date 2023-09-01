CREATE TABLE [dbo].[VaultAnswersHeritage] (
    [Id]         INT             IDENTITY (1, 1) NOT NULL,
    [Question] NVARCHAR(450)             NOT NULL,
    [VaultId]   NVARCHAR (450)  NOT NULL,
    [Data]       NVARCHAR (2048) NULL,
    [Comment]    NVARCHAR (2048) NULL,
    [Job]        INT             NULL,
    [Selected]   BIT             NULL,
    CONSTRAINT [PK_VaultAnswersHeritage] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_VaultAnswersHeritage_Vaults] FOREIGN KEY ([VaultId]) REFERENCES [dbo].[Vaults] ([Id]),
    CONSTRAINT [FK_VaultAnswersHeritage_JobParticular] FOREIGN KEY ([Job]) REFERENCES [dbo].[JobParticular] ([Id])
);
