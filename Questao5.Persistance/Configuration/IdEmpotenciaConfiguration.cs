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
    public class IdEmpotenciaConfiguration : IEntityTypeConfiguration<IdEmpotencia>
    {
        public void Configure(EntityTypeBuilder<IdEmpotencia> builder)
        {
            builder.HasKey(i => i.ChaveIdempotencia);
        }
    }
}
