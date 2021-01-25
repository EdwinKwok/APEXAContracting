CREATE TABLE [dbo].[Contract]
(
	[Id]  UNIQUEIDENTIFIER NOT NULL DEFAULT (newsequentialid()), 
	[OfferedById] UNIQUEIDENTIFIER NOT NULL,
	[AcceptedById] UNIQUEIDENTIFIER NOT NULL,
	[OfferedByKey] NVARCHAR(10) NULL,
	[AcceptedBykey] NVARCHAR(10) NULL,
	[ContractName]  NVARCHAR(255) NULL,
	[EffectedOn] DATETIME NOT NULL DEFAULT GETDATE(),
	[ExpiredOn] DATETIME NOT NULL DEFAULT DATEADD(year,1,GETDATE()),
	[IsDeleted] BIT NOT NULL DEFAULT 0,
	[IsExpired] BIT NOT NULL DEFAULT 0,
	[ContractPath] VARCHAR(MAX) NULL,
	CONSTRAINT [PK_Contract] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_Contract_OfferedBy] FOREIGN KEY ([OfferedById]) REFERENCES [dbo].[BusinessUnit]([Id]),
	CONSTRAINT [FK_Contract_AcceptedBy] FOREIGN KEY ([AcceptedById]) REFERENCES [dbo].[BusinessUnit]([Id]),
);
GO
CREATE UNIQUE INDEX UX_Contract_Participants ON [Contract] ([OfferedById], [AcceptedBykey] )
