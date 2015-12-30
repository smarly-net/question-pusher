CREATE TYPE [dbo].[TagTable] AS TABLE (
    [QuestionId] BIGINT        NOT NULL,
    [TagName]  NVARCHAR (64) COLLATE Cyrillic_General_CI_AS NOT NULL);

