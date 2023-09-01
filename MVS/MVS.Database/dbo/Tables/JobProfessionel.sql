CREATE TABLE [dbo].[JobProfessionel] (
    [Id]  INT            IDENTITY (1, 1) NOT NULL,
    [Job] NVARCHAR (MAX) NOT NULL,
    CONSTRAINT [PK_JobProfessionel] PRIMARY KEY CLUSTERED ([Id] ASC)
);

