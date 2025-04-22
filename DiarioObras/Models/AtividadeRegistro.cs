using System.ComponentModel.DataAnnotations;

namespace DiarioObras.Models
{
    public class AtividadeRegistro
    {
        public int Id { get; set; }

        [Required]
        public string Descricao { get; set; }
        public int RegistroDiarioId { get; set; }
        public RegistroDiario? RegistroDiario { get; set; }
    }
}
