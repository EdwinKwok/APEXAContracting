using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using APEXAContracting.Common;
using System.Linq;


namespace APEXAContracting.Model.Mapper
{
    public class BusinessUnitProfile : Profile
    {
        public BusinessUnitProfile()
        {

            CreateMap<Entity.BusinessUnit, DTO.BusinessUnitDTO>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(e => e.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(e => e.Name))
                .ForMember(dto => dto.Name2, conf => conf.MapFrom(e => e.Name2))
                .ForMember(dto => dto.Address, conf => conf.MapFrom(e => e.Address))
                .ForMember(dto => dto.Phone, conf => conf.MapFrom(e => e.Phone))
                .ForMember(dto => dto.HierarchyPrefix, conf => conf.MapFrom(e => e.HierarchyPrefix))
                .ForMember(dto => dto.HierarchyKey, conf => conf.MapFrom(e => e.HierarchyKey))
                .ForMember(dto => dto.HealthStatusId, conf => conf.MapFrom(e => e.HealthStatusId))
                .ForMember(dto => dto.BusinessTypeId, conf => conf.MapFrom(e => e.BusinessTypeId))
                .ForMember(dto => dto.IsDeleted, conf => conf.MapFrom(e => e.IsDeleted));

        }
    }
}
