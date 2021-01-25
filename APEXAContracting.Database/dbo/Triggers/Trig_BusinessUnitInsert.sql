CREATE TRIGGER [Trig_BusinessUnitInsert]
ON [dbo].[BusinessUnit]
INSTEAD OF INSERT
AS
BEGIN
	SET NOCOUNT ON
	--get random Health Status Id
	DECLARE @MinTally int, @MaxTally int, @RandomNumber int, @HSId int, @BusinessTypeId int

	SELECT @BusinessTypeId = [BusinessTypeId] FROM Inserted

	--Health Status apply to Advisor only
	SET @HSId = 0
	IF (@BusinessTypeId = 3)
	BEGIN
		SELECT @MinTally = MIN(uv_HealthStatusWeight.TallyStart),
			   @MaxTally = MAX(uv_HealthStatusWeight.TallyEnd)
		FROM uv_HealthStatusWeight;
		SELECT @RandomNumber = FLOOR(RAND()*(@MaxTally-@MinTally+1))+@MinTally 
		SELECT @HSId = Id FROM uv_HealthStatusWeight 
		WHERE @RandomNumber BETWEEN uv_HealthStatusWeight.TallyStart AND uv_HealthStatusWeight.TallyEnd
	END

	--generate Hierarchy key
	DECLARE @hPrefix char(3), @keyCnt int, @hKey varchar(10)
	SELECT @hPrefix = UPPER(SUBSTRING([Name], 1,3)) FROM Inserted
	SELECT @keyCnt  = Count(*) FROM BusinessUnit WHERE [HierarchyPrefix] = @hPrefix
	SET @keyCnt = @keyCnt + 1
	SELECT @hKey = @hPrefix + '<' + RIGHT('00000' + CONVERT(varchar(5),@keyCnt),5) + '>'


    INSERT INTO BusinessUnit ([Id], [Name], [Name2], [Address], [Phone], [HealthStatusId], [BusinessTypeId], [HierarchyPrefix], [HierarchyKey])
    SELECT [Id], [Name], [Name2], [Address], [Phone] ,@HSId, [BusinessTypeId], @hPrefix, @hKey
    FROM   Inserted
END
