using System.Collections.Generic;
using System.Text;
using AutoMapper;
using APEXAContracting.Common;
using System.Linq;

namespace APEXAContracting.Model.Mapper
{
    public class BusinessTypeProfile : Profile

    {
        public BusinessTypeProfile()
        {
            CreateMap<Entity.BusinessType, DTO.BusinessTypeDTO>()
                    .ForMember(dto => dto.Id, conf => conf.MapFrom(e => e.Id))
                    .ForMember(dto => dto.Name, conf => conf.MapFrom(e => e.Name))
                    .ForMember(dto => dto.IsDeleted, conf => conf.MapFrom(e => e.IsDeleted));

        }
    }
}
