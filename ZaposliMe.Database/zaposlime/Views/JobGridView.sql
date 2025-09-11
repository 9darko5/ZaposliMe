CREATE VIEW [zaposlime].[JobGridView]
	AS SELECT 
	Id,
	Title,
	Description,
	NumberOfWorkers,
	EmployerId,
	CreatedAt
	FROM [Job]
