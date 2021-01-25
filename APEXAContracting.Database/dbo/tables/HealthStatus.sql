CREATE TABLE [dbo].[HealthStatus]
(
	[Id] TINYINT IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(50) NOT NULL, 
	[Weight] TINYINT NOT NULL DEFAULT 1, --Weight of item in the set. Total is 100
	[IsDeleted] bit NOT NULL DEFAULT 0,
    CONSTRAINT [PK_HealthStatus] PRIMARY KEY ([Id]),
	CONSTRAINT [CHK__HealthStatusWeight] CHECK ([Weight] BETWEEN 0 AND 100)
)
