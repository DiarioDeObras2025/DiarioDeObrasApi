using System.ComponentModel.DataAnnotations;

namespace DiarioObras.Models;

public class FotoRegistro
{
    public int Id { get; set; }
    [Required]
    [StringLength(80)]
    public string? Descricao { get; set; }
    public string? CaminhoArquivo { get; set; }
    public DateTime DataUpload { get; set; } = DateTime.UtcNow;

    public int RegistroDiarioId { get; set; }
    public RegistroDiario? RegistroDiario { get; set; }

    public string? Categoria { get; set; }

    [StringLength(50)]
    public string? Localizacao { get; set; }
}