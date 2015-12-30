CREATE TABLE [dbo].[Statuses] (
    [StatusId]    SMALLINT      IDENTITY (1, 1) NOT NULL,
    [Description] NVARCHAR (64) NOT NULL,
    CONSTRAINT [PK_Statuses] PRIMARY KEY CLUSTERED ([StatusId] ASC)
);

