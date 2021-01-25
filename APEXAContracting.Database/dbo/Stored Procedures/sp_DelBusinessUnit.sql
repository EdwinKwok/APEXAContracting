CREATE PROCEDURE [dbo].[sp_DelBusinessUnit]
	@id UNIQUEIDENTIFIER
	
AS
DECLARE @ErrorNumber INT

BEGIN TRY
	BEGIN TRANSACTION
		UPDATE [BusinessUnit] SET IsDeleted = 1 WHERE ID = @id
		--Terminate all related contract
		UPDATE [Contract] SET IsDeleteD = 1 
		WHERE [OfferedById] = @id
		OR [AcceptedById] = @id

	COMMIT TRAN	

END TRY
BEGIN CATCH
    SELECT 
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_SEVERITY() AS ErrorSeverity,
        ERROR_STATE() AS ErrorState,
        ERROR_PROCEDURE() AS ErrorProcedure,
        ERROR_LINE() AS ErrorLine,
        ERROR_MESSAGE() AS ErrorMessage
    ROLLBACK TRANSACTION

END CATCH


RETURN 0
