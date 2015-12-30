CREATE TABLE [dbo].[PushAllUser] (
    [UserId]        NVARCHAR (128) NOT NULL,
    [PushAllUserId] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_PushAllUser] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [FK_PushAllUser_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);

