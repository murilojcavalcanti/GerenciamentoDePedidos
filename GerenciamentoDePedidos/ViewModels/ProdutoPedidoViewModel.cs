using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GerenciamentoDePedidos.ViewModels
{
    public class ProdutoPedidoViewModel
    {
        public int IdProduto { get; set; }
        public string NomeProduto { get; set; }
        public int Quantidade { get; set; }
    }
}
