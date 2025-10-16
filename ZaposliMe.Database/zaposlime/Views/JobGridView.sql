CREATE VIEW [zaposlime].[JobGridView]
	AS SELECT 
	j.Id,
	j.Title,
	j.Description,
	j.NumberOfWorkers,
	j.EmployerId,
	j.CreatedAt,
	u.FirstName + ' ' + u.LastName AS EmployerFullName,
	j.CityId,
	c.Name AS CityName
	FROM [zaposlime].[Job] j
	LEFT JOIN [identity].[AspNetUsers] u ON u.Id = j.EmployerId
	LEFT JOIN [zaposlime].[City] c ON c.Id = j.CityId
