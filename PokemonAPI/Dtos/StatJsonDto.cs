using System.Text.Json.Serialization;

namespace PokemonAPI.Dtos;

internal sealed record StatJsonDto(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("value")] int Value
);