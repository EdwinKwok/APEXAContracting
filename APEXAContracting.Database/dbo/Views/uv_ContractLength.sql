CREATE VIEW [dbo].[uv_ContractLength]
	AS 
	Select 
	Id,
	OfferedById,
	AcceptedById,
	OfferedByKey,
	AcceptedBykey,
	ContractName,
	EffectedOn,
	ExpiredOn,
	IsDeleted,
	IsExpired,
	ContractPath,
	(len(ContractPath) - len(replace(ContractPath, '|', '')))+1 As PathLength
	From Contract
