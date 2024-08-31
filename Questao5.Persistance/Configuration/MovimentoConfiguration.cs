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
    public class MovimentoConfiguration : IEntityTypeConfiguration<Movimento>
    {
        public void Configure(EntityTypeBuilder<Movimento> builder)
        {
            builder.HasKey(m => m.IdMovimento);

            builder.Property(m => m.IdContaCorrente).IsRequired();
            builder.Property(m => m.DataMovimento).IsRequired();
            builder.Property(m => m.TipoMovimento).IsRequired();
            builder.Property(m => m.Valor).IsRequired();
        }
    }
}
