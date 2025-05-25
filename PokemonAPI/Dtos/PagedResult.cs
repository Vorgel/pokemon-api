namespace PokemonAPI.Dtos;

public record PagedResult<T>(
    int TotalCount,
    List<T> Items
);