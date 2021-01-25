Print 'Populating [Language]';

MERGE INTO [dbo].[Language] AS Target
USING (VALUES
  (1,'en-us','English')
 ,(2,'fr-ca','French')
) AS Source ([Id],[Code],[Name])
ON (Target.[Id] = Source.[Id])
WHEN MATCHED AND (Target.[Code] <> Source.[Code] OR Target.[Name] <> Source.[Name]) THEN
	UPDATE SET
		[Code] = Source.[Code], 
		[Name] = Source.[Name]
WHEN NOT MATCHED BY TARGET THEN
	INSERT([Id],[Code],[Name])
	VALUES(Source.[Id],Source.[Code],Source.[Name])
WHEN NOT MATCHED BY SOURCE THEN 
	DELETE;
GO
Print '--[Language] Populated ok';
Print ''


Print 'Populating [HealthStatus]';

Set IDENTITY_INSERT [dbo].[HealthStatus] ON
MERGE INTO [dbo].[HealthStatus] AS Target
USING (VALUES
  (0,'NA', 0)
 ,(1,'Green', 70)
 ,(2,'Red', 30)
) AS Source ([Id],[Name],[Weight])
ON (Target.[Id] = Source.[Id])
WHEN MATCHED AND (Target.[Name] <> Source.[Name] OR Target.[Weight] <> Source.[Weight]) THEN
	UPDATE SET
		[Name] = Source.[Name],
		[Weight] = Source.[Weight]
WHEN NOT MATCHED BY TARGET THEN
	INSERT([Id],[Name],[Weight])
	VALUES(Source.[Id],Source.[Name],Source.[Weight])
WHEN NOT MATCHED BY SOURCE THEN 
	DELETE;
GO
Set IDENTITY_INSERT [dbo].[HealthStatus] OFF
Print '--[HealthStatus] Populated ok';
Print ''


Print 'Populating [BusinessType]';
Set IDENTITY_INSERT [dbo].[BusinessType] ON
MERGE INTO [dbo].[BusinessType] AS Target
USING (VALUES
  (1,'Carrier')
 ,(2,'MGA')
  ,(3,'Advisor')
) AS Source ([Id],[Name])
ON (Target.[Id] = Source.[Id])
WHEN MATCHED AND (Target.[Name] <> Source.[Name]) THEN
	UPDATE SET
		[Name] = Source.[Name]
WHEN NOT MATCHED BY TARGET THEN
	INSERT([Id],[Name])
	VALUES(Source.[Id],Source.[Name])
WHEN NOT MATCHED BY SOURCE THEN 
	DELETE;
GO
Set IDENTITY_INSERT [dbo].[BusinessType] OFF
Print '--[BusinessType] Populated ok';
Print ''


Print 'Populating [BusinessUnit]';
IF NOT EXISTS (SELECT Id FROM [BusinessUnit] WHERE [NAME] = 'CarrierX'  AND [BusinessTypeId] = 1)
BEGIN
	INSERT[dbo].[BusinessUnit]([Id], [Name],[BusinessTypeId])
	VALUES('6DB5A9E3-DC5D-EB11-B383-2016B97D0838', 'CarrierX',1)
END

IF NOT EXISTS (SELECT Id FROM [BusinessUnit] WHERE [NAME] = 'MGAY'  AND [BusinessTypeId] = 2)
BEGIN
	INSERT[dbo].[BusinessUnit]([Id], [Name],[BusinessTypeId])
	VALUES('6EB5A9E3-DC5D-EB11-B383-2016B97D0838', 'MGAY',2)
END

IF NOT EXISTS (SELECT Id FROM [BusinessUnit] WHERE [NAME] = 'AdvisorZ'  AND [BusinessTypeId] = 3)
BEGIN
	INSERT[dbo].[BusinessUnit]([Id], [Name],[BusinessTypeId])
	VALUES('6FB5A9E3-DC5D-EB11-B383-2016B97D0838', 'AdvisorZ',3)
END
Print '--[BusinessUnit] Populated ok';
Print ''
GO



Print 'Populating [BusinessContract]';
IF NOT EXISTS (SELECT Id FROM [Contract] WHERE [ContractName] = 'Contract1')
BEGIN
	INSERT[dbo].[Contract]([OfferedById], [OfferedByKey], [AcceptedById], [AcceptedBykey], [ContractName], [ContractPath])
	VALUES('6DB5A9E3-DC5D-EB11-B383-2016B97D0838', 'CAR<00001>', '6EB5A9E3-DC5D-EB11-B383-2016B97D0838', 'MGA<00001>', 'Contract1', 'CAR<00001>|MGA<00001>')
END

IF NOT EXISTS (SELECT Id FROM [Contract] WHERE [ContractName] = 'Contract2')
BEGIN
	INSERT[dbo].[Contract]([OfferedById], [OfferedByKey], [AcceptedById], [AcceptedBykey], [ContractName], [ContractPath])
	VALUES('6EB5A9E3-DC5D-EB11-B383-2016B97D0838', 'MGA<00001>', '6FB5A9E3-DC5D-EB11-B383-2016B97D0838', 'ADV<00001>', 'Contract2', 'CAR<00001>|MGA<00001>|ADV<00001>')
END

Print '--[BusinessContract] Populated ok';
Print ''
GO






