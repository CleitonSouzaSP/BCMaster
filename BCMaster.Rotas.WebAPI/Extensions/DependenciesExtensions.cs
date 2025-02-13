using BCMaster.Domain.Inteface.Rotas;
using BCMaster.Infra.Infra.Rotas;
using BCMaster.Services.Services.Rotas;

namespace BCMaster.Rotas.WebAPI.Extensions
{
    public static  class DependenciesExtensions
    {
        public static void AddRepositories(this IServiceCollection services) 
        {
            services.AddScoped<IRotaRepository, RotaRepository>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IRotaService, RotaService>();
        }

    }
}
