using System.ComponentModel.DataAnnotations;
using DiarioObras.Models;

namespace DiarioObras.DTOs.Financeiro
{
    public class CustoObraDTO
    {
        [Required]
        public int ObraId { get; set; }

        [Required]
        public CategoriaCusto Categoria { get; set; }

        [Required]
        [StringLength(100)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero")]
        public decimal Valor { get; set; }

        [Required]
        public DateTime Data { get; set; }
    }
}
