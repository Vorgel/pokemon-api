using System.Text.Json.Serialization;

namespace PokemonAPI.Dtos;

internal sealed record PokemonJsonDto(
    [property: JsonPropertyName("number")] int Number,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("generation")] string Generation,
    [property: JsonPropertyName("height")] int Height,
    [property: JsonPropertyName("weight")] int Weight,
    [property: JsonPropertyName("types")] IReadOnlyList<string> Types,
    [property: JsonPropertyName("stats")] IReadOnlyList<StatJsonDto> Stats,
    [property: JsonPropertyName("moves")] IReadOnlyList<string> Moves,
    [property: JsonPropertyName("abilities")] IReadOnlyList<string> Abilities,
    [property: JsonPropertyName("evolution")] EvolutionJsonDto Evolution,
    [property: JsonPropertyName("image")] string Image
);