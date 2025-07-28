CREATE TABLE [identity].[AspNetUserRoles]
(
	[UserId] NVARCHAR(450) NOT NULL,
	[RoleId] NVARCHAR(450) NOT NULL,
    CONSTRAINT [PK_user.AspNetUserRoles] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_user.AspNetUserRoles_user.AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [identity].[AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_user.AspNetUserRoles_user.AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [identity].[AspNetUsers] ([Id]) ON DELETE CASCADE
)
