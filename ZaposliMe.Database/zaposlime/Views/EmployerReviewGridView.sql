CREATE VIEW [zaposlime].[EmployerReviewGridView]
	AS
    WITH R AS
    (
        SELECT
            er.EmployerId,
            COUNT_BIG(*) AS TotalReviews,
            SUM(CASE
                    WHEN er.Comment IS NOT NULL AND LTRIM(RTRIM(er.Comment)) <> N'' THEN 1
                    ELSE 0
                END) AS CommentCount,
            AVG(CAST(er.Rating AS DECIMAL(10,4))) AS AvgRating
        FROM [zaposlime].[EmployerReview] er
        GROUP BY er.EmployerId
    )
    SELECT
        r.EmployerId,
        u.FirstName + N' ' + u.LastName AS EmployerFullName,
        r.TotalReviews,
        r.CommentCount,
        CAST(r.AvgRating AS DECIMAL(4,1)) AS AverageRating
    FROM R r
    INNER JOIN [identity].[AspNetUsers] u
        ON u.Id = r.EmployerId;
    GO