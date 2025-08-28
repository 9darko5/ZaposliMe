CREATE VIEW [zaposlime].[JobGridView]
	AS SELECT 
	Id,
	Title,
	Description,
	NumberOfWorkers
	FROM [Job]
