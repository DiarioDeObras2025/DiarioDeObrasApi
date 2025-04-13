using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static DiarioObras.DTOs.ObraDTOs.ObraDTO;

namespace DiarioObras.Models;
public class Obra
{
    public Obra()
    {
        RegistrosDiarios = new Collection<RegistroDiario>();
    }

    [Key]
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
    public StatusObra Status { get; set; }
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public DateTime? DataAtualizacao { get; set; }

    public int EmpresaId { get; set; }
    public Empresa Empresa { get; set; }

    [JsonIgnore]
    public ICollection<RegistroDiario>? RegistrosDiarios { get; set; }
    public enum StatusObra
    {
        Andamento,
        Cancelada,
        Planejada,
        Concluida,
        Pausada
    }

}