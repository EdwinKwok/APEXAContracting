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
    public class ContractService : BaseService, IContractService
    {
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _environment;

        /// <summary>
        ///  Dependency inject IUnitOfWork which accessing FirstOnSite.DocumentManagementOrders database.
        /// </summary>
        /// <param name="uow"></param>
        public ContractService(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfigSettings config, IHostingEnvironment environment, ILogger<LookupService> logger)
            : base(uow, httpContextAccessor, config, logger)
        {
            this._mapper = mapper;
            this._environment = environment;
        }

        public BusinessResult<List<ContractDTO>> GetContractList()
        {
            BusinessResult<List<ContractDTO>> result = new BusinessResult<List<ContractDTO>>() { ResultStatus = ResultStatus.Success };
            try
            {
                //do not retun the ContractPath to reduce traffic
                var dtoCTX = UOW.Contracts.GetAll<ContractDTO>().Where(ctx => ctx.IsDeleted == false).Select(
                    t => new ContractDTO
                    {
                        Id = t.Id,
                        OfferedById = t.OfferedById,
                        AcceptedById = t.AcceptedById,
                        OfferedByKey = t.OfferedByKey,
                        AcceptedBykey = t.AcceptedBykey,
                        ContractName = t.ContractName,
                        EffectedOn = t.EffectedOn,
                        ExpiredOn = t.ExpiredOn,
                        IsDeleted = t.IsDeleted,
                        IsExpired = t.IsExpired
                    }).ToList();
                result.Item = dtoCTX;
                return result;
            }
            catch (Exception ex)
            {
                this.Logger.LogError("{0} exception: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
                return new BusinessResult<List<ContractDTO>>(ResultStatus.Failure, "", "Get Data Failure.");
            }

        }

        public BusinessResult<ContractDTO> GetContractById(Guid id)
        {
            BusinessResult<ContractDTO> result = new BusinessResult<ContractDTO>() { ResultStatus = ResultStatus.Success };
            try
            {
                //do not retun the ContractPath to reduce traffic
                var dtoCTX = UOW.Contracts.GetAll<ContractDTO>().Where(ctx => ctx.Id == id).Select(
                    t => new ContractDTO
                    {
                        Id = t.Id,
                        OfferedById = t.OfferedById,
                        AcceptedById = t.AcceptedById,
                        OfferedByKey = t.OfferedByKey,
                        AcceptedBykey = t.AcceptedBykey,
                        ContractName = t.ContractName,
                        EffectedOn = t.EffectedOn,
                        ExpiredOn = t.ExpiredOn,
                        IsDeleted = t.IsDeleted,
                        IsExpired = t.IsExpired
                    }).FirstOrDefault();
                result.Item = dtoCTX;
                return result;
            }
            catch (Exception ex)
            {
                this.Logger.LogError("{0} exception: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
                return new BusinessResult<ContractDTO>(ResultStatus.Failure, "", "Get Data Failure.");
            }

        }
        public BusinessResult<ContractDTO> GetContractByKeys(string OfferedByKey, string AcceptedByKey)
        {
            BusinessResult<ContractDTO> result = new BusinessResult<ContractDTO>() { ResultStatus = ResultStatus.Success };
            try
            {

                string[] userIds = { OfferedByKey, AcceptedByKey };
                //UvContractLength contractPath = UOW.uv_ContractLengths.GetAll().Where(uv => userIds.Contains(uv.ContractPath))
                //    .OrderByDescending(uv => uv.PathLength).FirstOrDefault();
                var contractPath = UOW.uv_ContractLengths.GetAll()
                    .Where(uv => uv.ContractPath.Contains(OfferedByKey) || uv.ContractPath.Contains(AcceptedByKey))
                    .OrderBy(uv => uv.PathLength).ThenBy(uv => uv.Id).FirstOrDefault();

                ContractDTO dtoCTX = new ContractDTO();
                if (contractPath != null)
                {
                    dtoCTX.Id = contractPath.Id;
                    dtoCTX.OfferedById = contractPath.OfferedById;
                    dtoCTX.AcceptedById = contractPath.AcceptedById;
                    dtoCTX.OfferedByKey = contractPath.OfferedByKey;
                    dtoCTX.AcceptedBykey = contractPath.AcceptedBykey;
                    dtoCTX.ContractName = contractPath.ContractName;
                    dtoCTX.EffectedOn = contractPath.EffectedOn;
                    dtoCTX.ExpiredOn = contractPath.ExpiredOn;
                    dtoCTX.IsDeleted = contractPath.IsDeleted;
                    dtoCTX.IsExpired = contractPath.IsExpired;
                }
                result.Item = dtoCTX;
                return result;
            }
            catch (Exception ex)
            {
                this.Logger.LogError("{0} exception: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
                return new BusinessResult<ContractDTO>(ResultStatus.Failure, "", "Get Data Failure.");
            }

        }

        public BusinessResult<ContractDTO> UpdateContract(ContractDTO dto)
        {
            BusinessResult<ContractDTO> result = new BusinessResult<ContractDTO>() { ResultStatus = ResultStatus.Success };
            if (dto.Id == Guid.Empty)
            {
                this.Logger.LogError("{0} exception: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Id is empty");
                return new BusinessResult<ContractDTO>(ResultStatus.Failure, "", "Update Failure.");
            }

            try
            {
                //ContractPath is not sending to user for reduce traffic, get the whole record again for update
                Contract entity = new Contract();
                var ctxDTO = UOW.Contracts.GetAll<ContractDTO>().Where(ctx => ctx.Id == dto.Id);
                var dtoForUpdate = (from ctx in ctxDTO
                                    select new ContractDTO
                                    {
                                        Id = ctx.Id,
                                        OfferedById = ctx.OfferedById,
                                        AcceptedById = ctx.AcceptedById,
                                        OfferedByKey = ctx.OfferedByKey,
                                        AcceptedBykey = ctx.AcceptedBykey,
                                        ContractName = ctx.ContractName,
                                        EffectedOn = ctx.EffectedOn,
                                        ExpiredOn = ctx.ExpiredOn,
                                        IsDeleted = ctx.IsDeleted,
                                        IsExpired = ctx.IsExpired,
                                        ContractPath = ctx.ContractPath

                                    }).FirstOrDefault();

                entity.Id = dtoForUpdate.Id;
                entity.OfferedById = dtoForUpdate.OfferedById;
                entity.AcceptedById = dtoForUpdate.AcceptedById;
                entity.OfferedByKey = dtoForUpdate.OfferedByKey;
                entity.AcceptedBykey = dtoForUpdate.AcceptedBykey;
                entity.ContractName = dto.ContractName;
                entity.EffectedOn = dto.EffectedOn;
                entity.ExpiredOn = dto.ExpiredOn;
                entity.IsDeleted = dto.IsDeleted;
                if (DateTime.Now > dto.ExpiredOn)
                {
                    dto.IsExpired = true;
                }
                entity.IsExpired = dto.IsExpired;
                entity.ContractPath = dtoForUpdate.ContractPath;

                UOW.Contracts.Update(entity);
                if (UOW.Save() > 0)
                {
                    ContractDTO returnDTO = new ContractDTO();

                    returnDTO.Id = entity.Id;
                    returnDTO.ContractName = entity.ContractName;
                    returnDTO.IsDeleted = entity.IsDeleted;
                    result.Item = returnDTO;
                    result.Message = string.Format("{0} Successfully.", System.Reflection.MethodBase.GetCurrentMethod().Name);
                    result.ResultStatus = ResultStatus.Success;
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogError("{0} exception: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
                return new BusinessResult<ContractDTO>(ResultStatus.Failure, "", "Save Failure.");
            }
            return result;
        }
        public BusinessResult<ContractDTO> AddContract(ContractDTO dto)
        {
            BusinessResult<ContractDTO> result = new BusinessResult<ContractDTO>() { ResultStatus = ResultStatus.Success };
            if (dto.OfferedById == dto.AcceptedById)
            {
                this.Logger.LogError("{0} exception: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Cannot create self contract");
                return new BusinessResult<ContractDTO>(ResultStatus.Failure, "", "Add Failure.");

            }
            try
            {
                Contract entity = new Contract();
                entity.Id = dto.Id;
                entity.OfferedById = dto.OfferedById;
                entity.AcceptedById = dto.AcceptedById;
                entity.OfferedByKey = dto.OfferedByKey;
                entity.AcceptedBykey = dto.AcceptedBykey;
                entity.ContractName = dto.ContractName;
                entity.EffectedOn = dto.EffectedOn;
                entity.ExpiredOn = dto.ExpiredOn;
                entity.IsDeleted = dto.IsDeleted;
                if (DateTime.Now > dto.ExpiredOn)
                {
                    dto.IsExpired = true;
                }
                entity.IsExpired = dto.IsExpired;
                //entity.ContractPath = dto.ContractPath;

                string[] userIds = { dto.OfferedByKey, dto.AcceptedBykey };
                //UvContractLength contractPath = UOW.uv_ContractLengths.GetAll().Where(uv => userIds.Contains(uv.ContractPath))
                //    .OrderByDescending(uv => uv.PathLength).FirstOrDefault();
                var contractPath = UOW.uv_ContractLengths.GetAll()
                    .Where(uv => uv.ContractPath.Contains(dto.OfferedByKey) || uv.ContractPath.Contains(dto.AcceptedBykey))
                    .OrderByDescending(uv => uv.PathLength).Select(uv => uv.ContractPath).FirstOrDefault();


                string path = contractPath;
                if (!path.Contains(dto.OfferedByKey))
                {
                    path = path + "|" + dto.OfferedByKey;
                }

                if (!path.Contains(dto.AcceptedBykey))
                {
                    path += "|" + dto.AcceptedBykey;
                }
                entity.ContractPath = path;

                UOW.Contracts.Add(entity);
                if (UOW.Save() > 0)
                {
                    ContractDTO returnDTO = new ContractDTO();

                    returnDTO.Id = entity.Id;
                    returnDTO.ContractName = entity.ContractName;
                    returnDTO.IsDeleted = entity.IsDeleted;
                    result.Item = returnDTO;
                    result.Message = string.Format("{0} Successfully.", System.Reflection.MethodBase.GetCurrentMethod().Name);
                    result.ResultStatus = ResultStatus.Success;
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogError("{0} exception: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
                return new BusinessResult<ContractDTO>(ResultStatus.Failure, "", "Save Failure.");
            }
            return result;
        }
        public BusinessResult<ContractDTO> DeleteContract(Guid id)
        {
            //only do soft delete BusinessUnit to maintain data integrity
            BusinessResult<ContractDTO> result = new BusinessResult<ContractDTO>() { ResultStatus = ResultStatus.Success };
            if (id == Guid.Empty)
            {
                this.Logger.LogError("{0} exception: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Id is empty");
                return new BusinessResult<ContractDTO>(ResultStatus.Failure, "", "Delete Failure.");
            }
            try
            {
                //All related contract will be expired, logic in table trigger
                Contract entity = new Contract();
                entity = UOW.Contracts.FirstOrDefault(bu => bu.Id == id);
                if (entity == null)
                {
                    this.Logger.LogError("{0} exception: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, "Record Not found");
                    return new BusinessResult<ContractDTO>(ResultStatus.Failure, "", "Delete Failure.");
                }
                entity.IsDeleted = true;

                UOW.Contracts.Update(entity);
                if (UOW.Save() > 0)//Save OK
                {
                    ContractDTO returnDTO = new ContractDTO();
                    returnDTO.Id = entity.Id;
                    returnDTO.ContractName = entity.ContractName;
                    returnDTO.IsDeleted = entity.IsDeleted;
                    result.Item = returnDTO;
                    result.Message = string.Format("{0} Successfully.", System.Reflection.MethodBase.GetCurrentMethod().Name);
                    result.ResultStatus = ResultStatus.Success;
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogError("{0} exception: {1}", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.ToString());
                return new BusinessResult<ContractDTO>(ResultStatus.Failure, "", "Save Failure.");

            }
            return result;

        }

    }
}
