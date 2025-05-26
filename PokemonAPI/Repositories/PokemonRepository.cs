using Microsoft.EntityFrameworkCore;
using PokemonAPI.Data;
using PokemonAPI.Dtos;
using PokemonAPI.Dtos.Query;

namespace PokemonAPI.Repositories;

public class PokemonRepository(ApplicationDbContext context) : IPokemonRepository
{
    public async Task<PagedResult<PokemonListItemDto>> GetPagedAsync(
    PokemonQueryParameters query,
    CancellationToken ct)
    {
        var pokemons = context.Pokemons
            .AsNoTracking()
            .Include(p => p.Types)
            .Include(p => p.Moves)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim().ToLower();

            var isNumber = int.TryParse(search, out var parsedNumber);

            pokemons = pokemons.Where(p =>
                p.Name.ToLower().Contains(search) ||
                (isNumber && p.Number == parsedNumber) ||
                p.Moves.Any(m => m.Name.ToLower().Contains(search))
            );
        }

        if (!string.IsNullOrWhiteSpace(query.Type))
            pokemons = pokemons.Where(p => p.Types.Any(t => t.Name == query.Type));

        if (!string.IsNullOrWhiteSpace(query.Generation))
            pokemons = pokemons.Where(p => p.Generation == query.Generation);

        pokemons = query.SortBy?.ToLower() switch
        {
            "name" => query.Desc ? pokemons.OrderByDescending(p => p.Name) : pokemons.OrderBy(p => p.Name),
            "number" => query.Desc ? pokemons.OrderByDescending(p => p.Number) : pokemons.OrderBy(p => p.Number),
            "generation" => query.Desc ? pokemons.OrderByDescending(p => p.Generation) : pokemons.OrderBy(p => p.Generation),
            "height" => query.Desc ? pokemons.OrderByDescending(p => p.Height) : pokemons.OrderBy(p => p.Height),
            "weight" => query.Desc ? pokemons.OrderByDescending(p => p.Weight) : pokemons.OrderBy(p => p.Weight),
            "movescount" => query.Desc ? pokemons.OrderByDescending(p => p.Moves.Count) : pokemons.OrderBy(p => p.Moves.Count),
            _ => pokemons.OrderBy(p => p.Id)
        };

        var total = await pokemons.CountAsync(ct);

        var items = await pokemons
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(p => new PokemonListItemDto(
                p.Id,
                p.Number,
                p.Name,
                p.Generation,
                p.Height,
                p.Weight,
                p.Types.OrderBy(t => t.Id).Select(t => t.Name).FirstOrDefault() ?? "",
                p.Types.OrderBy(t => t.Id).Select(t => t.Name).Skip(1).FirstOrDefault(),
                p.Moves.Count,
                p.ImageUrl
            ))
            .ToListAsync(ct);

        return new PagedResult<PokemonListItemDto>(total, items);
    }

    public async Task<PokemonDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        return await context.Pokemons
            .AsNoTracking()
            .Include(p => p.Types)
            .Include(p => p.Moves)
            .Include(p => p.Abilities)
            .Include(p => p.EvolvesFrom)
            .Include(p => p.EvolvesTo)
            .Where(p => p.Id == id)
            .Select(p => new PokemonDto(
                p.Id,
                p.Name,
                p.Generation,
                p.Height,
                p.Weight,
                p.ImageUrl,
                p.HP,
                p.Attack,
                p.Defense,
                p.SpecialAttack,
                p.SpecialDefense,
                p.Speed,
                p.Types.Select(t => t.Name).ToList(),
                p.Moves.Select(m => m.Name).ToList(),
                p.Abilities.Select(a => a.Name).ToList(),
                p.EvolvesFrom != null ? new EvolutionInfoDto(p.EvolvesFrom.Id, p.EvolvesFrom.Name) : null,
                p.EvolvesTo.Select(e => new EvolutionInfoDto(e.Id, e.Name)).ToList()
            ))
            .FirstOrDefaultAsync(ct);
    }

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken ct)
    {
        var all = await context.Pokemons
            .AsNoTracking()
            .Include(p => p.Types)
            .ToListAsync(ct);

        int totalCount = all.Count;

        var countPerType = all
            .SelectMany(p => p.Types)
            .GroupBy(t => t.Name)
            .ToDictionary(g => g.Key, g => g.Count());

        var countPerGeneration = all
            .GroupBy(p => p.Generation)
            .ToDictionary(g => g.Key, g => g.Count());

        return new DashboardSummaryDto(totalCount, countPerType, countPerGeneration);
    }

    public async Task<List<string>> GetAvailableGenerationsAsync(CancellationToken ct)
    {
        return await context.Pokemons
            .AsNoTracking()
            .Select(p => p.Generation)
            .Distinct()
            .OrderBy(g => g)
            .ToListAsync(ct);
    }

    public async Task<List<string>> GetAvailableTypesAsync(CancellationToken ct)
    {
        return await context.PokemonTypes
            .AsNoTracking()
            .Select(t => t.Name)
            .Distinct()
            .OrderBy(name => name)
            .ToListAsync(ct);
    }
}