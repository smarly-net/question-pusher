

CREATE PROCEDURE [dbo].[InsertError]
	@Message NVARCHAR(MAX)
	,@StackTrace NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Errors]
		([Message]
		,[StackTrace])
	VALUES
		(@Message
		,@StackTrace)

END