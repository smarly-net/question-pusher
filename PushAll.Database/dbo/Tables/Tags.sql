CREATE TABLE [dbo].[Tags] (
    [TagId]       INT             IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (64)   COLLATE Cyrillic_General_CI_AS NOT NULL,
    [Image]       VARBINARY (MAX) NULL,
    [Subscribers] INT             CONSTRAINT [DF_Tags_Subscribers] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED ([TagId] ASC),
    CONSTRAINT [IX_Tags] UNIQUE NONCLUSTERED ([Name] ASC)
);



