using System.ComponentModel.DataAnnotations;

namespace DiarioObras.Models;

public class Empresa
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nome { get; set; }

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;

    public ICollection<ApplicationUser> Usuarios { get; set; }
    public ICollection<Obra> Obras { get; set; }
}

