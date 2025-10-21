CREATE TABLE [zaposlime].[EmployeeReview]
(
	[Id]                                UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Rating] INT NOT NULL, 
    [Comment] NVARCHAR(MAX) NULL,
    [EmployeeId]                        NVARCHAR (450) NOT NULL,
    [CreatedBy]                         UNIQUEIDENTIFIER NULL, 
    [CreatedAt]                         DATETIME NULL, 
    [UpdatedBy]                         UNIQUEIDENTIFIER NULL, 
    [UpdatedAt]                         DATETIME NULL, 
    CONSTRAINT [FK_Review_Employee] FOREIGN KEY ([EmployeeId]) REFERENCES [identity].[AspNetUsers]([Id]),
)
