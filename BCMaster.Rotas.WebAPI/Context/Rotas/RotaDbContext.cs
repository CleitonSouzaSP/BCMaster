using BCMaster.Rotas.WebAPI.Domain.Rotas;
using BCMaster.Rotas.WebAPI.Infra.Rotas;
using Microsoft.EntityFrameworkCore;

namespace BCMaster.Rotas.WebAPI.Context.Rotas
{
    public class RotaDbContext : DbContext
    {
        public RotaDbContext(DbContextOptions<RotaDbContext> options) : base(options)
        {
            TryApplyMigration(options);
        }

        public DbSet<Rota> Rotas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new RotaEntityConfiguration());
        }

        private void TryApplyMigration(DbContextOptions<RotaDbContext> options)
        {
            var inMemoryConfiguration = options.Extensions.FirstOrDefault(x => x.ToString().Contains("InMemoryOptionsExtension"));

            if (inMemoryConfiguration == null && Database.GetPendingMigrations().Count() > 0)
                Database.Migrate();
        }
    }
}
