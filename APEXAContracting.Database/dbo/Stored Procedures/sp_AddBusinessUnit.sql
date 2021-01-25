CREATE PROCEDURE [dbo].[sp_AddBusinessUnit]
	@Name nvarchar(255), @Name2 nvarchar(255), @Address nvarchar(500), @Phone varchar(20), @BusinessTypeId int
	
AS
	Insert into [dbo].[BusinessUnit] ([Name], [Name2], [Address], [Phone], [BusinessTypeId])
	Values(@Name, @Name2, @Address, @Phone, @BusinessTypeId)
RETURN 0
