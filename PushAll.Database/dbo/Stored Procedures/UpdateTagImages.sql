



CREATE PROCEDURE [dbo].[UpdateTagImages]
	@TagImages [dbo].[TagImageTable] READONLY
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE tagsTable
	SET tagsTable.[Image] = imageTable.[Image]
		,tagsTable.[Subscribers] = imageTable.[Subscribers]
	FROM [dbo].[Tags] AS tagsTable
	INNER JOIN @TagImages AS imageTable ON tagsTable.[TagId] = imageTable.[TagId]
END