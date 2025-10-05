namespace GerenciamentoDePedidos.Core.Entities
{
    public class PedidoProduto
    {
        public int IdPedido { get; set; }
        public int IdProduto { get; set; }
        public string NomeProduto { get; set; }
        public int Quantidade { get; set; }
    }
}
