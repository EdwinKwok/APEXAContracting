using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace APEXAContracting.Model.Mapper
{
    public class HealthStatusProfile : Profile
    {
        public HealthStatusProfile() {
            CreateMap<Entity.HealthStatus, DTO.HealthStatusDTO>()
                        .ForMember(dto => dto.Id, conf => conf.MapFrom(e => e.Id))
                        .ForMember(dto => dto.Name, conf => conf.MapFrom(e => e.Name))
                        .ForMember(dto => dto.Weight, conf => conf.MapFrom(e => e.Weight))
                        .ForMember(dto => dto.IsDeleted, conf => conf.MapFrom(e => e.IsDeleted));
        }
    }
}
