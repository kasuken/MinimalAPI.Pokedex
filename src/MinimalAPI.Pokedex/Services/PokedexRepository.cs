using MinalAPI.Pokedex.Models;
using System.Text.Json;

namespace MinimalAPI.Pokedex.Services;

public class PokedexRepository : IPokedexRepository
{
    private List<PokemonEntity> pokemons;
    readonly IWebHostEnvironment environment;

    private JsonSerializerOptions jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public PokedexRepository(IWebHostEnvironment _environment)
    {
        environment = _environment;
    }

    public async Task<List<PokemonEntity>> LoadPokemons()
    {
        if (pokemons == null)
        {
            var data = await File.ReadAllTextAsync(Path.Combine(environment.ContentRootPath, "db", "pokemon.json"));
            var pokemons = new List<PokemonEntity>();
            using (var document = JsonDocument.Parse(data))
            {
                foreach (var item in document.RootElement.EnumerateArray())
                {
                    var pokemonEntity = JsonSerializer.Deserialize<PokemonEntity>(item, jsonOptions);
                    pokemons.Add(pokemonEntity);
                }
            }
            return pokemons;
        }

        return pokemons;
    }
}