CREATE VIEW [zaposlime].[EmployerReviewView]
	AS SELECT 
	er.[Id],
    er.[EmployerId],
    er.[Rating],
    er.[Comment],
    er.[CreatedAt],
    er.[CreatedBy],
    u.FirstName + N' ' + u.LastName AS CreatedByFullName    
	FROM [EmployerReview] er
	LEFT JOIN [identity].[AspNetUsers] u ON u.Id = er.CreatedBy;
