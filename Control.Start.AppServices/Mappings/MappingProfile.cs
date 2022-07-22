using System;
using System.Collections.Generic;
using System.Text;

namespace Control.Facilites.AppServices.Mappings
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<Dtos.DocumentoFilterDto, Domain.Filters.DocumentoFilter>().ReverseMap();
            CreateMap<Dtos.FornecedorFilterDto, Domain.Filters.FornecedorFilter>().ReverseMap();
            CreateMap<Dtos.UsuarioFilterDto, Domain.Filters.UsuarioFilter>().ReverseMap();
            CreateMap<Dtos.DocumentoFilterDto, Domain.Filters.DocumentoFilter>().ReverseMap();

        }
    }

}
