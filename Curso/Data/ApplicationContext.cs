using Curso.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curso.Data
{
    public class ApplicationContext : DbContext
    {
        
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer("Data source=(localdb)\\mssqllocaldb;Initial Catalog=CursoEFCore;Integrated Security=true",
                p=>p.EnableRetryOnFailure(
                    maxRetryCount: 2,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd:null));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Ele vai procurar todos os arquivos que implementam o IEntityTypeConfiguration
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);

        }

        private void MapearPropriedadesEsquecidas(ModelBuilder modelBuilder)
        {
            foreach(var entity in modelBuilder.Model.GetEntityTypes())
            {
                var propriedades = entity.GetProperties().Where(p=>p.ClrType == typeof(string));
                foreach(var propriedade in propriedades)
                {
                    if (string.IsNullOrEmpty(propriedade.GetColumnType())
                        && !propriedade.GetMaxLength().HasValue)
                    {
                        propriedade.SetColumnType("VARCHAR(100)");

                    }
                }
            }

        }
    }
}
