/* Sample Codes:
using AutoMapper;
using APEXAContracting.Business.Interface;
using APEXAContracting.Common;
using APEXAContracting.Common.Interfaces;
using APEXAContracting.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Dynamic;
using DTO = APEXAContracting.Model.DTO;
using Entity = APEXAContracting.Model.Entity;
using System.Linq;
using APEXAContracting.Common.Helpers;

namespace APEXAContracting.Business
{
    public class RegistrationService : BaseService, IRegistrationService
    {
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _environment;

        /// <summary>
        ///  Dependency inject IUnitOfWork which accessing FirstOnSite.DocumentManagementOrders database.
        /// </summary>
        /// <param name="uow"></param>
        public RegistrationService(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfigSettings config, IHostingEnvironment environment, ILogger<RegistrationService> logger)
            : base(uow, httpContextAccessor, config, logger)
        {
            this._mapper = mapper;
            this._environment = environment;
        }

        public List<DTO.RegistrationOutTime> GetRegistrationOutTime(Guid regId)
        {
            List<DTO.RegistrationOutTime> result = new List<DTO.RegistrationOutTime>();

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@RegId", regId);

            List<ExpandoObject> list = this.UOW.ExecuteStoredProcedureWithDynamicReturn("usp_Dashboard_RegistrationOutTime", parameters).ToList();
            foreach (dynamic item in list)
            {
                result.Add(new DTO.RegistrationOutTime()
                {
                    Id = item.Id,
                    RegistrationId = item.RegistrationId,
                    TimeRange = item.TimeRange,
                    StartDate = item.StartDate,
                    EndDate = item.EndDate
                });
            }
            return result;
        }

        public List<DTO.RegistrationHistoryLocation> GetRegistrationOutDetail(Guid regId, DateTime fromDate, DateTime toDate)
        {
            List<DTO.RegistrationHistoryLocation> result = new List<DTO.RegistrationHistoryLocation>();

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@RegId", regId);
            parameters.Add("@FromDate", fromDate);
            parameters.Add("@ToDate", toDate);

            List<ExpandoObject> list = this.UOW.ExecuteStoredProcedureWithDynamicReturn("usp_Dashboard_RegistrationOutDetail", parameters).ToList();
            foreach (dynamic item in list)
            {
                result.Add(new DTO.RegistrationHistoryLocation()
                {
                    HistoryDateTime = item.HistoryDateTime,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    IsolationStatusId = item.IsolationStatusId,
                    SignalVariation = item.SignalVariation
                });
            }
            return result;
        }

        public BusinessResult<string> Save(DTO.Registration model)
        {
            try
            {
                BusinessResult<string> result = new BusinessResult<string>();
                bool isActive = false;

                if (model.IsolateAddressLatitude.HasValue && model.IsolateAddressLatitude.Value != 0 &&
                        model.IsolateAddressLongitude.HasValue && model.IsolateAddressLongitude.Value != 0)
                {
                    GPSPoint gps = GPSHelper.WGS84ToGCJ02((double)model.IsolateAddressLatitude.Value, (double)model.IsolateAddressLongitude.Value);
                    model.IsolateAddressLatitude = (decimal)gps.Latitude;
                    model.IsolateAddressLongitude = (decimal)gps.Longitude;

                    isActive = true;
                }

                Entity.Registration temp = this.UOW.Registrations.FirstOrDefault(t => t.BeaconNumber == model.BeaconNumber);
                if (temp == null)
                {
                    temp = _mapper.Map<Entity.Registration>(model);
                    //TBD
                    temp.GroupId = 1;
                    temp.LanguageId = 1;
                    temp.SubjectId = 1;
                    temp.TimezoneId = 10;
                    temp.IsActive = isActive;
                    temp.IsArchived = false;
                    temp.CreatedOn = this.CurrentDateTime;
                    this.UOW.Registrations.Add(temp);
                }
                else
                {
                    temp.IsActive = isActive;
                    temp.IsArchived = false;
                    temp.IsolateAddressLatitude = model.IsolateAddressLatitude;
                    temp.IsolateAddressLongitude = model.IsolateAddressLongitude;
                    temp.PrimaryPhone = model.PrimaryPhone;
                    temp.SecondaryPhone = model.SecondaryPhone;
                    temp.IsolateAddress = model.IsolateAddress;
                    temp.AliasName = model.AliasName;
                    temp.Email = model.Email;
                    temp.ModifiedOn = CurrentDateTime;
                }
                if (this.UOW.Save() > 0)
                {
                    result.ResultStatus = ResultStatus.Success;
                    result.Item = temp.Id.ToString();
                }
                else
                {
                    result.ResultStatus = ResultStatus.Failure;
                    result.Message = "Save Failure!";
                }

                return result;
            }
            catch (System.Exception ex)
            {
                this.Logger.LogError("RegistrationService.Create exception: {0}", ex.ToString());
                return new BusinessResult<string>(ResultStatus.Failure, "", "Create Registration Failure.");
            }
        }


        public BusinessResult<string> UploadRealData(DTO.BeaconRealData model)
        {
            try
            {
                BusinessResult<string> result = new BusinessResult<string>();
                if (!model.HistoryDateTime.HasValue)
                    model.HistoryDateTime = DateTime.UtcNow;

                Entity.BeaconRealData entity = _mapper.Map<Entity.BeaconRealData>(model);

                if (model.Latitude.HasValue && model.Longitude.HasValue && (model.Latitude.Value != 0 || model.Longitude.Value != 0))
                {
                    GPSPoint gps = GPSHelper.WGS84ToGCJ02((double)model.Latitude.Value, (double)model.Longitude.Value);
                    entity.Latitude = (decimal)gps.Latitude;
                    entity.Longitude = (decimal)gps.Longitude;
                }

                entity.CreatedOn = this.CurrentDateTime;
                // entity.Id = Guid.NewGuid(); // new Sequentialid() set in table already.
                this.UOW.BeaconRealDatas.Add(entity);

                if (this.UOW.Save() > 0)
                {
                    int statusId = 2;
                    Entity.Registration temp = this.UOW.Registrations.QueryNoTracking(t => t.BeaconNumber == model.BeaconNumber && !t.IsArchived).FirstOrDefault();
                    if (temp != null)
                    {
                        Entity.RegistrationHistoryLocation tempLocation = this.UOW.RegistrationHistoryLocations
                            .QueryNoTracking(t => t.RegistrationId == temp.Id && t.BeaconNumber == model.BeaconNumber)
                            .OrderByDescending(t => t.HistoryDateTime).FirstOrDefault();
                        if (tempLocation != null && tempLocation.IsolationStatusId.HasValue)
                            statusId = tempLocation.IsolationStatusId.Value;
                    }

                    result.ResultStatus = ResultStatus.Success;
                    result.Item = statusId.ToString();
                }
                else
                {
                    result.ResultStatus = ResultStatus.Failure;
                    result.Message = "Save Failure!";
                }

                return result;
            }
            catch (System.Exception ex)
            {
                this.Logger.LogError("RegistrationService.UploadRealData exception: {0}", ex.ToString());
                return new BusinessResult<string>(ResultStatus.Failure, "", "Upload RealData Failure.");
            }
        }

        public BusinessResult<string> EndIsolate(string beaconNumber)
        {
            try
            {
                BusinessResult<string> result = new BusinessResult<string>();

                Entity.Registration entity = this.UOW.Registrations.FirstOrDefault(t => t.BeaconNumber == beaconNumber && !t.IsArchived);
                if (entity != null)
                {
                    entity.IsActive = true;
                    entity.IsArchived = true;
                    entity.ModifiedOn = this.CurrentDateTime;

                    if (this.UOW.Save() > 0)
                    {
                        result.ResultStatus = ResultStatus.Success;
                        result.Item = entity.Id.ToString();
                    }
                    else
                    {
                        result.ResultStatus = ResultStatus.Failure;
                        result.Message = "Unbundling Failure!";
                    }
                }
                else
                {
                    result.ResultStatus = ResultStatus.Failure;
                    result.Message = "Unbundling Failure!";
                }
                return result;
            }
            catch (System.Exception ex)
            {
                this.Logger.LogError("RegistrationService.EndIsolate exception: {0}", ex.ToString());
                return new BusinessResult<string>(ResultStatus.Failure, "", "End Isolate Failure.");
            }
        }

    }
}
*/