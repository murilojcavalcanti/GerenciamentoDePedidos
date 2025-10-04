using System.Collections.Generic;

namespace GerenciamentoDePedidos.Core.Entities
{
    public class Database
    {
        public List<Produto> Produtos { get; set; } = new List<Produto>();
        public List<Pessoa> Pessoas { get; set; } = new List<Pessoa>();
        public List<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}