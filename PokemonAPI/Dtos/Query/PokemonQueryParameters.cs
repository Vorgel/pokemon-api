namespace PokemonAPI.Dtos.Query;

public class PokemonQueryParameters
{
    private int _page = 1;
    private int _pageSize = 25;

    public int Page
    {
        get => _page;
        set => _page = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value is < 1 or > 100 ? 25 : value;
    }

    public string? Search { get; set; }
    public string? Type { get; set; }
    public string? Generation { get; set; }

    private string? _sortBy;
    public string SortBy
    {
        get => string.IsNullOrWhiteSpace(_sortBy) ? "number" : _sortBy!;
        set => _sortBy = value;
    }

    public bool Desc { get; set; } = false;
}
