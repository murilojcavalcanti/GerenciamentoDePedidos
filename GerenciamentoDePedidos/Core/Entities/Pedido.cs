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
        public Pedido() { }

        public Pedido(int id, int idPessoa, List<int> idProdutos, EnumFormaPagamento formaPagamento, EnumStatusVenda statusVenda = EnumStatusVenda.Pendente)
        {
            Id = id;
            IdPessoa = idPessoa;
            IdProdutos = idProdutos;
            DataVenda = DateTime.Now;
            FormaPagamento = formaPagamento;
            StatusVenda = statusVenda;
            CalcularValorTotal();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public int IdPessoa { get; set; }
        [Required]
        public List<int> IdProdutos { get; set; }
        [Required]
        public DateTime DataVenda { get; set; }
        [Required]
        public EnumFormaPagamento FormaPagamento { get; set; }
        [Required]
        public EnumStatusVenda StatusVenda { get; set; }

        public decimal ValorTotal { get; set; }
        // ValorTotal é calculado sob demanda
        public decimal CalcularValorTotal()
        {
            var todosProdutos = DatabaseRepository.CarregarProdutos().Where(p=>IdProdutos.Contains(p.Id));
            if (IdProdutos == null || todosProdutos == null)
                return 0;

            
            return todosProdutos
                .Where(p => IdProdutos.Contains(p.Id))
                .Sum(p => p.Valor);
        }

        public void AdicionarProduto(int idProduto)
        {
            if (!IdProdutos.Contains(idProduto))
            {
                IdProdutos.Add(idProduto);
            }
        }

        public void RemoverProduto(int idProduto)
        {
            if (IdProdutos.Contains(idProduto))
            {
                IdProdutos.Remove(idProduto);
            }
        }

        public void Update(Pedido novoPedido)
        {
            if (novoPedido == null)
                throw new ArgumentNullException(nameof(novoPedido));

            IdPessoa = novoPedido.IdPessoa;
            IdProdutos = new List<int>(novoPedido.IdProdutos ?? IdProdutos);
            DataVenda = novoPedido.DataVenda;
            FormaPagamento = novoPedido.FormaPagamento;
            StatusVenda = novoPedido.StatusVenda;
            CalcularValorTotal();
        }
    }
}
