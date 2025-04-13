using APICatalogo.Services;
using DiarioObras.AutoMapper.EmpresaMapping;
using DiarioObras.AutoMapper.ObraMapping;
using DiarioObras.AutoMapper.RegistroDiarioMapping;
using DiarioObras.Data.Interfaces;
using DiarioObras.Data.Repositories;

namespace DiarioObras.Configurations;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        services.AddScoped<IObraRepository, ObraRepository>();
        services.AddScoped<IRegistroDiarioRepository, RegistroDiarioRepository>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmpresaRepository, EmpresaRepository>();

        return services;
    }
}

public static class AutoMapperConfig
{
    public static IServiceCollection AddAutoMapperConfig(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ObraDTOMappingProfile));
        services.AddAutoMapper(typeof(RegistroDiarioDTOMappingProfile));
        services.AddAutoMapper(typeof(EmpresaDTOMappingProfile));

        return services;
    }
}
