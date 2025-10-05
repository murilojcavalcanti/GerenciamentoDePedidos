using GerenciamentoDePedidos.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciamentoDePedidos.Core.Entities
{
    public class Pedido
    {
        public Pedido()
        {
            Produtos = new List<PedidoProduto>();
        }

        public Pedido(int id, int idPessoa, List<PedidoProduto> produtos, EnumFormaPagamento formaPagamento, EnumStatusVenda statusVenda = EnumStatusVenda.Pendente)
        {
            Id = id;
            IdPessoa = idPessoa;
            Produtos = produtos ?? new List<PedidoProduto>();
            DataVenda = DateTime.Now;
            FormaPagamento = formaPagamento;
            StatusVenda = statusVenda;
            ValorTotal = CalcularValorTotal();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public int IdPessoa { get; set; }
        [Required]
        public List<PedidoProduto> Produtos { get; set; }
        [Required]
        public DateTime DataVenda { get; set; }
        [Required]
        public EnumFormaPagamento FormaPagamento { get; set; }
        [Required]
        public EnumStatusVenda StatusVenda { get; set; }

        public decimal ValorTotal { get; set; }

        public decimal CalcularValorTotal()
        {
            if (Produtos == null || Produtos.Count == 0)
                return 0;

            var todosProdutos = DatabaseRepository.CarregarProdutos();
            decimal total = 0;

            foreach (var pedidoProduto in Produtos)
            {
                var produto = todosProdutos.FirstOrDefault(p => p.Id == pedidoProduto.IdProduto);
                if (produto != null)
                {
                    total += produto.Valor * pedidoProduto.Quantidade;
                }
            }
            return total;
        }

        public void AdicionarProduto(PedidoProduto pedidoProduto)
        {
            if (Produtos == null)
                Produtos = new List<PedidoProduto>();

            var existente = Produtos.FirstOrDefault(p => p.IdProduto == pedidoProduto.IdProduto);
            if (existente != null)
            {
                existente.Quantidade += pedidoProduto.Quantidade;
            }
            else
            {
                Produtos.Add(pedidoProduto);
            }
        }

        public void RemoverProduto(int idProduto)
        {
            var produto = Produtos?.FirstOrDefault(p => p.IdProduto == idProduto);
            if (produto != null)
            {
                Produtos.Remove(produto);
            }
        }

        public void Update(Pedido novoPedido)
        {
            if (novoPedido == null)
                throw new ArgumentNullException(nameof(novoPedido));

            IdPessoa = novoPedido.IdPessoa;
            Produtos = new List<PedidoProduto>(novoPedido.Produtos ?? new List<PedidoProduto>());
            DataVenda = novoPedido.DataVenda;
            FormaPagamento = novoPedido.FormaPagamento;
            StatusVenda = novoPedido.StatusVenda;
            ValorTotal = CalcularValorTotal();
        }
    }
}
