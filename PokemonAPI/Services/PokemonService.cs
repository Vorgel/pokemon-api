using Microsoft.Extensions.Caching.Memory;
using PokemonAPI.Dtos;
using PokemonAPI.Dtos.Query;
using PokemonAPI.Repositories;
using System.Text.RegularExpressions;

namespace PokemonAPI.Services;

public class PokemonService(IPokemonRepository repository, IMemoryCache cache) : IPokemonService
{
    public async Task<PokemonDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        string cacheKey = $"pokemon_details_{id}";

        if (cache.TryGetValue(cacheKey, out PokemonDto? cached) && cached is not null)
            return cached;

        var dto = await repository.GetByIdAsync(id, ct);

        if (dto is not null)
            cache.Set(cacheKey, dto, TimeSpan.FromMinutes(10));

        return dto;
    }

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken ct)
    {
        const string cacheKey = "dashboard_summary";

        if (cache.TryGetValue(cacheKey, out DashboardSummaryDto? cached) && cached is not null)
            return cached;

        var summary = await repository.GetDashboardSummaryAsync(ct);

        cache.Set(cacheKey, summary, TimeSpan.FromMinutes(10));

        return summary;
    }

    public Task<PagedResult<PokemonListItemDto>> GetPagedAsync(
        PokemonQueryParameters query, CancellationToken ct)
    {
        return repository.GetPagedAsync(query, ct);
    }

    public async Task<List<string>> GetAvailableTypesAsync(CancellationToken ct)
    {
        const string cacheKey = "available_types";

        if (cache.TryGetValue(cacheKey, out List<string>? cached) && cached is not null)
            return cached;

        var types = await repository.GetAvailableTypesAsync(ct);
        var sorted = types.OrderBy(t => t).ToList();

        cache.Set(cacheKey, sorted, TimeSpan.FromHours(1));

        return sorted;
    }

    public async Task<List<string>> GetAvailableGenerationsAsync(CancellationToken ct)
    {
        const string cacheKey = "available_generations";

        if (cache.TryGetValue(cacheKey, out List<string>? cached) && cached is not null)
            return cached;

        var generations = await repository.GetAvailableGenerationsAsync(ct);

        var sorted = generations
            .OrderBy(g =>
            {
                var match = Regex.Match(g, @"Generation (\w+)");
                return match.Success ? RomanToInt(match.Groups[1].Value) : int.MaxValue;
            })
            .ToList();

        cache.Set(cacheKey, sorted, TimeSpan.FromHours(1));

        return sorted;
    }

    private static int RomanToInt(string roman)
    {
        return roman switch
        {
            "I" => 1,
            "II" => 2,
            "III" => 3,
            "IV" => 4,
            "V" => 5,
            "VI" => 6,
            "VII" => 7,
            "VIII" => 8,
            "IX" => 9,
            "X" => 10,
            _ => int.MaxValue
        };
    }
}
