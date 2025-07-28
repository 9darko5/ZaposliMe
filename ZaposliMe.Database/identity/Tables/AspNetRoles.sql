CREATE TABLE [identity].[AspNetRoles]
(
	[Id] NVARCHAR(450) NOT NULL PRIMARY KEY,
	[Name] NVARCHAR(256) NULL,
	[NormalizedName] NVARCHAR(256) NULL,
	[ConcurrencyStamp] NVARCHAR(MAX) NULL
)
