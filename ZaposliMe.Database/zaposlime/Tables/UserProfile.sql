CREATE TABLE [zaposlime].[UserProfile]
(
	[Id] NVARCHAR(450) NOT NULL PRIMARY KEY,
	[AspNetUserId] NVARCHAR(450) NOT NULL,
	[FirstName] NVARCHAR(50) NOT NULL,
	[LastName] NVARCHAR(50) NOT NULL,
	[Age] BIGINT NULL,
	CONSTRAINT FK_UserProfile_AspNetUser
	FOREIGN KEY ([AspNetUserId]) REFERENCES [identity].[AspNetUser]([Id])
)
