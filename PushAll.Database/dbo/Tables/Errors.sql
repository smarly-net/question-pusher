CREATE TABLE [dbo].[Errors] (
    [ErrorId]    INT            IDENTITY (1, 1) NOT NULL,
    [Message]    NVARCHAR (MAX) NOT NULL,
    [StackTrace] NVARCHAR (MAX) NOT NULL,
    [CreateDate] DATETIME       CONSTRAINT [DF_Errors_CreateDate] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_Errors] PRIMARY KEY CLUSTERED ([ErrorId] ASC)
);

