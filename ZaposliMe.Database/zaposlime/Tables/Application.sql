CREATE TABLE [zaposlime].[Application]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [EmployeeId] NVARCHAR (450) NOT NULL, 
    [JobId] UNIQUEIDENTIFIER NOT NULL, 
    [Status] INT NOT NULL, 
    [AppliedAt] DATETIME NULL, 
    [StatusChangedAt] DATETIME NULL, 
    CONSTRAINT [FK_Application_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [identity].[AspNetUsers]([Id]),
    CONSTRAINT [FK_Application_Job] FOREIGN KEY ([JobId]) REFERENCES [zaposlime].[Job]([Id])
)
