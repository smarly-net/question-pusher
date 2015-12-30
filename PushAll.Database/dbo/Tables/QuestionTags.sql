CREATE TABLE [dbo].[QuestionTags] (
    [QuestionId] BIGINT NOT NULL,
    [TagId]      INT    NOT NULL,
    CONSTRAINT [PK_QuestionTags] PRIMARY KEY CLUSTERED ([QuestionId] ASC, [TagId] ASC)
);

