using System.ComponentModel.DataAnnotations;

namespace DiarioObras.Models
{
    public class DocumentoRegistro
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string NomeArquivo { get; set; }

        [Required]
        [StringLength(50)]
        public string Tipo { get; set; } // "pdf", "doc", etc.

        [Required]
        public string CaminhoArquivo { get; set; }

        public int RegistroDiarioId { get; set; }
        public RegistroDiario? RegistroDiario { get; set; }
    }
}
