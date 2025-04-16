using AutoMapper;
using DiarioObras.DTOs.RegistroDiarioDTOs;
using DiarioObras.Models;

namespace DiarioObras.AutoMapper.RegistroDiarioMapping
{
    public class RegistroDiarioDTOMappingProfile : Profile
    {
        public RegistroDiarioDTOMappingProfile()
        {
            // Mapeamento de MaterialUtilizado para MaterialUtilizadoDTO
            CreateMap<MaterialUtilizado, MaterialUtilizadoDTO>();

            // Mapeamento de MaterialUtilizadoDTO para MaterialUtilizado
            CreateMap<MaterialUtilizadoDTO, MaterialUtilizado>();

            // Mapeamento de MaterialUtilizado para MaterialUtilizadoDTO
            CreateMap<MembroEquipe, MembroEquipeDTO>();

            // Mapeamento de MaterialUtilizadoDTO para MaterialUtilizado
            CreateMap<MembroEquipeDTO, MembroEquipe>();

            CreateMap<RegistroDiario, RegistroDiarioResumoDTO>()
             .ForMember(dest => dest.NomeObra, opt => opt.MapFrom(src => src.Obra.Nome));


            // Entidade → DTO
            CreateMap<RegistroDiario, RegistroDiarioDTO>()
                 .ForMember(dest => dest.Equipe, opt => opt.MapFrom(src => src.Equipe))
                .ForMember(dest => dest.Materiais, opt => opt.MapFrom(src =>
                    src.Materiais != null
                        ? src.Materiais.Select(m => new MaterialUtilizadoDTO
                        {
                            Nome = m.Nome,
                            Quantidade = m.Quantidade,
                            Unidade = m.Unidade
                        }).ToList()
                        : new List<MaterialUtilizadoDTO>()
                ));

            // DTO → Entidade
            CreateMap<RegistroDiarioDTO, RegistroDiario>()
                .ForMember(dest => dest.Equipe, opt => opt.MapFrom(src => src.Equipe))
                .ForMember(dest => dest.Materiais, opt => opt.MapFrom(src =>
                    src.Materiais != null
                        ? src.Materiais.Select(m => new MaterialUtilizado
                        {
                            Nome = m.Nome,
                            Quantidade = m.Quantidade,
                            Unidade = m.Unidade
                        }).ToList()
                        : new List<MaterialUtilizado>()
                ))
                .ForMember(dest => dest.Fotos, opt => opt.Ignore())
                .ForMember(dest => dest.Documentos, opt => opt.Ignore())
                .ForMember(dest => dest.Obra, opt => opt.Ignore());
        }
    }
}