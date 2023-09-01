CREATE TABLE [dbo].[Vaults] (
    [Id]                           NVARCHAR (450) NOT NULL,
    [Title]                        NVARCHAR (MAX) NOT NULL,
    [ContactId]                    NVARCHAR (MAX) NULL,
    [Sex]                          INT            NULL,
    [FirstName]                    NVARCHAR (MAX) NULL,
    [LastName]                     NVARCHAR (MAX) NULL,
    [BirthDate]                    DATETIME2 (7)  NULL,
    [BirthLocation]                NVARCHAR (MAX) NULL,
    [Address]                      NVARCHAR (MAX) NULL,
    [ZipceCodeAndCity]             NVARCHAR (MAX) NULL,
    [Country]                      NVARCHAR (MAX) NULL,
    [Email]                        NVARCHAR (MAX) NULL,
    [PhoneNumber]                  NVARCHAR (MAX) NULL,
    [UserId]                       NVARCHAR (450) NOT NULL,
    [isArchived]                   BIT            NULL,
    [CreationDate]                 DATETIME2 (7)  NOT NULL,
    [IsDeleteAdmin]                BIT            NULL,
    [DocumentDownload]             BIT            NULL,
    [BirthName]                    NVARCHAR (450) NULL,
    [HaveBirthName]                BIT            NULL,
    [Nationality] NVARCHAR(450) NULL, 
    [AcceptedCondition] BIT NULL, 
    CONSTRAINT [PK_Vaults] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Vaults_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);

