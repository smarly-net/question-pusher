


CREATE PROCEDURE [dbo].[GetQuestionForSend]
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @questions TABLE
	(
		[QuestionId] BIGINT
		,[Title] NVARCHAR(MAX)
		,[Url] NVARCHAR(256)
		,[ProviderId] SMALLINT
	)

	UPDATE [dbo].[Questions]
	SET [StatusId] = 3
		,[SendingDate] = GETUTCDATE()
	OUTPUT INSERTED.[QuestionId], INSERTED.[Title], INSERTED.[Url], INSERTED.[ProviderId] 
	INTO @questions ([QuestionId], [Title], [Url], [ProviderId])
	WHERE [StatusId] = 2

	SELECT questionTable.[QuestionId]
		, questionTable.[Title]
		, questionTable.[Url]
		, providerTable.[Description] AS [ProviderDescription]
		, pushallUserTable.[PushAllUserId]
	FROM @questions AS questionTable
	INNER JOIN [dbo].[Providers] AS providerTable ON providerTable.[ProviderId] = questionTable.[ProviderId]
	INNER JOIN [dbo].[QuestionTags] AS questionTagTable ON questionTable.[QuestionId] = questionTagTable.[QuestionId]
	INNER JOIN [dbo].[UserTags] AS userTagTable ON userTagTable.[TagId] = questionTagTable.[TagId]
	INNER JOIN [dbo].[PushAllUser] AS pushallUserTable ON pushallUserTable.[UserId] = userTagTable.[UserId]
	GROUP BY questionTable.[QuestionId], userTagTable.[UserId], questionTable.[Title], questionTable.[Url], providerTable.[Description], pushallUserTable.[PushAllUserId]
--	ORDER BY questionTable.[QuestionId] 

END