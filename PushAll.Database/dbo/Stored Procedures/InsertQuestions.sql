CREATE PROCEDURE [dbo].[InsertQuestions]
	@Questions [dbo].[QuestionTable] READONLY
AS
BEGIN
	SET NOCOUNT ON;

	MERGE [dbo].[Questions] AS T
	USING @Questions AS S
	ON (T.[ProviderId] = S.[ProviderId] AND T.[Url] = S.[Url]) 
	WHEN NOT MATCHED BY TARGET 
		THEN INSERT([Title], [Url], [ProviderId], [StatusId]) VALUES(S.[Title], S.[Url], S.[ProviderId], 1)
	OUTPUT inserted.*;

END