CREATE TABLE [zaposlime].[EmployerReview]
(
	[Id]                                UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Rating] INT NOT NULL, 
    [Comment] NVARCHAR(MAX) NULL,
    [EmployerId]                        NVARCHAR (450) NOT NULL,
    [CreatedBy]                         NVARCHAR (450)  NULL, 
    [CreatedAt]                         DATETIME NULL, 
    [UpdatedBy]                         NVARCHAR (450)  NULL, 
    [UpdatedAt]                         DATETIME NULL, 
    CONSTRAINT [FK_Review_Employer] FOREIGN KEY ([EmployerId]) REFERENCES [identity].[AspNetUsers]([Id]),
)
