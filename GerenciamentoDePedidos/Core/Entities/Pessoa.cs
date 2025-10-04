using System.ComponentModel.DataAnnotations;

namespace GerenciamentoDePedidos.Core.Entities
{
    public class Pessoa
    {

        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public string CPF { get; set; }
        public string Endereco { get; set; }
    }
}
