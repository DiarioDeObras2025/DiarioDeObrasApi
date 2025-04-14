using System.ComponentModel.DataAnnotations;

namespace DiarioObras.DTOs.RegistroDiarioDTOs;

public class MaterialUtilizadoDTO
{
    [StringLength(100, ErrorMessage = "O nome do material deve ter no máximo 100 caracteres")]
    public string Nome { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "A quantidade deve ser um valor positivo")]
    public decimal Quantidade { get; set; }

    [StringLength(20, ErrorMessage = "A unidade deve ter no máximo 20 caracteres")]
    public string? Unidade { get; set; }
}