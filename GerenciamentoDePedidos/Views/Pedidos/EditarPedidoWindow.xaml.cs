using GerenciamentoDePedidos.Core.Entities;
using GerenciamentoDePedidos.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace GerenciamentoDePedidos.Views.Pedidos
{
    /// <summary>
    /// Lógica interna para EditarPedidoWindow.xaml
    /// </summary>

    public partial class EditarPedidoWindow : Window
    {
        private Pedido pedido;
        private List<Produto> produtos;

        public EditarPedidoWindow(int idPedido)
        {
            InitializeComponent();

            pedido = DatabaseRepository.CarregarPedidos().First(p => p.Id == idPedido);
            produtos = DatabaseRepository.CarregarProdutos();
            ProdutoComboBox.ItemsSource = produtos;

            CarregarCampos();
        }

        private void CarregarCampos()
        {
            // Pessoa fixa (não pode ser alterada)
            PessoaTextBlock.Text = DatabaseRepository.CarregarPessoas()
                .First(p => p.Id == pedido.IdPessoa).Nome;

            FormaPagamentoComboBox.ItemsSource = Enum.GetValues(typeof(EnumFormaPagamento));
            FormaPagamentoComboBox.SelectedItem = pedido.FormaPagamento;

            StatusComboBox.ItemsSource = Enum.GetValues(typeof(EnumStatusVenda));
            StatusComboBox.SelectedItem = pedido.StatusVenda;

            AtualizarListaProdutos();
            AtualizarValorTotal();
        }

        private void AtualizarListaProdutos()
        {
            // 1️⃣ Carrega todos os produtos do sistema
            var todosProdutos = DatabaseRepository.CarregarProdutos();
            // 2️⃣ Monta uma lista combinando os dados do pedido e dos produtos
            var produtosPedido = pedido.Produtos
                .Select(pp =>
                {
                    // Busca o produto correspondente pelo Id
                    var produto = todosProdutos.FirstOrDefault(p => p.Id == pp.IdProduto);

                    // Evita erro caso algum produto tenha sido removido do banco
                    if (produto == null)
                        return null;

                    // Retorna um objeto anônimo com todos os dados que queremos mostrar
                    return new
                    {
                        Nome = produto.Nome,
                        Quantidade = pp.Quantidade,
                        PrecoUnitario = produto.Valor,
                        Subtotal = produto.Valor * pp.Quantidade
                    };
                })
                .Where(p => p != null) // Remove nulos, se algum produto não for encontrado
                .ToList();

            // 3️⃣ Exibe na interface (por exemplo, ListBox ou DataGrid)
            ProdutosListBox.ItemsSource = null;
            ProdutosListBox.ItemsSource = produtosPedido;
        }

        private void AtualizarValorTotal()
        {
            ValorTotalTextBox.Text = pedido.CalcularValorTotal().ToString("C2");
        }

        private void BtnAdicionarProduto_Click(object sender, RoutedEventArgs e)
        {
            if (ProdutoComboBox.SelectedItem == null)
            {
                MessageBox.Show("Selecione um produto para adicionar.");
                return;
            }

            var produtoSelecionado = (Produto)ProdutoComboBox.SelectedItem;

            var existente = pedido.Produtos.FirstOrDefault(p => p.IdProduto == produtoSelecionado.Id);
            if (existente != null)
            {
                existente.Quantidade++;
            }
            else
            {
                pedido.Produtos.Add(new PedidoProduto
                {
                    IdProduto = produtoSelecionado.Id,
                    NomeProduto = produtoSelecionado.Nome,
                    Quantidade = 1
                });
            }

            AtualizarListaProdutos();
            AtualizarValorTotal();
        }

        private void BtnRemoverProduto_Click(object sender, RoutedEventArgs e)
        {
            if (ProdutosListBox.SelectedItem == null)
            {
                MessageBox.Show("Selecione um produto para remover.");
                return;
            }

            dynamic produtoSelecionado = ProdutosListBox.SelectedItem;

            // Pega o nome ou o ID (dependendo do que você mostra)
            var nome = produtoSelecionado.Nome;

            // Encontra no pedido real
            var todosProdutos = DatabaseRepository.CarregarProdutos();
            var produto = todosProdutos.FirstOrDefault(p => p.Nome == nome);

            if (produto != null)
            {
                pedido.RemoverProduto(produto.Id);
                AtualizarListaProdutos();
                AtualizarValorTotal();
            }
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (pedido.Produtos == null || pedido.Produtos.Count == 0)
            {
                MessageBox.Show("Adicione ao menos um produto ao pedido.");
                return;
            }

            pedido.FormaPagamento = (EnumFormaPagamento)FormaPagamentoComboBox.SelectedItem;
            pedido.StatusVenda = (EnumStatusVenda)StatusComboBox.SelectedItem;
            pedido.ValorTotal = pedido.CalcularValorTotal();

            DatabaseRepository.AtualizarPedido(pedido);

            DialogResult = true;
            Close();
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
