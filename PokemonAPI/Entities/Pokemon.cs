namespace PokemonAPI.Entities;

public class Pokemon
{
    public int Id { get; set; }
    public int Number { get; set; }
    public string Name { get; set; } = null!;
    public string Generation { get; set; } = null!;
    public int Height { get; set; }
    public int Weight { get; set; }
    public string ImageUrl { get; set; } = null!;

    public int HP { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }
    public int SpecialAttack { get; set; }
    public int SpecialDefense { get; set; }
    public int Speed { get; set; }

    public ICollection<PokemonType> Types { get; set; } = [];
    public ICollection<PokemonMove> Moves { get; set; } = [];
    public ICollection<PokemonAbility> Abilities { get; set; } = [];

    public int? EvolvesFromId { get; set; }
    public Pokemon? EvolvesFrom { get; set; }
    public ICollection<Pokemon> EvolvesTo { get; set; } = [];
}