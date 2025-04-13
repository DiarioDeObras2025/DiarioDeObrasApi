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

    public TipoEnum Tipo { get; set; }

    [Required]
    [StringLength(50)]
    public string? Localizacao { get; set; }
}

public enum TipoEnum
{
    Geral,
    Progresso,
    Problema,
    Seguranca,
    Material,
    Equipamento,
    Maquinario,
    Estrutural,
    Acabamento,
    Reuniao,
    Inspecao,
    Documentacao,
    Emergencia,
    AntesDepois,
    ClimaTempo
}