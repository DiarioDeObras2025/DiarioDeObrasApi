using DiarioObras.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DiarioObras.Data.Context;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<Obra>? Obras { get; set; }
    public DbSet<FotoRegistro>? FotoRegistros { get; set; }
    public DbSet<RegistroDiario>? RegistroDiarios { get; set; }
    public DbSet<Empresa>? Empresas { get; set; }
    public DbSet<MaterialUtilizado>? MaterialUtilizados { get; set; }
    public DbSet<DocumentoRegistro>? DocumentoRegistros { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<RegistroDiario>()
            .HasIndex(r => r.ObraId);

        builder.Entity<RegistroDiario>()
            .HasIndex(r => r.Data);

        builder.Entity<RegistroDiario>()
            .HasIndex(r => new { r.ObraId, r.Data });
    }

}
