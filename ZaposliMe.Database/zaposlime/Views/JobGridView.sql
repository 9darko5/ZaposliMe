CREATE VIEW [zaposlime].[JobGridView]
	AS SELECT 
	j.Id,
	j.Title,
	j.Description,
	j.NumberOfWorkers,
	j.EmployerId,
	j.CreatedAt,
	u.FirstName + ' ' + u.LastName AS EmployerFullName
	FROM [zaposlime].[Job] j
	LEFT JOIN [identity].[AspNetUsers] u ON u.Id = j.EmployerId
