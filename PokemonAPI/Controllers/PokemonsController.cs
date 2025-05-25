using Microsoft.AspNetCore.Mvc;
using PokemonAPI.Dtos.Query;
using PokemonAPI.Services;

namespace PokemonAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PokemonsController(IPokemonService service) : ControllerBase
{
    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboardData(CancellationToken ct)
    {
        var result = await service.GetDashboardSummaryAsync(ct);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetPaged(
        [FromQuery] PokemonQueryParameters query,
        CancellationToken ct = default)
    {
        var result = await service.GetPagedAsync(query, ct);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDetails(int id, CancellationToken ct)
    {
        var pokemon = await service.GetByIdAsync(id, ct);

        return pokemon is null ? NotFound() : Ok(pokemon);
    }

    [HttpGet("types")]
    public async Task<ActionResult<List<string>>> GetAvailableTypes(CancellationToken ct)
    {
        var types = await service.GetAvailableTypesAsync(ct);

        return Ok(types);
    }

    [HttpGet("generations")]
    public async Task<ActionResult<List<string>>> GetAvailableGenerations(CancellationToken ct)
    {
        var generations = await service.GetAvailableGenerationsAsync(ct);

        return Ok(generations);
    }
}