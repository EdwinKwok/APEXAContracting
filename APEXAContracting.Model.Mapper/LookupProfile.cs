using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using APEXAContracting.Common;
using System.Linq;

namespace APEXAContracting.Model.Mapper
{
    /// <summary>
    ///  About authentication and authorization assocaited entity models and dto models mapping.
    /// </summary>
    public class LookupProfile : Profile
    {
        public LookupProfile()
        {

            CreateMap<Entity.Language, DTO.LookupNum>()
                   .ForMember(dto => dto.Key, conf => conf.MapFrom(e => e.Id))
                   .ForMember(dto => dto.Value, conf => conf.MapFrom(e => e.Name));
                   
            CreateMap<Entity.HealthStatus, DTO.LookupNum>()
                   .ForMember(dto => dto.Key, conf => conf.MapFrom(e => e.Id))
                   .ForMember(dto => dto.Value, conf => conf.MapFrom(e => e.Name));

        }
    }
}
