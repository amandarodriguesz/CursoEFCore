using Curso.Domain;
using Curso.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;

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

            InserirDados();
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

            using var db = new Data.ApplicationContext();
            //db.Produtos.Add(produto);
            //db.Set<Produto>().Add(produto);
            db.Add(produto);

            var registros = db.SaveChanges();
            Console.WriteLine($"Total de registros:  : {registros}");

        }
    }
}
