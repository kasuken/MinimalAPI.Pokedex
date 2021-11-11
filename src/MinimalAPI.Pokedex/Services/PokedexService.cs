using MinalAPI.Pokedex.Models;

namespace MinimalAPI.Pokedex.Services;
public class PokedexService : IPokedexService
{
    private readonly IPokedexRepository pokedexRepository;

    public PokedexService(IPokedexRepository pokedexRepository)
    {
        this.pokedexRepository = pokedexRepository;
    }
    public async Task<PokedexResponse> GetAllAsync()
    {
        var pokemons = await pokedexRepository.LoadPokemons();
        return new PokedexResponse
        {
            Data = pokemons.Select(p => new PokemonListItemEntity
            {
                Num = p.Num,
                Name = p.Name,
                Image = p.Variations[0].Image
            }).ToList(),
        };
    }

    public async Task<PokedexPagedResponse> GetAsync(int page, int pageSize)
    {
        var pokemons = await pokedexRepository.LoadPokemons();
        var pokemonCount = pokemons.Count();
        var totalaPages = Convert.ToInt32(Math.Ceiling((double)pokemonCount / pageSize));
        int skipCount = (page - 1) * pageSize;
        return new PokedexPagedResponse
        {
            Data = pokemons.Skip(skipCount).Take(pageSize).Select(p => new PokemonListItemEntity
            {
                Num = p.Num,
                Name = p.Name,
                Image = p.Variations[0].Image
            }).ToList(),
            Page = page,
            TotalPages = totalaPages,
            TotalResults = pokemonCount
        };
    }

    public async Task<PokemonEntity> GetAsync(string name)
    {
        var pokemons = await pokedexRepository.LoadPokemons();
        return pokemons.FirstOrDefault(p => p.Name.ToLowerInvariant() == name.ToLowerInvariant());
    }

    public async Task<PokedexPagedResponse> SearchAsync(string query, int page, int pageSize)
    {
        var pokemons = await pokedexRepository.LoadPokemons();
        var filteredPokemons = pokemons.Where(p =>
        {
            var variation = p.Variations[0];
            return variation.Name.ToLowerInvariant().Contains(query.ToLowerInvariant()) ||
                   variation.Description.ToLowerInvariant().Contains(query.ToLowerInvariant()) ||
                   variation.Image.ToLowerInvariant().Contains(query.ToLowerInvariant()) ||
                   variation.Specie.ToLowerInvariant().Contains(query.ToLowerInvariant());
        });
 
        var pokemonCount = pokemons.Count();
        var totalaPages = Convert.ToInt32(Math.Ceiling((double)pokemonCount / pageSize));
        int skipCount = (page - 1) * pageSize;
        return new PokedexPagedResponse
        {
            Data = pokemons.Skip(skipCount).Take(pageSize).Select(p => new PokemonListItemEntity
            {
                Num = p.Num,
                Name = p.Name,
                Image = p.Variations[0].Image
            }).ToList(),
            Page = page,
            TotalPages = totalaPages,
            TotalResults = pokemonCount
        };
    }
}