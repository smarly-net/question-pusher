CREATE TABLE [dbo].[Providers] (
    [ProviderId]  SMALLINT      IDENTITY (1, 1) NOT NULL,
    [Description] NVARCHAR (64) NOT NULL,
    CONSTRAINT [PK_Providers] PRIMARY KEY CLUSTERED ([ProviderId] ASC)
);

