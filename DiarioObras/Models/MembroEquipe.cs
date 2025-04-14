using System.ComponentModel.DataAnnotations;

namespace DiarioObras.Models;

public class MembroEquipe
{
    [Key]
    public int Id { get; set; }

    [StringLength(100)]
    public string Nome { get; set; }

    [StringLength(50)]
    public string Cargo { get; set; }

    [StringLength(200)]
    public string? Observacao { get; set; }

    public bool Terceirizado { get; set; }

    public int RegistroDiarioId { get; set; }
    public RegistroDiario? RegistroDiario { get; set; }
}