namespace PokemonAPI.Dtos;

public record PokemonListItemDto(
    int Id,
    int Number,
    string Name,
    string Generation,
    int Height,
    int Weight,
    string? Type1,
    string? Type2,
    int MovesCount,
    string ImageUrl
);
