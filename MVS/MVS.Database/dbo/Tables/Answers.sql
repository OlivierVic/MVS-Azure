CREATE TABLE [dbo].[Answers] (
    [Id]         INT             IDENTITY (1, 1) NOT NULL,
    [Question] NVARCHAR(450)             NOT NULL,
    [VaultId]   NVARCHAR (450)  NOT NULL,
    [Data]       NVARCHAR (2048) NULL,
    [Comment]    NVARCHAR (2048) NULL,
    [Job]        INT             NULL,
    [Selected]   BIT             NULL,
    CONSTRAINT [PK_Answers] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Answers_JobParticular] FOREIGN KEY ([Job]) REFERENCES [dbo].[JobParticular] ([Id])
);




GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Champ libre pour compléments', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Answers', @level2type = N'COLUMN', @level2name = N'Comment';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id bénéficiaire/dossier pour lequel répond', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Answers', @level2type = N'COLUMN', @level2name = 'VaultId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Id user répond à la question', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Answers', @level2type = N'COLUMN', @level2name = 'Question';

