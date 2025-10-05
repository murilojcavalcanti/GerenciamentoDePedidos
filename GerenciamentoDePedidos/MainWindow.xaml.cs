using GerenciamentoDePedidos.Infra;
using GerenciamentoDePedidos.Views;
using GerenciamentoDePedidos.Views.Pedidos;
using System;
using System.Linq;
using System.Windows;

namespace GerenciamentoDePedidos
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CarregarUltimosPedidos();
        }

        private void CarregarUltimosPedidos()
        {
            // Exemplo de busca dos 5 últimos pedidos (ajuste conforme sua fonte de dados)
            // Supondo que exista um repositório ou serviço para buscar pedidos
            try
            {
                // Substitua pelo seu método real de acesso a dados
                var pedidos = DatabaseRepository.CarregarPedidos().OrderByDescending(p=>p.DataVenda).Take(5);

                // Exemplo: exibir em um ListBox chamado lstUltimosPedidos (adicione no XAML)
                if (this.FindName("lstUltimosPedidos") is System.Windows.Controls.ListBox listBox)
                {
                    listBox.ItemsSource = pedidos;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar os últimos pedidos: " + ex.Message);
            }
        }

        private void BtnListarPedidos_Click(object sender, RoutedEventArgs e)
        {
            var janela = new ListarPedidosWindow();
            janela.ShowDialog();
        }
        private void BtnListarPessoas_Click(object sender, RoutedEventArgs e)
        {
            var janela = new Views.ListarPessoasWindow();
            janela.ShowDialog();
        }

        private void BtnListarProdutos_Click(object sender, RoutedEventArgs e)
        {
            var janela = new Views.ListarProdutosWindow();
            janela.ShowDialog();
        }
    }
}
