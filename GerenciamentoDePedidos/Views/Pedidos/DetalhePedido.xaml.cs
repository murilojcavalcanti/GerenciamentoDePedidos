using GerenciamentoDePedidos.Infra;
using System.Linq;
using System.Windows;

namespace GerenciamentoDePedidos.Views.Pedidos
{
    /// <summary>
    /// Lógica interna para DetalhePedido.xaml
    /// </summary>
    public partial class DetalhePedido : Window
    {
        public DetalhePedido(int idPedido)
        {
            InitializeComponent();
            CarregarDetalhes(idPedido);
        }

        private void CarregarDetalhes(int idPedido)
        {
            var pedido = DatabaseRepository.CarregarPedidos().FirstOrDefault(p => p.Id == idPedido);
            if (pedido == null)
            {
                MessageBox.Show("Pedido não encontrado.");
                Close();
                return;
            }

            var pessoa = DatabaseRepository.CarregarPessoas().FirstOrDefault(p => p.Id == pedido.IdPessoa);

            TxtId.Text = pedido.Id.ToString();
            TxtNomePessoa.Text = pessoa != null ? pessoa.Nome : "N/A";
            TxtDataVenda.Text = pedido.DataVenda.ToString("dd/MM/yyyy");
            TxtFormaPagamento.Text = pedido.FormaPagamento.ToString();
            TxtStatusVenda.Text = pedido.StatusVenda.ToString();
            TxtValorTotal.Text = pedido.CalcularValorTotal().ToString("C");

            var todosProdutos = DatabaseRepository.CarregarProdutos();

            var produtosPedido = pedido.Produtos
                .Select(pp =>
                {
                    var produto = todosProdutos.FirstOrDefault(p => p.Id == pp.IdProduto);

                    if (produto == null)
                        return null;

                    return new
                    {
                        Nome = produto.Nome,
                        Quantidade = pp.Quantidade,
                        PrecoUnitario = produto.Valor,
                        Subtotal = produto.Valor * pp.Quantidade
                    };
                })
                .Where(p => p != null).ToList();

            ProdutosListView.ItemsSource = produtosPedido;
        }

        private void BtnFechar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
