using GerenciamentoDePedidos.Infra;
using System.Windows;

namespace GerenciamentoDePedidos.Views
{
    /// <summary>
    /// Lógica interna para ListarProdutos.xaml
    /// </summary>

    public partial class ListarProdutosWindow : Window
    {
        public ListarProdutosWindow()
        {
            InitializeComponent();
            ProdutosDataGrid.ItemsSource = DatabaseRepository.CarregarProdutos();
        }
        private void BtnAdicionarProduto_Click(object sender, RoutedEventArgs e)
        {
            var janela = new AdicionaProdutoWindow();
            janela.ShowDialog();
        }

    }

}
