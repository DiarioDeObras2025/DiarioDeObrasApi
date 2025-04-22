using System.ComponentModel.DataAnnotations;

namespace DiarioObras.DTOs.RegistroDiarioDTOs
{
    public class AtividadeRegistroDTO
    {
        [Required]
        public string Descricao { get; set; }
    }
}
