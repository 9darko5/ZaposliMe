CREATE TABLE [identity].[AspNetUsers]
(
	[Id]                   NVARCHAR (450) NOT NULL,
	[Initials]             NVARCHAR (5)   NULL,
    [UserName]             NVARCHAR (256) NOT NULL,
    [NormalizedUserName]   NVARCHAR (256) NOT NULL,
    [Email]                NVARCHAR (256) NULL,
    [NormalizedEmail]      NVARCHAR (256) NULL,
    [EmailConfirmed]       BIT            NOT NULL,
    [PasswordHash]         NVARCHAR (MAX) NULL,
    [SecurityStamp]        NVARCHAR (MAX) NULL,
    [ConcurrencyStamp]     NVARCHAR (MAX) NULL, 
    [PhoneNumber]          NVARCHAR (MAX) NULL,
    [PhoneNumberConfirmed] BIT            NOT NULL,
    [TwoFactorEnabled]     BIT            NOT NULL,
    [LockoutEnd]           DATETIME       NULL,
    [LockoutEnabled]       BIT            NOT NULL,
    [AccessFailedCount]    INT            NOT NULL,
    [AccountState]         INT NOT NULL DEFAULT 1, 
    [Language]             NVARCHAR(50) NULL, 
    CONSTRAINT [PK_user.AspNetUsers] PRIMARY KEY CLUSTERED ([Id] ASC)
)
