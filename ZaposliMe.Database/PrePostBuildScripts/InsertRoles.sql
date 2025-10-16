INSERT INTO [identity].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp])
VALUES 
(NEWID(), 'Admin', 'ADMIN', NEWID()),
(NEWID(), 'Employer', 'EMPLOYER', NEWID()),
(NEWID(), 'Employee', 'EMPLOYEE', NEWID());