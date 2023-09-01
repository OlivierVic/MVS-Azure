CREATE TABLE [dbo].[VaultAnswersAnticipationMeasures] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Question] NVARCHAR(450)            NOT NULL,
    [VaultId]   NVARCHAR (450) NOT NULL,
    [Data]       NVARCHAR (MAX) NULL,
    [Comment]    NVARCHAR (MAX) NULL,
    [Job]        INT            NULL,
    [Selected]   BIT            NULL,
    CONSTRAINT [PK_VaultAnswersAnticipationMeasures] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_VaultAnswersAnticipationMeasures_Vaults] FOREIGN KEY ([VaultId]) REFERENCES [dbo].[Vaults] ([Id]),
    CONSTRAINT [FK_VaultAnswersAnticipationMeasures_JobParticular] FOREIGN KEY ([Job]) REFERENCES [dbo].[JobParticular] ([Id])
);

