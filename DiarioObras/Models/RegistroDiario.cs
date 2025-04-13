using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace DiarioObras.Models;

public class RegistroDiario
{

    public RegistroDiario()
    {
        Fotos = new Collection<FotoRegistro>();
        Materiais = new Collection<MaterialUtilizado>();
    }

    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime Data { get; set; }

    public string Titulo { get; set; }

    public int ObraId { get; set; }
    public Obra? Obra { get; set; }

    [Required]
    [StringLength(300)]
    public string Resumo { get; set; }

    public CondicaoClimaticaEnum CondicoesClimaticas { get; set; }

    // Equipe
    public int TotalFuncionarios { get; set; }
    public int TotalTerceirizados { get; set; }
    public int HorasTrabalhadas { get; set; } = 8; // Valor padrão

    // Materiais e Equipamentos
    public string? Equipamentos { get; set; }
    public int ConsumoCimento { get; set; }
    public ICollection<MaterialUtilizado> Materiais { get; set; }

    // Progresso
    public EtapaObraEnum Etapa { get; set; }
    public int PercentualConcluido { get; set; }
    public decimal AreaExecutada { get; set; }

    // Ocorrências e Ambiente
    public string? Ocorrencias { get; set; }
    public decimal? Temperatura { get; set; }
    public decimal? Precipitacao { get; set; }

    // Anexos
    public ICollection<FotoRegistro> Fotos { get; set; }
    public ICollection<DocumentoRegistro> Documentos { get; set; } = new Collection<DocumentoRegistro>();

    // Controle
    public string? AssinaturaResponsavel { get; set; }
    public DateTime? DataAssinatura { get; set; }
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

public enum EtapaObraEnum
{
    [Display(Name = "Preparação do terreno")]
    PreparacaoTerreno,
    [Display(Name = "Fundação")]
    Fundacao,
    [Display(Name = "Estrutura")]
    Estrutura,
    [Display(Name = "Alvenaria")]
    Alvenaria,
    [Display(Name = "Instalações")]
    Instalacoes,
    [Display(Name = "Cobertura")]
    Cobertura,
    [Display(Name = "Acabamento")]
    Acabamento
}
