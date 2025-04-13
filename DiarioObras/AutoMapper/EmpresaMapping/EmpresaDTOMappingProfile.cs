using DiarioObras.Models;
using AutoMapper;
using DiarioObras.DTOs.EmpresaDTOs;

namespace DiarioObras.AutoMapper.EmpresaMapping
{
    public class EmpresaDTOMappingProfile : Profile
    {
        public EmpresaDTOMappingProfile()
        {
            CreateMap<Empresa, CreateEmpresaDTO>().ReverseMap();
        }

    }
}
