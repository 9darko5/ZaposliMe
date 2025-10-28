CREATE TABLE [zaposlime].[Job]
(
	[Id]                                UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [Title]                             NVARCHAR(100) NULL, 
    [NumberOfWorkers]                   INT NULL,
    [Description]                       NVARCHAR(MAX) NULL, 
    [EmployerId]                        NVARCHAR (450) NOT NULL,
    [CityId]                            UNIQUEIDENTIFIER NOT NULL, 
    [CreatedBy]                         NVARCHAR (450) NULL,
    [CreatedAt]                         DATETIME NULL, 
    [UpdatedBy]                         NVARCHAR (450) NULL,
    [UpdatedAt]                         DATETIME NULL, 
    CONSTRAINT [FK_Job_User] FOREIGN KEY ([EmployerId]) REFERENCES [identity].[AspNetUsers]([Id]),
    CONSTRAINT [FK_Job_City] FOREIGN KEY ([CityId]) REFERENCES [zaposlime].[City]([Id])
)
