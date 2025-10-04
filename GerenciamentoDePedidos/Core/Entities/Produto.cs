using System.ComponentModel.DataAnnotations;

namespace GerenciamentoDePedidos.Core.Entities
{
    public class Produto
    {
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string Codigo { get; set; }
        [Required]
        public decimal Valor { get; set; }
    }
}
