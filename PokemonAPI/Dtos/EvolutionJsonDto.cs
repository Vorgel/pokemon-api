using System.Text.Json.Serialization;

namespace PokemonAPI.Dtos;

internal sealed record EvolutionJsonDto(
    [property: JsonPropertyName("from")] string? From,
    [property: JsonPropertyName("to")] IReadOnlyList<string> To
);
