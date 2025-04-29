using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiarioObras.Models;

public class RegistroDiario
{

    public RegistroDiario()
    {
        Fotos = new Collection<FotoRegistro>();
        Materiais = new Collection<MaterialUtilizado>();
        Equipe = new Collection<MembroEquipe>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime Data { get; set; }

    public string Titulo { get; set; }

    public int ObraId { get; set; }
    public Obra? Obra { get; set; }

    public ICollection<AtividadeRegistro> Atividades { get; set; }

    public CondicaoClimaticaEnum CondicoesClimaticas { get; set; }

    public ICollection<MembroEquipe> Equipe { get; set; }
    public ICollection<MaterialUtilizado> Materiais { get; set; }

    public string? Ocorrencias { get; set; }

    public ICollection<FotoRegistro> Fotos { get; set; }
    public ICollection<DocumentoRegistro> Documentos { get; set; } = new Collection<DocumentoRegistro>();

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}

public enum CondicaoClimaticaEnum
{
    Ensolarado,
    ParcialmenteNublado,
    Nublado,
    Chuvoso,
    ChuvaForte,
    Tempestade,
    Ventania,
    Granizo
}