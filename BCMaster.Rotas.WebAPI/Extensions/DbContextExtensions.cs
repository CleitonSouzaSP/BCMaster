using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using BCMaster.Infra.Context.Rotas;

namespace BCMaster.Rotas.WebAPI.Extensions
{
    public static class DbContextExtensions
    {
        public static IServiceCollection UseDbContext<T>(this IServiceCollection service, IConfiguration configuration) where T : DbContext
        {
            //Registrando uma instancia do Contexto default
            var result = (configuration.GetConnectionString("DefaultConnection"), service);
            service.AddDbContext<T>(options => options.UseSqlServer(result.Item1.ToString()).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            return service;
        }

        public static void ApplyMigrations(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var database = scope.ServiceProvider.GetRequiredService<RotaDbContext>().Database;
                if (database.IsThereMigrationsToApply())
                    database.Migrate();
            }
        }

        public static bool IsThereMigrationsToApply(this DatabaseFacade db)
        {
            var migrationsAssembly = db.GetService<IMigrationsAssembly>();
            var historyRepository = db.GetService<IHistoryRepository>();

            var all = migrationsAssembly.Migrations.Keys;
            var applied = historyRepository.GetAppliedMigrations().Select(r => r.MigrationId);
            var pending = all.Except(applied);

            return pending.Count() > 0;
        }
    }
}
