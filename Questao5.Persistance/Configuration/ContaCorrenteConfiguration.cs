using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Questao5.Domain.Entites;

namespace Questao5.Persistance.Configuration
{
    public class ContaCorrenteConfiguration : IEntityTypeConfiguration<ContaCorrente>
    {
        public void Configure(EntityTypeBuilder<ContaCorrente> builder)
        {
            builder.HasKey(c => c.IdContaCorrente);

            builder.HasMany(c => c.Movimentos).WithOne(e => e.ContaCorrente).HasForeignKey(e => e.IdContaCorrente);

            builder.Property(c => c.Numero).IsRequired();
            builder.Property(c => c.Nome).IsRequired();
            builder.Property(c => c.Ativo).IsRequired();
        }
    }
}
