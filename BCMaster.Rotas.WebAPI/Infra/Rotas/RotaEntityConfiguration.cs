using BCMaster.Rotas.WebAPI.Domain.Rotas;
using Microsoft.EntityFrameworkCore;

namespace BCMaster.Rotas.WebAPI.Infra.Rotas
{
    public class RotaEntityConfiguration : IEntityTypeConfiguration<Rota>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Rota> builder)
        {
            builder.ToTable("Rotas");
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Origem);
            builder.Property(r => r.Destino);
            builder.Property(r => r.Valor);
        }
    }
}
