using GerenciamentoDePedidos.Infra;
using System.Windows;

namespace GerenciamentoDePedidos.Views
{
    /// <summary>
    /// Lógica interna para ListarPessoas.xaml
    /// </summary>
    public partial class ListarPessoasWindow : Window
    {
        public ListarPessoasWindow()
        {
            InitializeComponent();
            PessoasDataGrid.ItemsSource = DatabaseRepository.CarregarPessoas();
        }
        private void BtnAdicionarPessoa_Click(object sender, RoutedEventArgs e)
        {
            var janela = new AdicionarPessoa();
            janela.ShowDialog();
        
        }
    }
}
