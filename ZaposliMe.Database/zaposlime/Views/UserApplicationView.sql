CREATE VIEW [zaposlime].[UserApplicationView]
	AS SELECT  
	a.Id,
	a.EmployeeId,
	a.JobId,
	a.Status,
	j.Title AS JobTitle,
	j.Description AS JobDescription,
	u.FirstName + ' ' + u.LastName AS EmployerFullName,
	a.AppliedAt,
	j.CreatedAt AS JobCreatedAt
	FROM [zaposlime].[Application] a
	LEFT JOIN [zaposlime].[Job] j ON a.JobId = j.Id
	LEFT JOIN [identity].[AspNetUsers] u ON j.EmployerId = u.Id
