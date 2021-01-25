//using AutoMapper;
//using APEXAContracting.Business.Interface;
//using APEXAContracting.Common.Interfaces;
//using APEXAContracting.DataAccess;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace APEXAContracting.Business
//{
//    public class SchedulerService : BaseService, ISchedulerService
//    {
//        private readonly IMapper _mapper;
//        private readonly IHostingEnvironment _environment;

//        /// <summary>
//        ///  Dependency inject IUnitOfWork which accessing FirstOnSite.DocumentManagementOrders database.
//        /// </summary>
//        /// <param name="uow"></param>
//        public SchedulerService(IUnitOfWork uow, IMapper mapper, IHttpContextAccessor httpContextAccessor, IConfigSettings config, IHostingEnvironment environment, ILogger<SchedulerService> logger)
//            : base(uow, httpContextAccessor, config, logger)
//        {
//            this._mapper = mapper;
//            this._environment = environment;
//        }

//        public void Run() {
//          // TODO.  Scheduler details logic here.
//        }
//    }
// }
