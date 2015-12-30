CREATE TABLE [dbo].[Questions] (
    [QuestionId]  BIGINT         IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (MAX) NOT NULL,
    [Url]         NVARCHAR (256) NOT NULL,
    [ProviderId]  SMALLINT       NOT NULL,
    [StatusId]    SMALLINT       NOT NULL,
    [SendingDate] DATETIME       NULL,
    CONSTRAINT [PK_Questions] PRIMARY KEY CLUSTERED ([QuestionId] ASC),
    CONSTRAINT [FK_Questions_Providers] FOREIGN KEY ([ProviderId]) REFERENCES [dbo].[Providers] ([ProviderId]),
    CONSTRAINT [FK_Questions_Statuses] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[Statuses] ([StatusId]),
    CONSTRAINT [IX_Questions] UNIQUE NONCLUSTERED ([Url] ASC)
);






GO
CREATE NONCLUSTERED INDEX [IX_Questions_1]
    ON [dbo].[Questions]([StatusId] ASC, [SendingDate] ASC);

