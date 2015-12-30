

CREATE PROCEDURE [dbo].[UpdateTagsAndStatus]
	@Tags [dbo].[TagTable] READONLY
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Tags]([Name])
	OUTPUT inserted.*
	SELECT [TagName]
	FROM @Tags
	GROUP BY [TagName]
	HAVING [TagName] NOT IN (SELECT [Name] FROM [dbo].[Tags])

	INSERT INTO [dbo].[QuestionTags] ([QuestionId], [TagId])
	SELECT inboundTable.[QuestionId], tagTable.[TagId]
	FROM @Tags AS inboundTable
	INNER JOIN [dbo].[Tags] AS tagTable ON tagTable.[Name] = inboundTable.TagName

	UPDATE [dbo].[Questions]
	SET [StatusId] = 2
	WHERE [QuestionId] IN (SELECT [QuestionId] FROM @Tags GROUP BY [QuestionId])
END