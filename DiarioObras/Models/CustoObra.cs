using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiarioObras.Models
{
    public class CustoObra
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ObraId { get; set; }

        [Required]
        public CategoriaCusto Categoria { get; set; }

        [StringLength(100)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Valor { get; set; }

        public DateTime Data { get; set; } = DateTime.Now;

        public Obra Obra { get; set; }
    }

    public enum CategoriaCusto
    {
        PagamentoFuncionario = 1,
        Alimentacao = 2,
        CompraMateriais = 3,
        AluguelEquipamentos = 4,
        Transporte = 5,
        DespesasGerais = 6,
        ServicosTerceirizados = 7,
        Imprevistos = 8
    }
}
