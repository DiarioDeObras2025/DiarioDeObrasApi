using AutoMapper;
using DiarioObras.DTOs.ObraDTOs;
using DiarioObras.Models;

namespace DiarioObras.AutoMapper.ObraMapping;

public class ObraDTOMappingProfile : Profile
{
    public ObraDTOMappingProfile()
    {
        // Mapeamento principal Obra -> ObraDTO
        CreateMap<Obra, ObraDTO>()
            .ForMember(dest => dest.TotalRegistrosDiarios,
                      opt => opt.MapFrom(src => src.RegistrosDiarios != null ? src.RegistrosDiarios.Count : 0))
            .ForMember(dest => dest.Status,
                      opt => opt.MapFrom(src => src.Status.ToString())) // Converte enum para string
            .AfterMap((src, dest) =>
            {
                // Lógica adicional pós-mapeamento se necessário
            });

        // Mapeamento inverso ObraDTO -> Obra
        CreateMap<ObraDTO, Obra>()
            .ForMember(dest => dest.Status,
                      opt => opt.MapFrom(src => Enum.Parse<Obra.StatusObra>(src.Status)))
            .ForMember(dest => dest.RegistrosDiarios,
                      opt => opt.Ignore()); // Ignora o mapeamento dos registros

        // Mapeamento simplificado para ObraDTONome
        CreateMap<Obra, ObraDTONome>().ReverseMap();
    }
}