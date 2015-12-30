

CREATE PROCEDURE [dbo].[GetTags]
	@UserId NVARCHAR(128) = NULL
	,@IncludeImage BIT = 0
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @userTags TABLE
	(
		[TagId] INT
	)

	IF @UserId IS NOT NULL
	BEGIN
		INSERT INTO @userTags
		SELECT [TagId]
		FROM [dbo].[UserTags]
		WHERE [UserId] = @UserId
	END

	SELECT	tagsTable.[TagId]
			,tagsTable.[Name]
			,CASE WHEN @IncludeImage = 1 THEN tagsTable.[Image] ELSE NULL END AS [Image]
			,CAST (CASE WHEN tagsTable.[Image] IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS [HasImage]
			,tagsTable.[Subscribers]
			,CAST (CASE WHEN userTagsTable.[TagId] IS NOT NULL THEN 1 ELSE 0 END AS BIT) AS [IsSubscribed]
	FROM [dbo].[Tags] AS tagsTable
	LEFT OUTER JOIN @userTags AS userTagsTable ON tagsTable.[TagId] = userTagsTable.[TagId]

END