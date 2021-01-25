
using APEXAContracting.Common;
using APEXAContracting.Model.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace APEXAContracting.Business.Interface
{
    public interface IBusinessUnitService
    {
        BusinessResult<List<BusinessUnitExtDTO>> GetBusinessUnitList();
        BusinessResult<BusinessUnitExtDTO> GetBusinessUnitById(Guid id);
        BusinessResult<BusinessUnitDTO> UpdateBusinessUnit(BusinessUnitDTO dto);
        BusinessResult<BusinessUnitDTO> AddBusinessUnit(BusinessUnitDTO dto);
        BusinessResult<BusinessUnitDTO> DeleteBusinessUnit(Guid id);
        BusinessResult<List<BusinessUnitExtDTO>> GetBusinessUnitByTypeList(int typeId);
    }
}
