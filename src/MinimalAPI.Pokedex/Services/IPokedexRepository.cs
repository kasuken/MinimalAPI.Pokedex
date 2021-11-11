using MinalAPI.Pokedex.Models;

namespace MinimalAPI.Pokedex.Services;

public interface IPokedexRepository
{
    public Task<List<PokemonEntity>> LoadPokemons();
}

