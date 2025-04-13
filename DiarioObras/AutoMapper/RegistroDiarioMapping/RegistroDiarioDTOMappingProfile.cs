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
                .ForMember(dest => dest.Materiais, opt => opt.MapFrom(src =>
                    src.Materiais != null
                        ? src.Materiais.Select(m => m.Nome).ToList()
                        : new List<string>()
                ));

            // DTO → Entidade
            CreateMap<RegistroDiarioDTO, RegistroDiario>()
                .ForMember(dest => dest.Materiais, opt => opt.MapFrom(src =>
                    src.Materiais != null
                        ? src.Materiais.Select(nome => new MaterialUtilizado { Nome = nome }).ToList()
                        : new List<MaterialUtilizado>()
                ))
                .ForMember(dest => dest.Fotos, opt => opt.Ignore())
                .ForMember(dest => dest.Documentos, opt => opt.Ignore())
                .ForMember(dest => dest.Obra, opt => opt.Ignore());
        }
    }

}