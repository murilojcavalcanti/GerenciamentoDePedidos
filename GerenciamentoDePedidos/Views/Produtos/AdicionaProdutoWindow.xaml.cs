using GerenciamentoDePedidos.Core.Entities;
using GerenciamentoDePedidos.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GerenciamentoDePedidos.Views
{
    /// <summary>
    /// Lógica interna para AdicionaProdutoWindow.xaml
    /// </summary>
    public partial class AdicionaProdutoWindow : Window
    {
        public AdicionaProdutoWindow()
        {
            InitializeComponent();
        }
        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            // Validação simples
            if (string.IsNullOrWhiteSpace(TxtNome.Text) || string.IsNullOrWhiteSpace(TxtValor.Text))
            {
                MessageBox.Show("Preencha todos os campos.");
                return;
            }

            if (!decimal.TryParse(TxtValor.Text, out decimal valor) || valor < 0)
            {
                MessageBox.Show("Valor inválido.");
                return;
            }

            // Gerar novo Id
            var produtos = DatabaseRepository.CarregarProdutos();
            int novoId = produtos.Any() ? produtos.Max(p => p.Id) + 1 : 1;

            // Criar e salvar produto
            var produto = new Produto
            {
                Id = novoId,
                Nome = TxtNome.Text,
                Valor = valor
            };

            DatabaseRepository.AdicionarProduto(produto);

            MessageBox.Show("Produto salvo com sucesso!");
            this.Close();
        }
    }
}
