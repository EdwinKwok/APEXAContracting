using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using APEXAContracting.Business.Interface;
using APEXAContracting.Model.DTO;
using Microsoft.AspNetCore.Hosting;
using APEXAContracting.DataAccess;
using Microsoft.AspNetCore.Http;
using APEXAContracting.Common.Interfaces;
using Microsoft.Extensions.Logging;
using APEXAContracting.Model.Entity;
using System.Linq;
using APEXAContracting.Common;

namespace APEXAContracting.Business
{
    public class BusinessUnitService : BaseService, IBusinessUnitService
    {
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _environment;

        /// <summary>
        ///  Dependency inject IUnitOfWork which accessing FirstOnSite.DocumentManagementOrders database.
        /// </summary>
        /// <param name="uow"></param>
        public BusinessUnitService(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfigSettings config, IHostingEnvironment environment, ILogger<LookupService> logger)
            : base(uow, httpContextAccessor, config, logger)
        {
            this._mapper = mapper;
            this._environment = environment;
        }

        public BusinessResult<List<BusinessUnitExtDTO>> GetBusinessUnitList()
        {
            BusinessResult<List<BusinessUnitExtDTO>> result = new BusinessResult<List<BusinessUnitExtDTO>>() { ResultStatus = ResultStatus.Success };
            try
            {
                //List<BusinessUnitDTO> dto = this.UOW.BusinessUnits.GetAll<BusinessUnitDTO>().Where(bu => bu.IsDeleted == false).ToList();
                var dtoBU = this.UOW.BusinessUnits.GetAll<BusinessUnitDTO>().Where(bu => bu.IsDeleted == false);
                var dtoBT = this.UOW.BusinessTypes.GetAll<BusinessTypeDTO>();
                var dtoHS = this.UOW.HealthStatuss.GetAll<HealthStatusDTO>();
                var dto = (from bu in dtoBU
                           join bt in dtoBT
                           on bu.BusinessTypeId equals bt.Id
                           join hs in dtoHS 
                           on bu.HealthStatusId equals hs.Id
                           select new BusinessUnitExtDTO
                           {
                               Id = bu.Id,
                               Name =bu.Name,
                               Name2 = bu.Name2,
                               Address = bu.Address,
                               Phone = bu.Phone,
                               HierarchyPrefix = bu.HierarchyPrefix,
                               HierarchyKey = bu.HierarchyKey,
                               HealthStatusId = bu.HealthStatusId,
                               BusinessTypeId = bu.BusinessTypeId,
                               IsDeleted = bu.IsDeleted,
                               BusinessTypeName = bt.Name,
                               HealthStatusColor = hs.Name,
                               IsNewRecord = bu.IsNewRecord,
                           }
                    ).ToList();
                result.Item = dto;
                return result;
            }
            catch (Exception ex)
            {
                this.Logger.LogError("{0} exception: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
                return new BusinessResult<List<BusinessUnitExtDTO>>(ResultStatus.Failure, "", "Get Data Failure.");

            }
        }

        public BusinessResult<List<BusinessUnitExtDTO>> GetBusinessUnitByTypeList(int typeId)
        {
            BusinessResult<List<BusinessUnitExtDTO>> result = new BusinessResult<List<BusinessUnitExtDTO>>() { ResultStatus = ResultStatus.Success };
            try
            {
                //List<BusinessUnitDTO> dto = this.UOW.BusinessUnits.GetAll<BusinessUnitDTO>().Where(bu => bu.IsDeleted == false).ToList();
                var dtoBU = this.UOW.BusinessUnits.GetAll<BusinessUnitDTO>().Where(bu => bu.IsDeleted == false && bu.BusinessTypeId == typeId);
                var dtoBT = this.UOW.BusinessTypes.GetAll<BusinessTypeDTO>();
                var dtoHS = this.UOW.HealthStatuss.GetAll<HealthStatusDTO>();
                var dto = (from bu in dtoBU
                           join bt in dtoBT
                           on bu.BusinessTypeId equals bt.Id
                           join hs in dtoHS
                           on bu.HealthStatusId equals hs.Id
                           select new BusinessUnitExtDTO
                           {
                               Id = bu.Id,
                               Name = bu.Name,
                               Name2 = bu.Name2,
                               Address = bu.Address,
                               Phone = bu.Phone,
                               HierarchyPrefix = bu.HierarchyPrefix,
                               HierarchyKey = bu.HierarchyKey,
                               HealthStatusId = bu.HealthStatusId,
                               BusinessTypeId = bu.BusinessTypeId,
                               IsDeleted = bu.IsDeleted,
                               BusinessTypeName = bt.Name,
                               HealthStatusColor = hs.Name,
                               IsNewRecord = bu.IsNewRecord,
                           }
                    ).ToList();
                result.Item = dto;
                return result;
            }
            catch (Exception ex)
            {
                this.Logger.LogError("{0} exception: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
                return new BusinessResult<List<BusinessUnitExtDTO>>(ResultStatus.Failure, "", "Get Data Failure.");

            }
        }


        public BusinessResult<BusinessUnitExtDTO> GetBusinessUnitById(Guid id)
        {
            BusinessResult<BusinessUnitExtDTO> result = new BusinessResult<BusinessUnitExtDTO>() { ResultStatus = ResultStatus.Success };
            try
            {

                var dtoBU = this.UOW.BusinessUnits.GetAll<BusinessUnitDTO>().Where(bu => bu.Id == id);
                var dtoBT = this.UOW.BusinessTypes.GetAll<BusinessTypeDTO>();
                var dtoHS = this.UOW.HealthStatuss.GetAll<HealthStatusDTO>();
                var dto = (from bu in dtoBU
                           join bt in dtoBT
                           on bu.BusinessTypeId equals bt.Id
                           join hs in dtoHS
                           on bu.HealthStatusId equals hs.Id
                           select new BusinessUnitExtDTO
                           {
                               Id = bu.Id,
                               Name = bu.Name,
                               Name2 = bu.Name2,
                               Address = bu.Address,
                               Phone = bu.Phone,
                               HierarchyPrefix = bu.HierarchyPrefix,
                               HierarchyKey = bu.HierarchyKey,
                               HealthStatusId = bu.HealthStatusId,
                               BusinessTypeId = bu.BusinessTypeId,
                               IsDeleted = bu.IsDeleted,
                               BusinessTypeName = bt.Name,
                               HealthStatusColor = hs.Name,
                               IsNewRecord = bu.IsNewRecord,
                           }
                    ).FirstOrDefault();


                
                result.Item = dto;
                return result;
            }
            catch (Exception ex)
            {
                this.Logger.LogError("{0} exception: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
                return new BusinessResult<BusinessUnitExtDTO>(ResultStatus.Failure, "", "Get Data Failure.");

            }

        }

        public BusinessResult<BusinessUnitDTO> UpdateBusinessUnit(BusinessUnitDTO dto)
        {

            BusinessResult<BusinessUnitDTO> result = new BusinessResult<BusinessUnitDTO>();
            if (dto.Id == Guid.Empty)
            {
                this.Logger.LogError("{0} exception: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Id is empty");
                return new BusinessResult<BusinessUnitDTO>(ResultStatus.Failure, "", "Update Failure.");
            }

            try
            {
                BusinessUnit entity = new BusinessUnit();
                entity.Id = dto.Id;
                entity.Name = dto.Name;
                entity.Name2 = dto.Name2;
                entity.Address = dto.Address;
                entity.Phone = dto.Phone;
                entity.BusinessTypeId = dto.BusinessTypeId;
                entity.HierarchyPrefix = dto.HierarchyPrefix;
                entity.HierarchyKey = dto.HierarchyKey;
                entity.HealthStatusId = dto.HealthStatusId;
                entity.IsDeleted = dto.IsDeleted;

                UOW.BusinessUnits.Update(entity);
                if (UOW.Save() > 0)//Save OK
                {
                    BusinessUnitDTO returnDTO = new BusinessUnitDTO();
                    returnDTO.Id = entity.Id;
                    returnDTO.Name = entity.Name;
                    returnDTO.IsDeleted = entity.IsDeleted;
                    result.Item = returnDTO;
                    result.Message = string.Format("{0} Successfully.", System.Reflection.MethodBase.GetCurrentMethod().Name);
                    result.ResultStatus = ResultStatus.Success;
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogError("{0} exception: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
                return new BusinessResult<BusinessUnitDTO>(ResultStatus.Failure, "", "Save Failure.");
            }
            return result;
        }


        public BusinessResult<BusinessUnitDTO> AddBusinessUnit(BusinessUnitDTO dto)
        {
            BusinessResult<BusinessUnitDTO> result = new BusinessResult<BusinessUnitDTO>();
            try
            {
                if (dto.BusinessTypeId != 3)
                {
                    dto.Name2 = "-";
                }
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@Name", dto.Name);
                parameters.Add("@Name2", dto.Name2);
                parameters.Add("@Address", dto.Address);
                parameters.Add("@Phone", dto.Phone);
                parameters.Add("@BusinessTypeId", dto.BusinessTypeId.ToString());

                if (UOW.ExecuteStoredProcedure("sp_AddBusinessUnit", parameters) > 0)
                {
                    BusinessUnitDTO returnDTO = new BusinessUnitDTO();
                    returnDTO.Id = dto.Id;
                    returnDTO.Name = dto.Name;
                    returnDTO.IsDeleted = dto.IsDeleted;
                    result.Item = returnDTO;
                    result.Message = string.Format("{0} Successfully.", System.Reflection.MethodBase.GetCurrentMethod().Name);
                    result.ResultStatus = ResultStatus.Success;
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogError("{0} exception: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
                return new BusinessResult<BusinessUnitDTO>(ResultStatus.Failure, "", "Save Failure.");

            }
            return result;
        }

        public BusinessResult<BusinessUnitDTO> DeleteBusinessUnit(Guid id)
        {

            //only do soft delete BusinessUnit to maintain data integrity
            BusinessResult<BusinessUnitDTO> result = new BusinessResult<BusinessUnitDTO>();
            if (id == Guid.Empty)
            {
                this.Logger.LogError("{0} exception: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Id is empty");
                return new BusinessResult<BusinessUnitDTO>(ResultStatus.Failure, "", "Delete Failure.");
            }

            
            try
            {

                //All related contract will be expired, logic in table trigger
                //Switch to SP 

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters.Add("@id", id);
                

                if (UOW.ExecuteStoredProcedure("sp_DelBusinessUnit", parameters) > 0)
                {
                    BusinessUnitDTO returnDTO = new BusinessUnitDTO();
                    returnDTO.Id = Guid.NewGuid();
                    returnDTO.Name = "";
                    returnDTO.IsDeleted = true;
                    result.Item = returnDTO;
                    result.Message = string.Format("{0} Successfully.", System.Reflection.MethodBase.GetCurrentMethod().Name);
                    result.ResultStatus = ResultStatus.Success;
                }

                //BusinessUnit entity = new BusinessUnit();
                //entity = UOW.BusinessUnits.FirstOrDefault(bu => bu.Id == id);
                //if (entity == null)
                //{
                //    this.Logger.LogError("{0} exception: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Record Not found");
                //    return new BusinessResult<BusinessUnitDTO>(ResultStatus.Failure, "", "Delete Failure.");
                //}
                //entity.IsDeleted = true;

                //UOW.BusinessUnits.Update(entity);
                //if (UOW.Save() > 0)//Save OK
                //{
                //    BusinessUnitDTO returnDTO = new BusinessUnitDTO();
                //    returnDTO.Id = entity.Id;
                //    returnDTO.Name = entity.Name;
                //    returnDTO.IsDeleted = entity.IsDeleted;
                //    result.Item = returnDTO;
                //    result.Message = string.Format("{0} Successfully.", System.Reflection.MethodBase.GetCurrentMethod().Name);
                //    result.ResultStatus = ResultStatus.Success;
                //}

            }
            catch (Exception ex)
            {
                this.Logger.LogError("{0} exception: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
                return new BusinessResult<BusinessUnitDTO>(ResultStatus.Failure, "", "Save Failure.");

            }
            return result;
        }
    }
}
