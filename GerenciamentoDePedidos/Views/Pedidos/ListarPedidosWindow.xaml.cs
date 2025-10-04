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
using Sys7tem.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GerenciamentoDePedidos.Views.Pedidos
{
    /// <summary>
    /// Lógica interna para ListarPedidosWindow.xaml
    /// </summary>
    public partial class ListarPedidosWindow : Window
    {

        private List<Pedido> pedidos;
        private List<Pessoa> pessoas;

        public ListarPedidosWindow()
        {
            InitializeComponent();
            CarregarFiltros();
            CarregarPedidos();
        }

        private void CarregarFiltros()
        {
            FiltroStatus.ItemsSource = System.Enum.GetValues(typeof(EnumStatusVenda));
            FiltroStatus.SelectedIndex = -1;
        }

        private void CarregarPedidos()
        {
            pedidos = DatabaseRepository.CarregarPedidos();
            pessoas = DatabaseRepository.CarregarPessoas();

            // Adiciona o nome da pessoa para exibição
            var pedidosView = pedidos.Select(p => new
            {
                p.Id,
                NomePessoa = pessoas.FirstOrDefault(x => x.Id == p.IdPessoa)?.Nome ?? "",
                p.DataVenda,
                p.ValorTotal,
                p.StatusVenda
            }).ToList();

            PedidosDataGrid.ItemsSource = pedidosView;
        }

        private void BtnAdicionarPedido_Click(object sender, RoutedEventArgs e)
        {
            var janela = new AdicionarPedidoWindow();
            janela.ShowDialog();
            CarregarPedidos();
        }

        private void BtnFiltrar_Click(object sender, RoutedEventArgs e)
        {
            var filtroNome = FiltroPessoa.Text?.ToLower() ?? "";
            var filtroStatus = FiltroStatus.SelectedItem as EnumStatusVenda?;

            var pedidosFiltrados = pedidos.Where(p =>
                (string.IsNullOrEmpty(filtroNome) || (pessoas.FirstOrDefault(x => x.Id == p.IdPessoa)?.Nome.ToLower().Contains(filtroNome) ?? false)) &&
                (!filtroStatus.HasValue || p.StatusVenda == filtroStatus.Value)
            ).Select(p => new
            {
                p.Id,
                NomePessoa = pessoas.FirstOrDefault(x => x.Id == p.IdPessoa)?.Nome ?? "",
                p.DataVenda,
                p.ValorTotal,
                p.StatusVenda
            }).ToList();

            PedidosDataGrid.ItemsSource = pedidosFiltrados;
        }

        private void BtnEditarPedido_Click(object sender, RoutedEventArgs e)
        {
            // Obtém o objeto anônimo do DataContext
            var pedidoView = ((FrameworkElement)sender).DataContext;
            if (pedidoView == null)
            {
                MessageBox.Show("Nenhum pedido selecionado.");
                return;
            }

            // Usa reflexão para obter o Id do pedido selecionado
            var idProperty = pedidoView.GetType().GetProperty("Id");
            if (idProperty == null)
            {
                MessageBox.Show("Erro ao obter o pedido selecionado.");
                return;
            }
            int idPedido = (int)idProperty.GetValue(pedidoView);

            // Abre a janela de edição passando apenas o id do pedido
            var janela = new EditarPedidoWindow(idPedido);
            bool? resultado = janela.ShowDialog();

            if (resultado == true)
            {
                // Recarrega os pedidos após edição
                CarregarPedidos();
            }
        }

        private void BtnExcluirPedido_Click(object sender, RoutedEventArgs e)
        {
            var pedidoView = ((FrameworkElement)sender).DataContext;
            if (pedidoView == null)
            {
                MessageBox.Show("Nenhum pedido selecionado.");
                return;
            }

            // Usa reflexão para obter o Id do pedido selecionado
            var idProperty = pedidoView.GetType().GetProperty("Id");
            if (idProperty == null)
            {
                MessageBox.Show("Erro ao obter o pedido selecionado.");
                return;
            }
            int idPedido = (int)idProperty.GetValue(pedidoView);

            var resultado = MessageBox.Show("Tem certeza que deseja excluir este pedido?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (resultado != MessageBoxResult.Yes)
                return;

            // Remove o pedido da base de dados
            var pedidoParaExcluir = pedidos.FirstOrDefault(p => p.Id == idPedido);
            if (pedidoParaExcluir == null)
            {
                MessageBox.Show("Pedido não encontrado.");
                return;
            }

            // Remove do repositório e atualiza a lista
            DatabaseRepository.ExcluirPedido(idPedido);
            CarregarPedidos();
            MessageBox.Show("Pedido excluído com sucesso.");
        }
    }
}
