using System.ComponentModel.DataAnnotations;
using DiarioObras.DTOs.FotoRegistroDTOs;
using DiarioObras.Models;

namespace DiarioObras.DTOs.RegistroDiarioDTOs;

public class RegistroDiarioDTO
{
    public int Id { get; set; }

    public DateTime? Data { get; set; }

    [Required(ErrorMessage = "Houve um erro interno, contate o suporte!")]
    public int ObraId { get; set; }

    [Required(ErrorMessage = "O título do relatório é um campo obrigatório")]
    public string Titulo { get; set; }

    public CondicaoClimaticaEnum CondicoesClimaticas { get; set; }

    public List<MembroEquipeDTO>? Equipe { get; set; }

    public List<AtividadeRegistroDTO>? Atividades { get; set; }

    public List<MaterialUtilizadoDTO>? Materiais { get; set; } // Alterado para List<MaterialUtilizadoDTO>

    // Ocorrências e Ambiente
    public string? Ocorrencias { get; set; }

    public List<FotoRegistroMetadataDto>? Fotos { get; set; }
    //public List<string>? UrlsDocumentos { get; set; }
}