CREATE TABLE [identity].[AspNetUserLogins]
(
	[LoginProvider] NVARCHAR (450) NOT NULL,
    [ProviderKey]   NVARCHAR (450) NOT NULL,
    [ProviderDisplayName] NVARCHAR (MAX) NULL,
    [UserId]        NVARCHAR (450) NOT NULL,
    CONSTRAINT [PK_user.AspNetUserLogins] PRIMARY KEY CLUSTERED ([LoginProvider] ASC, [ProviderKey] ASC, [UserId] ASC),
    CONSTRAINT [FK_user.AspNetUserLogins_user.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[AspNetUsers] ([Id]) ON DELETE CASCADE
)
