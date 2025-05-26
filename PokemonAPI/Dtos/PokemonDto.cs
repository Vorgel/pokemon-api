namespace PokemonAPI.Dtos;

public record PokemonDto
(
    int Id,
    string Name,
    string Generation,
    int Height,
    int Weight,
    string ImageUrl,
    int HP,
    int Attack,
    int Defense,
    int SpecialAttack,
    int SpecialDefense,
    int Speed,
    List<string> Types,
    List<string> Moves,
    List<string> Abilities,
    EvolutionInfoDto? EvolvesFrom,
    List<EvolutionInfoDto> EvolvesTo
);
