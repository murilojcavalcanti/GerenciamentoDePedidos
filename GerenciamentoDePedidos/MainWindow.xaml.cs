using GerenciamentoDePedidos.Views;
using System;
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
        }

        


        private void BtnAdicionarPedido_Click(object sender, RoutedEventArgs e)
        {/*
            var janela = new AdicionarPedidoWindow();
            janela.ShowDialog();*/
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
