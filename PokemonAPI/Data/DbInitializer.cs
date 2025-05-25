using Microsoft.EntityFrameworkCore;
using PokemonAPI.Dtos;
using PokemonAPI.Entities;
using PokemonAPI.Services;
using System.Text.Json;

namespace PokemonAPI.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(
         ApplicationDbContext context,
         string jsonPath,
         ILogger? logger = null,
         CancellationToken cancellationToken = default)
    {
        if (await context.Pokemons.AnyAsync(cancellationToken))
        {
            logger?.LogInformation("Database already contains Pokemon data. Skipping seeding.");
            return;
        }

        try
        {
            if (!File.Exists(jsonPath))
            {
                logger?.LogWarning("Seed file not found: {Path}", jsonPath);
                return;
            }

            await using var stream = File.OpenRead(jsonPath);
            var dtoList = await JsonSerializer.DeserializeAsync<List<PokemonJsonDto>>(stream, cancellationToken: cancellationToken);

            if (dtoList is null)
            {
                logger?.LogError("Failed to deserialize Pokemon data.");
                return;
            }

            var typeCache = new Dictionary<string, PokemonType>();
            var moveCache = new Dictionary<string, PokemonMove>();
            var abilityCache = new Dictionary<string, PokemonAbility>();

            var pokemons = dtoList.Select(dto =>
                PokemonMapper.ToPokemon(dto, typeCache, moveCache, abilityCache)).ToList();

            var nameMap = pokemons.ToDictionary(p => p.Name.ToLower());

            foreach (var dto in dtoList)
            {
                if (dto.Evolution?.From is not null &&
                    nameMap.TryGetValue(dto.Evolution.From.ToLower(), out var parent))
                {
                    nameMap[dto.Name.ToLower()].EvolvesFrom = parent;
                }
            }

            await context.Pokemons.AddRangeAsync(pokemons, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            logger?.LogInformation("Successfully seeded {Count} Pokemon(s).", pokemons.Count);
        }
        catch (OperationCanceledException)
        {
            logger?.LogWarning("Seeding cancelled by cancellation token.");
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "Error during Pokemon database initialization.");
        }
    }
}