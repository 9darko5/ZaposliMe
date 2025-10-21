CREATE VIEW [zaposlime].[JobGridView]
	AS 
	SELECT 
    j.Id,
    j.Title,
    j.Description,
    j.NumberOfWorkers,
    j.EmployerId,
    j.CreatedAt,
    u.FirstName + ' ' + u.LastName AS EmployerFullName,
    j.CityId,
    c.Name AS CityName,
    ISNULL(ar.AverageRating, 0.00) AS EmployerRating
FROM [zaposlime].[Job] j
LEFT JOIN [identity].[AspNetUsers] u 
    ON u.Id = j.EmployerId
LEFT JOIN [zaposlime].[City] c 
    ON c.Id = j.CityId
LEFT JOIN (
    SELECT 
        EmployerId,
        CAST(AVG(CAST(Rating AS DECIMAL(9,4))) AS DECIMAL(5,1)) AS AverageRating
    FROM [zaposlime].[EmployerReview]
    GROUP BY EmployerId
) ar
    ON ar.EmployerId = j.EmployerId;
