CREATE TABLE [zaposlime].[Job]
(
	[Id]                                UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [Title]                             NVARCHAR(100) NULL, 
    [NumberOfWorkers]                   INT NULL,
    [Description]                       NVARCHAR(MAX) NULL, 
    [EmployerId]                        NVARCHAR (450) NOT NULL, 
    [CreatedBy]                         UNIQUEIDENTIFIER NULL, 
    [CreatedAt]                         DATETIME NULL, 
    [UpdatedBy]                         UNIQUEIDENTIFIER NULL, 
    [UpdatedAt]                         DATETIME NULL, 
    CONSTRAINT FK_Job_User
        FOREIGN KEY ([EmployerId]) REFERENCES [identity].[AspNetUsers]([Id])
)
