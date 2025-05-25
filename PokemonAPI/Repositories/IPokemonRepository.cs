using PokemonAPI.Dtos;
using PokemonAPI.Dtos.Query;

namespace PokemonAPI.Repositories;

public interface IPokemonRepository
{
    Task<PagedResult<PokemonListItemDto>> GetPagedAsync(PokemonQueryParameters query, CancellationToken ct);
    Task<List<string>> GetAvailableTypesAsync(CancellationToken ct);
    Task<List<string>> GetAvailableGenerationsAsync(CancellationToken ct);
    Task<PokemonDto?> GetByIdAsync(int id, CancellationToken ct);
    Task<DashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken ct);
}
