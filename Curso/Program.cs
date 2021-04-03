using Curso.Domain;
using Curso.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Curso
{
    class Program
    {
        static void Main(string[] args)
        {

            //executando migração na primeira execução do programa
            //não é indicado para o ambiente de produção, pois vocÊ pode gerar concorrencia

            //using var db = new Data.ApplicationContext();

            //db.Database.Migrate();

            //InserirDados();
            //ConsultarDados();
            //CadastrarPedido();
            //ConsultarPedidoCarregamentoAdiantado();
            //AtualizarDados();
            RemoverRegistros();
        }

        private static void RemoverRegistros()
        {
            using var db = new Data.ApplicationContext();
            //var clienteRemover = db.Clientes.Find(4);
            var clienteRemover = new Cliente { Id = 3 }; //dessa forma ela só executa um comando no banco de dados

            db.Entry(clienteRemover).State = EntityState.Deleted;
            //db.Remove(clienteRemover);
            db.SaveChanges();
        }

        public static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();
            var cliente = db.Clientes.Find(1);
            cliente.Nome = "Cliente alterado 2";

            //quando usa o update ele atualiza todos os dados da entidade, 
            //quando você não utilzia a consulta de alteração ficar menos e mais performaartica
            //db.Update(cliente);

            db.SaveChanges();

        }
        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();
            var pedidos = 
                db.Pedidos
                .Include(p=> p.Itens)
                .ThenInclude(p=>p.Produto)
                .ToList();
            Console.WriteLine(pedidos.Count());
        }
        public static void CadastrarPedido()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();
            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Itens = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10
                    }
                }

            };

            db.Pedidos.Add(pedido);

            db.SaveChanges();
        }
        public static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();
            //var consultaPorSintaxe = (from c in db.Clientes where c.Id>0 select c).ToList();
            var consultaPorMetodo = db.Clientes.Where(p => p.Id > 0).ToList();

            foreach(var cliente in consultaPorMetodo)
            {
                Console.WriteLine($"Consultando Cliente: {cliente.Nome}");
                db.Clientes.Find(cliente.Id);
            }
        }
        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto Teste",
                CodigoBarras = "1234567897",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };
            var listaClientes = new List<Cliente>
            {
                new Cliente
                {
                Nome = "Amanda Rodrigues2",
                CEP = "79290000",
                Cidade = "Bonito",
                Estado = "MS",
                Telefone = "991646461"
                },
                new Cliente
                {
                Nome = "Amanda Rodrigues3",
                CEP = "79290000",
                Cidade = "Bonito",
                Estado = "MS",
                Telefone = "991646461"
                },
                new Cliente{
                Nome = "Amanda Rodrigues4",
                CEP = "79290000",
                Cidade = "Bonito",
                Estado = "MS",
                Telefone = "991646461"
                }
            };
            var cliente = new Cliente
            {
                Nome = "Amanda Rodrigues",
                CEP = "79290000",
                Cidade = "Bonito",
                Estado = "MS",
                Telefone = "991646461"
            };

            using var db = new Data.ApplicationContext();
            //db.Produtos.Add(produto);
            //db.Set<Produto>().Add(produto);
            //db.Add(cliente);
            db.AddRange(listaClientes);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total de registros:  : {registros}");

        }
    }
}
