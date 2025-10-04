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
    /// Lógica interna para AdicionarPessoa.xaml
    /// </summary>
    public partial class AdicionarPessoa : Window
    {
        public AdicionarPessoa()
        {
            InitializeComponent();
        }
        private void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtNome.Text) ||
                string.IsNullOrWhiteSpace(TxtCpf.Text) ||
                string.IsNullOrWhiteSpace(TxtEndereco.Text))
            {
                MessageBox.Show("Preencha todos os campos.");
                return;
            }

            // Gera novo Id
            var pessoas = DatabaseRepository.CarregarPessoas();
            int novoId = pessoas.Any() ? pessoas.Max(p => p.Id) + 1 : 1;

            var pessoa = new Pessoa
            {
                Id = novoId,
                Nome = TxtNome.Text,
                CPF = TxtCpf.Text,
                Endereco = TxtEndereco.Text
            };

            DatabaseRepository.AdicionarPessoa(pessoa);

            MessageBox.Show("Pessoa salva com sucesso!");
            this.Close();
        }
    }
}
