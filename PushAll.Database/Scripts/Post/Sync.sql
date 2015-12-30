SET IDENTITY_INSERT [dbo].[Providers] ON

DECLARE @tosterProviderId SMALLINT = 1
	,@stackoverflowProviderId SMALLINT = 2

IF (NOT EXISTS(SELECT * FROM [dbo].[Providers] WHERE [ProviderId] = @tosterProviderId))
BEGIN
	INSERT INTO [dbo].[Providers]([ProviderId], [Description])
	VALUES (@tosterProviderId, N'Toster')
END

IF (NOT EXISTS(SELECT * FROM [dbo].[Providers] WHERE [ProviderId] = @stackoverflowProviderId))
BEGIN
	INSERT INTO [dbo].[Providers]([ProviderId], [Description])
	VALUES (@stackoverflowProviderId, N'stackoverflow')
END

SET IDENTITY_INSERT [dbo].[Providers] OFF

SET IDENTITY_INSERT [dbo].[Statuses] ON

DECLARE @inProgressStatusId SMALLINT = 1
	,@readyToSendStatusId SMALLINT = 2
	,@sendingStatusId SMALLINT = 3
	,@completeStatusId SMALLINT = 4

IF (NOT EXISTS(SELECT * FROM [dbo].[Statuses] WHERE [StatusId] = @inProgressStatusId))
BEGIN
	INSERT INTO [dbo].[Statuses]([StatusId], [Description])
	VALUES (@inProgressStatusId, N'В обработке')
END

IF (NOT EXISTS(SELECT * FROM [dbo].[Statuses] WHERE [StatusId] = @readyToSendStatusId))
BEGIN
	INSERT INTO [dbo].[Statuses]([StatusId], [Description])
	VALUES (@readyToSendStatusId, N'Готово к отправке')
END

IF (NOT EXISTS(SELECT * FROM [dbo].[Statuses] WHERE [StatusId] = @sendingStatusId))
BEGIN
	INSERT INTO [dbo].[Statuses]([StatusId], [Description])
	VALUES (@sendingStatusId, N'Отправляется')
END

IF (NOT EXISTS(SELECT * FROM [dbo].[Statuses] WHERE [StatusId] = @completeStatusId))
BEGIN
	INSERT INTO [dbo].[Statuses]([StatusId], [Description])
	VALUES (@completeStatusId, N'Отправлено')
END

SET IDENTITY_INSERT [dbo].[Statuses] OFF