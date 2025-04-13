using System.ComponentModel.DataAnnotations;

namespace DiarioObras.Models
{
    public class MaterialUtilizado
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        public int RegistroDiarioId { get; set; }
        public RegistroDiario? RegistroDiario { get; set; }
    }
}
