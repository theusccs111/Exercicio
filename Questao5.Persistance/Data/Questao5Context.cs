using Microsoft.EntityFrameworkCore;
using Questao5.Domain.Entites;
using Questao5.Persistance.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao5.Persistance.Data
{
    public class Questao5Context : DbContext
    {
        public DbSet<ContaCorrente> ContaCorrente { get; set; }
        public DbSet<IdEmpotencia> IdEmpotencia { get; set; }
        public DbSet<Movimento> Movimento { get; set; }

        public Questao5Context(DbContextOptions<Questao5Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ApplyConfiguratons(modelBuilder);
        }


        private static void ApplyConfiguratons(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ContaCorrenteConfiguration());
            modelBuilder.ApplyConfiguration(new IdEmpotenciaConfiguration());
            modelBuilder.ApplyConfiguration(new MovimentoConfiguration());
        }
    }
}
