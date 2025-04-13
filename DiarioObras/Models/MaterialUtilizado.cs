using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiarioObras.Models
{
    public class MaterialUtilizado
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantidade { get; set; }

        [StringLength(20)]
        public string? Unidade { get; set; }

        public int RegistroDiarioId { get; set; }
        public RegistroDiario? RegistroDiario { get; set; }
    }
}
