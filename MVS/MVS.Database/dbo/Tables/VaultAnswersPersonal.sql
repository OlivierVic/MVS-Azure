CREATE TABLE [dbo].[VaultAnswersPersonal] (
    [Id]         INT             IDENTITY (1, 1) NOT NULL,
    [Question] NVARCHAR(450)             NOT NULL,
    [VaultId]   NVARCHAR (450)  NOT NULL,
    [Data]       NVARCHAR (2048) NULL,
    [Comment]    NVARCHAR (2048) NULL,
    [Job]        INT             NULL,
    [Selected]   BIT             NULL,
    CONSTRAINT [PK_VaultAnswersPersonal] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_VaultAnswersPersonale_Vaults] FOREIGN KEY ([VaultId]) REFERENCES [dbo].[Vaults] ([Id]),
    CONSTRAINT [FK_VaultAnswersPersonal_JobParticular] FOREIGN KEY ([Job]) REFERENCES [dbo].[JobParticular] ([Id])
);
