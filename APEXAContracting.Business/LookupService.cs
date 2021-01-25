using AutoMapper;
using APEXAContracting.Business.Interface;
using APEXAContracting.Common.Interfaces;
using APEXAContracting.DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using APEXAContracting.Model.DTO;

namespace APEXAContracting.Business
{
    public class LookupService : BaseService, ILookupService
    {
        private readonly IMapper _mapper;
        private readonly IHostingEnvironment _environment;

        /// <summary>
        ///  Dependency inject IUnitOfWork which accessing FirstOnSite.DocumentManagementOrders database.
        /// </summary>
        /// <param name="uow"></param>
        public LookupService(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfigSettings config, IHostingEnvironment environment, ILogger<LookupService> logger)
            : base(uow, httpContextAccessor, config, logger)
        {
            this._mapper = mapper;
            this._environment = environment;
        }

     

    

        public List<LookupNum> GetLanguageLookup(bool includeEmptyRow)
        {
            List<LookupNum> result = this.UOW.Languages.GetAll<LookupNum>().OrderBy(r => r.Key).ToList();
            if (includeEmptyRow)
            {
                LookupNum emptyRow = new LookupNum() { Value = string.Empty };
                result.Insert(0, emptyRow);
            }
            return result;
            throw new Exception();
        }

        public List<LookupNum> GetHealthStatusLookup(bool includeEmptyRow)
        {
            List<LookupNum> result = this.UOW.HealthStatuss.GetAll<LookupNum>().OrderBy(r => r.Key).ToList();
            if (includeEmptyRow)
            {
                LookupNum emptyRow = new LookupNum() { Value = string.Empty };
                result.Insert(0, emptyRow);
            }
            return result;
            throw new Exception();
        }
    }
 }
