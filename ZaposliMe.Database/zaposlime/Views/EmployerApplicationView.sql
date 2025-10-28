CREATE VIEW [zaposlime].[EmployerApplicationView]
	AS SELECT  
	   a.[Id],
       a.[JobId]
      ,a.[Status]
      ,a.[AppliedAt]
	  ,j.Title AS JobTitle
	  ,j.Description AS JobDescription
      ,a.[StatusChangedAt]
	  ,j.[EmployerId]
	  ,u.FirstName + ' ' + u.LastName AS EmployeeFullName
	  ,u.PhoneNumber AS EmployeePhone
	  ,j.CreatedAt AS JobCreatedAt
  FROM [zaposlime].[Application] a
  LEFT JOIN [zaposlime].[Job] j ON J.Id = a.JobId
  LEFT JOIN [identity].[AspNetUsers] u ON a.[EmployeeId] = u.Id
