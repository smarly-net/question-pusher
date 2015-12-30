CREATE TABLE [dbo].[UserTags] (
    [UserId] NVARCHAR (128) NOT NULL,
    [TagId]  INT            NOT NULL,
    CONSTRAINT [PK_UserTags] PRIMARY KEY CLUSTERED ([UserId] ASC, [TagId] ASC),
    CONSTRAINT [FK_UserTags_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_UserTags_Tags] FOREIGN KEY ([TagId]) REFERENCES [dbo].[Tags] ([TagId])
);

