namespace PokemonAPI.Entities;

public class PokemonMove
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Pokemon> Pokemons { get; set; } = [];
}
