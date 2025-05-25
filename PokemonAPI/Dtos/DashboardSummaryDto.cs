namespace PokemonAPI.Dtos;

public record DashboardSummaryDto(
    int TotalCount,
    Dictionary<string, int> CountPerType,
    Dictionary<string, int> CountPerGeneration
);