using AutoMapper;
using DiarioObras.DTOs.RegistroDiarioDTOs;
using DiarioObras.Models;

namespace DiarioObras.AutoMapper.RegistroDiarioMapping
{
    public class RegistroDiarioDTOMappingProfile : Profile
    {
        public RegistroDiarioDTOMappingProfile()
        {
            // Entidade → DTO
            CreateMap<RegistroDiario, RegistroDiarioDTO>()
                .ForMember(dest => dest.CondicoesClimaticas,
                    opt => opt.MapFrom(src => src.CondicoesClimaticas.ToString()))
                .ForMember(dest => dest.Etapa,
                    opt => opt.MapFrom(src => src.Etapa.ToString()));

            // DTO → Entidade
            CreateMap<RegistroDiarioDTO, RegistroDiario>()
                //.ForMember(dest => dest.CondicoesClimaticas,
                //    opt => opt.MapFrom(src => Enum.Parse<CondicaoClimaticaEnum>(src.CondicoesClimaticas)))
                //.ForMember(dest => dest.Etapa,
                //    opt => opt.MapFrom(src => Enum.Parse<EtapaObraEnum>(src.Etapa)))
                .ForMember(dest => dest.Fotos, opt => opt.Ignore())
                .ForMember(dest => dest.Documentos, opt => opt.Ignore())
                .ForMember(dest => dest.Obra, opt => opt.Ignore())
                .ForMember(dest => dest.Materiais, opt => opt.Ignore());
        }
    }
}