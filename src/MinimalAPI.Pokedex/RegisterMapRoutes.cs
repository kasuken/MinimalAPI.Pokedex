using MinalAPI.Pokedex;
using MinalAPI.Pokedex.Models;
using MinimalAPI.Pokedex.Services;

namespace MinimalAPI.Pokedex;

public static class RegisterMapRoutes
{
    public static IEndpointRouteBuilder RegisterRoutes(this IEndpointRouteBuilder builder)
    {
        MapPagedList(builder);
        MapSinglePokemon(builder);
        MapAll(builder);
        MapSearch(builder);
        return builder;
    }

    private static void MapSearch(IEndpointRouteBuilder builder)
    {
        builder.MapGet(RouteConstants.Search, async (string query, int? page, int? pageSize, IPokedexService service) =>
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Results.BadRequest();
            }
            return Results.Ok(await service.SearchAsync(query, page ?? 1, pageSize ?? 20));
        })
        .Produces<PokedexPagedResponse>(StatusCodes.Status200OK)
        .Produces<PokedexPagedResponse>(StatusCodes.Status400BadRequest)
        .RequireCors("AnyOrigin");
    }

    private static void MapAll(IEndpointRouteBuilder builder)
    {
        builder.MapGet(RouteConstants.All, async (IPokedexService service) =>
        {
            return await service.GetAllAsync();
        })
        .Produces<PokedexResponse>(StatusCodes.Status200OK)
        .RequireCors("AnyOrigin");
    }

    private static void MapSinglePokemon(IEndpointRouteBuilder builder)
    {
        builder.MapGet(RouteConstants.SinglePokemon, async (string name, IPokedexService service) =>
        {
            var pokemon = await service.GetAsync(name);
            if (pokemon == null)
            {
                return Results.NotFound();
            }
            return Results.Ok(pokemon);
        })
        .Produces<PokemonEntity>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequireCors("AnyOrigin");
    }

    private static void MapPagedList(IEndpointRouteBuilder builder)
    {
        builder.MapGet(RouteConstants.PagedList, async (int? page, int? pageSize, IPokedexService service) =>
        {
            return await service.GetAsync(page ?? 1, pageSize ?? 20);
        })
        .Produces<PokedexPagedResponse>(StatusCodes.Status200OK)
        .RequireCors("AnyOrigin");
    }
}