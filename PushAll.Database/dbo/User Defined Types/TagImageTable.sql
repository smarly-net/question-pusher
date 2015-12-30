CREATE TYPE [dbo].[TagImageTable] AS TABLE (
    [TagId] INT             NOT NULL,
    [Image] VARBINARY (MAX) NULL,
		[Subscribers] INT NULL
		);

