using System.ComponentModel.DataAnnotations;
using DiarioObras.Models;

namespace DiarioObras.DTOs.RegistroDiarioDTOs;

public class RegistroDiarioDTO
{
    public int Id { get; set; }


    public DateTime? Data { get; set; }


    [Required(ErrorMessage = "Houve um erro interno, contate o suporte!")]
    public int ObraId { get; set; }

    [Required(ErrorMessage = "O resumo é um campo obrigátorio")]
    public string Resumo { get; set; }

    [Required(ErrorMessage = "O titulo do relatorio é um campo obrigátorio")]
    public string Titulo { get; set; }

    public CondicaoClimaticaEnum CondicoesClimaticas { get; set; }


    public int TotalFuncionarios { get; set; }


    public int TotalTerceirizados { get; set; }

    public int HorasTrabalhadas { get; set; } = 8;

    // Materiais e Equipamentos
    public string? Equipamentos { get; set; }

    [Range(0, 1000, ErrorMessage = "O consumo de cimento deve ser entre 0 e 1000 sacos")]
    public int ConsumoCimento { get; set; }

    public List<string>? Materiais { get; set; } // Lista simplificada para o DTO

    // Progresso
    public EtapaObraEnum Etapa { get; set; }

    [Range(0, 100, ErrorMessage = "O percentual concluído deve ser entre 0% e 100%")]
    public int PercentualConcluido { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "A área executada deve ser um valor positivo")]
    public decimal AreaExecutada { get; set; }

    // Ocorrências e Ambiente
    public string? Ocorrencias { get; set; }

    [Range(-50, 50, ErrorMessage = "A temperatura deve estar entre -50°C e 50°C")]
    public decimal? Temperatura { get; set; }

    [Range(0, 500, ErrorMessage = "A precipitação deve ser entre 0mm e 500mm")]
    public decimal? Precipitacao { get; set; }

    // Anexos (para upload)
    //public List<IFormFile>? Fotos { get; set; }
    //public List<IFormFile>? Documentos { get; set; }

    // Assinatura
    public string? AssinaturaResponsavel { get; set; }
    public DateTime? DataAssinatura { get; set; }
}

// DTO para resposta (quando não precisamos dos arquivos)
public class RegistroDiarioResponseDTO : RegistroDiarioDTO
{
    public List<string>? UrlsFotos { get; set; }
    public List<string>? UrlsDocumentos { get; set; }
    public DateTime DataCriacao { get; set; }
}