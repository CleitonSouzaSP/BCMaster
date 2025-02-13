using BCMaster.Rotas.WebAPI.Infra.Rotas;
using BCMaster.Rotas.WebAPI.Inteface.Rotas;
using BCMaster.Rotas.WebAPI.Services.Rotas;

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
