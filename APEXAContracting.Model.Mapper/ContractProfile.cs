using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using APEXAContracting.Common;
using System.Linq;


namespace APEXAContracting.Model.Mapper
{
    public class ContractProfile : Profile
    {
        public ContractProfile()
        {
            CreateMap<Entity.Contract, DTO.ContractDTO>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(e => e.Id))
                .ForMember(dto => dto.OfferedById, conf => conf.MapFrom(e => e.OfferedById))
                .ForMember(dto => dto.OfferedByKey, conf => conf.MapFrom(e => e.OfferedByKey))
                .ForMember(dto => dto.AcceptedById, conf => conf.MapFrom(e => e.AcceptedById))
                .ForMember(dto => dto.AcceptedBykey, conf => conf.MapFrom(e => e.AcceptedBykey))
                .ForMember(dto => dto.ContractName, conf => conf.MapFrom(e => e.ContractName))
                .ForMember(dto => dto.EffectedOn, conf => conf.MapFrom(e => e.EffectedOn))
                .ForMember(dto => dto.ExpiredOn, conf => conf.MapFrom(e => e.ExpiredOn))
                .ForMember(dto => dto.IsDeleted, conf => conf.MapFrom(e => e.IsDeleted))
                .ForMember(dto => dto.IsExpired, conf => conf.MapFrom(e => e.IsExpired))
                .ForMember(dto => dto.ContractPath, conf => conf.MapFrom(e => e.ContractPath));

        }
    }
}
