using GerenciamentoDePedidos.Core.Entities;
using GerenciamentoDePedidos.Infra;
using GerenciamentoDePedidos.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GerenciamentoDePedidos.Views.Pedidos
{
    /// <summary>
    /// Lógica interna para AdicionarPedidoWindow.xaml
    /// </summary>
    public partial class AdicionarPedidoWindow : Window
    {
        private List<Pessoa> pessoas;
        private List<Produto> produtos;
        private List<ProdutoPedidoViewModel> produtosDoPedido = new List<ProdutoPedidoViewModel>();

        public AdicionarPedidoWindow()
        {
            InitializeComponent();
            pessoas = DatabaseRepository.CarregarPessoas();
            produtos = DatabaseRepository.CarregarProdutos();

            cbCliente.ItemsSource = pessoas;
            cbCliente.DisplayMemberPath = "Nome";
            cbCliente.SelectedValuePath = "Id";

            cbProduto.ItemsSource = produtos;
            cbProduto.DisplayMemberPath = "Nome";
            cbProduto.SelectedValuePath = "Id";

            dgProdutos.ItemsSource = produtosDoPedido;
        }

        private void BtnAdicionarProduto_Click(object sender, RoutedEventArgs e)
        {
            if (cbProduto.SelectedItem is Produto produto && int.TryParse(txtQuantidade.Text, out int quantidade) && quantidade > 0)
            {
                var existente = produtosDoPedido.FirstOrDefault(p => p.IdProduto == produto.Id);
                if (existente != null)
                {
                    existente.Quantidade += quantidade;
                }
                else
                {
                    produtosDoPedido.Add(new ProdutoPedidoViewModel
                    {
                        IdProduto = produto.Id,
                        NomeProduto = produto.Nome,
                        Quantidade = quantidade
                    });
                }

                dgProdutos.Items.Refresh();
                AtualizarTotal(); // <<<<< Atualiza total
            }
            else
            {
                MessageBox.Show("Selecione um produto e informe uma quantidade válida.");
            }
        }

        private void BtnRemoverProduto_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement)?.DataContext as ProdutoPedidoViewModel;
            if (item != null)
            {
                produtosDoPedido.Remove(item);
                dgProdutos.Items.Refresh();
                AtualizarTotal(); // <<<<< Atualiza total
            }
        }

        private void AtualizarTotal()
        {
            var listaPedidoProduto = produtosDoPedido.Select(p => new PedidoProduto
            {
                IdProduto = p.IdProduto,
                Quantidade = p.Quantidade
            }).ToList();

            var pedidoTemp = new Pedido
            {
                IdPessoa = cbCliente.SelectedValue is int idPessoa ? idPessoa : 0,
                Produtos = listaPedidoProduto,
                DataVenda = DateTime.Now,
                FormaPagamento = EnumFormaPagamento.Dinheiro,
                StatusVenda = EnumStatusVenda.Pendente
            };

            pedidoTemp.ValorTotal = pedidoTemp.CalcularValorTotal();
            txtValorTotal.Text = $"R$ {pedidoTemp.ValorTotal:N2}";
        }

        private void BtnSalvarPedido_Click(object sender, RoutedEventArgs e)
        {
            if (cbCliente.SelectedItem is Pessoa pessoa && produtosDoPedido.Any())
            {
                // Monta a lista de PedidoProduto a partir dos produtos selecionados
                var listaPedidoProduto = produtosDoPedido.Select(p => new PedidoProduto
                {
                    IdProduto = p.IdProduto,
                    Quantidade = p.Quantidade
                }).ToList();

                var pedido = new Pedido
                {
                    IdPessoa = pessoa.Id,
                    Produtos = listaPedidoProduto,
                    DataVenda = DateTime.Now,
                    FormaPagamento = EnumFormaPagamento.Dinheiro, // ajuste conforme necessário
                    StatusVenda = EnumStatusVenda.Pendente
                };
                pedido.ValorTotal = pedido.CalcularValorTotal();
                var pedidos = DatabaseRepository.CarregarPedidos();
                pedido.Id = pedidos.Any() ? pedidos.Max(p => p.Id) + 1 : 1;

                DatabaseRepository.AdicionarPedido(pedido);
                MessageBox.Show("Pedido salvo com sucesso!");
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Selecione um cliente e adicione pelo menos um produto.");
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
