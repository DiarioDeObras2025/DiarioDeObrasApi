using System.ComponentModel.DataAnnotations;

namespace DiarioObras.DTOs.RegistroDiarioDTOs;

public class MembroEquipeDTO
{
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
    public string Nome { get; set; }

    [StringLength(50, ErrorMessage = "O cargo deve ter no máximo 50 caracteres")]
    public string Cargo { get; set; }

    [StringLength(200, ErrorMessage = "A observação deve ter no máximo 200 caracteres")]
    public string? Observacao { get; set; }

    public bool Terceirizado { get; set; }
}