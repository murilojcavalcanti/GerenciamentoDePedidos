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
        private List<Pessoa> pessoas;
        private List<Produto> produtos;

        public EditarPedidoWindow(int idPedido)
        {
            InitializeComponent();
            pedido = DatabaseRepository.CarregarPedidos().Where(p=>p.Id == idPedido).First();
            pessoas = DatabaseRepository.CarregarPessoas();
            produtos = DatabaseRepository.CarregarProdutos();

            CarregarCampos();
        }

        private void CarregarCampos()
        {
            PessoaComboBox.ItemsSource = pessoas;
            PessoaComboBox.DisplayMemberPath = "Nome";
            PessoaComboBox.SelectedValuePath = "Id";
            PessoaComboBox.SelectedValue = pedido.IdPessoa;

            FormaPagamentoComboBox.ItemsSource = Enum.GetValues(typeof(EnumFormaPagamento));
            FormaPagamentoComboBox.SelectedItem = pedido.FormaPagamento;

            StatusComboBox.ItemsSource = Enum.GetValues(typeof(EnumStatusVenda));
            StatusComboBox.SelectedItem = pedido.StatusVenda;

            ProdutosListBox.ItemsSource = produtos;
            ProdutosListBox.DisplayMemberPath = "Nome";
            ProdutosListBox.SelectedValuePath = "Id";
            foreach (var idProd in pedido.IdProdutos)
            {
                var item = produtos.FirstOrDefault(p => p.Id == idProd);
                if (item != null)
                    ProdutosListBox.SelectedItems.Add(item);
            }

            ValorTotalTextBox.Text = pedido.CalcularValorTotal().ToString("C");
            ProdutosListBox.SelectionChanged += ProdutosListBox_SelectionChanged;
        }

        private void ProdutosListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var idsSelecionados = ProdutosListBox.SelectedItems.Cast<Produto>().Select(p => p.Id).ToList();
            pedido.IdProdutos = idsSelecionados;
            ValorTotalTextBox.Text = pedido.CalcularValorTotal().ToString("C");
        }

        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (PessoaComboBox.SelectedValue == null || ProdutosListBox.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selecione uma pessoa e pelo menos um produto.");
                return;
            }

            pedido.IdPessoa = (int)PessoaComboBox.SelectedValue;
            pedido.FormaPagamento = (EnumFormaPagamento)FormaPagamentoComboBox.SelectedItem;
            pedido.StatusVenda = (EnumStatusVenda)StatusComboBox.SelectedItem;
            pedido.IdProdutos = ProdutosListBox.SelectedItems.Cast<Produto>().Select(p => p.Id).ToList();
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
