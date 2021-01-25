CREATE TABLE [dbo].[BusinessUnit]
(
	[Id]  UNIQUEIDENTIFIER NOT NULL DEFAULT (newsequentialid()), 
	[Name] NVARCHAR(255) NOT NULL, 
	[Name2] NVARCHAR(255) NULL DEFAULT '', 
	[Address] NVARCHAR(500) NULL, 
	[Phone] NVARCHAR(20) NULL, 
	[HierarchyPrefix] NVARCHAR(3) NOT NULL DEFAULT '', 
	[HierarchyKey] NVARCHAR(10) NOT NULL DEFAULT '', 
	[HealthStatusId] TINYINT NOT NULL,
	[BusinessTypeId] TINYINT NOT NULL,
	[IsDeleted] BIT NOT NULL DEFAULT 0,
	CONSTRAINT [PK_BusinessUnit] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_BusinessUnit_BusinessType] FOREIGN KEY ([BusinessTypeId]) REFERENCES [dbo].[BusinessType]([Id]),
	CONSTRAINT [FK_BusinessUnit_HealthStatus] FOREIGN KEY ([HealthStatusId]) REFERENCES [dbo].[HealthStatus]([Id]),
);
GO

CREATE INDEX IX_BusinessUnit_HierarchyPrefix ON BusinessUnit (HierarchyPrefix)
GO 
CREATE UNIQUE INDEX UX_BusinessUnit_NAMES ON BusinessUnit ([Name], [Name2])
GO
