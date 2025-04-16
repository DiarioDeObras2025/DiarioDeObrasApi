namespace DiarioObras.DTOs.RegistroDiarioDTOs
{
    public class RegistroDiarioResumoDTO
    {
        public int Id { get; set; }
        public DateTime? Data { get; set; }
        public string Titulo { get; set; }
        public int ObraId { get; set; }
        public string NomeObra { get; set; } // <- novo campo
    }

}
