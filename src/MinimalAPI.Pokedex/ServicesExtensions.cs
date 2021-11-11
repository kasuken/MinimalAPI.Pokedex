using MinimalAPI.Pokedex.Services;

namespace MinimalAPI.Pokedex;

public static class ServicesExtensions
{
    public static IServiceCollection AddPokedexServices(this IServiceCollection services)
    {
        services.AddScoped<IPokedexRepository, PokedexRepository>();
        services.AddScoped<IPokedexService, PokedexService>();
        return services;
    }
}
