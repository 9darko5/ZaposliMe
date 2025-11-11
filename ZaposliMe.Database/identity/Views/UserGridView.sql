CREATE VIEW [identity].[UserGridView]
	AS SELECT
	u.Id,
	u.FirstName,
	u.LastName,
	u.Email,
	u.PhoneNumber,
	u.Age,
	u.IsDeleted
	FROM [identity].[AspNetUsers] u
