CREATE TABLE [dbo].[JobParticular] (
    [Id]  INT           IDENTITY (1, 1) NOT NULL,
    [Job] NVARCHAR(MAX) NOT NULL,
    CONSTRAINT [PK_JobParticular] PRIMARY KEY CLUSTERED ([Id] ASC)
);

