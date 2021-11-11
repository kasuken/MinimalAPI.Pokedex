# MinimalAPI.Pokedex
This project is a showcase for the new .NET 6 Minimal APIs feature for developing Web APIs.

![](![](https://countrush-prod.azurewebsites.net/l/badge/?repository=kasuken.MinimalAPI.Pokedex)

# Description

The project uses a static pokedex JSON file as a data store. The JSON file is kept in `db` folder. Swagger and Swagger UI are implemented to look the supported endpoints.

Feedbacks are welcome.

# Requirements
- .NET 6
- Visual Studio 2022 or Visual Studio Code

# Highlights

- In program.cs, API explorer and Swagger.
    ```csharp
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options => options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Description = "Pokedex API with .NET 6 Minimal API",
        Title = "Pokedex API",
        Version = "v1",
        Contact = new OpenApiContact()
        {
            Name = "Emanuele Bartolesi",
            Url = new Uri("https://github.com/kasuken")
        }
    }));
    ```
- CORS
    ```csharp
    builder.Services.AddCors(options => options.AddPolicy("AnyOrigin", o => o.AllowAnyOrigin()));
    ```

- services to service collection.
    ```csharp
    public static IServiceCollection AddPokedexServices(this IServiceCollection services)
    {
        services.AddScoped<IPokedexRepository, PokedexRepository>();
        services.AddScoped<IPokedexService, PokedexService>();
        return services;
    }
    ```
- Routes in a separate class with some costants for path
    ```csharp
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
    ```    
