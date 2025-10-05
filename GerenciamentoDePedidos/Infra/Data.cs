using GerenciamentoDePedidos.Core.Entities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GerenciamentoDePedidos.Infra
{
    public static class DatabaseRepository
    {
        private static readonly string FilePath = "database.json";

        private static Database CarregarDatabase()
        {
            if (!File.Exists(FilePath))
                return new Database();

            var json = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<Database>(json) ?? new Database();
        }

        private static void SalvarDatabase(Database db)
        {
            var json = JsonConvert.SerializeObject(db, Formatting.Indented);
            File.WriteAllText(FilePath, json);
        }

        // Métodos para Produto
        public static List<Produto> CarregarProdutos()
        {
            return CarregarDatabase().Produtos;
        }

        public static void AdicionarProduto(Produto produto)
        {
            var db = CarregarDatabase();
            db.Produtos.Add(produto);
            SalvarDatabase(db);
        }

        // Métodos para Pessoa
        public static List<Pessoa> CarregarPessoas()
        {
            return CarregarDatabase().Pessoas;
        }

        public static void AdicionarPessoa(Pessoa pessoa)
        {
            var db = CarregarDatabase();
            db.Pessoas.Add(pessoa);
            SalvarDatabase(db);
        }

        // Métodos para Pedido
        public static List<Pedido> CarregarPedidos()
        {
            return CarregarDatabase().Pedidos;
        }

        public static void AdicionarPedido(Pedido pedido)
        {
            var db = CarregarDatabase();
            db.Pedidos.Add(pedido);
            SalvarDatabase(db);
        }
        public static void AtualizarPedido(Pedido pedidoAtualizado)
        {
            // Carrega o banco completo
            var db = CarregarDatabase();

            // Localiza o pedido dentro do banco atual
            var pedidoExistente = db.Pedidos.FirstOrDefault(p => p.Id == pedidoAtualizado.Id);

            if (pedidoExistente != null)
            {
                // Atualiza os dados
                pedidoExistente.Update(pedidoAtualizado);
            }

            // Salva o mesmo objeto que foi alterado
            SalvarDatabase(db);
        }
        public static void RemoverPedido(int idPedido)
        {
            // Exemplo simples: substitua pelo seu mecanismo real de persistência
            var db = CarregarDatabase();
            var pedido = CarregarPedidos().First(p => p.Id == idPedido);
            if (pedido != null)
            {
                db.Pedidos.Remove(pedido);
                SalvarDatabase(db);
            }
        }
    }
}
