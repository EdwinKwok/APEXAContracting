using APEXAContracting.Common;
using APEXAContracting.Model.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace APEXAContracting.Business.Interface
{
    public interface IContractService
    {
        BusinessResult<List<ContractDTO>> GetContractList();
        BusinessResult<ContractDTO> GetContractById(Guid id);
        BusinessResult<ContractDTO> GetContractByKeys(string OfferedByKey, string AcceptedByKey);
        BusinessResult<ContractDTO> UpdateContract(ContractDTO dto);
        BusinessResult<ContractDTO> AddContract(ContractDTO dto);
        BusinessResult<ContractDTO> DeleteContract(Guid id);
    }
}
