using PokemonAPI.Dtos;
using PokemonAPI.Dtos.Query;

namespace PokemonAPI.Services;

public interface IPokemonService
{
    Task<PokemonDto?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<DashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken);
    Task<PagedResult<PokemonListItemDto>> GetPagedAsync(PokemonQueryParameters query, CancellationToken cancellationToken);
    Task<List<string>> GetAvailableTypesAsync(CancellationToken ct);
    Task<List<string>> GetAvailableGenerationsAsync(CancellationToken ct);
}
