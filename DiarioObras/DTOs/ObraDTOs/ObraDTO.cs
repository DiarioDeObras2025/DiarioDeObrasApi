using DiarioObras.DTOs.RegistroDiarioDTOs;
using DiarioObras.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiarioObras.DTOs.ObraDTOs;

public class ObraDTO
{
    public int Id { get; set; }

    [Required]
    [StringLength(80)]
    public string? Nome { get; set; }

    [Required]
    [StringLength(150)]
    public string? Endereco { get; set; }
    [Required]
    [StringLength(80)]
    public string? Cliente { get; set; }

    [StringLength(30)]
    public string? NumeroContrato { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime? DataTerminoPrevista { get; set; }

    [Required]
    [StringLength(80)]
    public string? EngenheiroResponsavel { get; set; }
    public string Status { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataAtualizacao { get; set; }
    public ICollection<RegistroDiarioDTO>? RegistrosDiarios { get; set; }
    [NotMapped] // Se estiver usando Entity Framework
    public int TotalRegistrosDiarios => RegistrosDiarios?.Count ?? 0;

}
