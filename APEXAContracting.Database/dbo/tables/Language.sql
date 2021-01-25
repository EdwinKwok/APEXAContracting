CREATE TABLE [dbo].[Language]
(
	[Id] INT NOT NULL, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Code] NVARCHAR(10) NOT NULL, 
    CONSTRAINT [PK_Language] PRIMARY KEY ([Id])
)
