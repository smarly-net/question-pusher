CREATE TYPE [dbo].[QuestionTable] AS TABLE (
    [Title]      NVARCHAR (MAX) NOT NULL,
    [Url]        NVARCHAR (256) NOT NULL,
    [ProviderId] SMALLINT       NOT NULL);

