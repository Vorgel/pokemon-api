using PokemonAPI.Dtos;
using PokemonAPI.Entities;

namespace PokemonAPI.Services;

public static class PokemonMapper
{
    internal static Pokemon ToPokemon(
        PokemonJsonDto dto,
        Dictionary<string, PokemonType> typeCache,
        Dictionary<string, PokemonMove> moveCache,
        Dictionary<string, PokemonAbility> abilityCache)
    {
        var statsDictionary = dto.Stats?.ToDictionary(s => s.Name.ToLower(), s => s.Value) ?? [];

        PokemonType GetOrAddType(string name)
        {
            if (!typeCache.TryGetValue(name, out var type))
                typeCache[name] = type = new PokemonType { Name = name };
            return type;
        }

        PokemonMove GetOrAddMove(string name)
        {
            if (!moveCache.TryGetValue(name, out var move))
                moveCache[name] = move = new PokemonMove { Name = name };
            return move;
        }

        PokemonAbility GetOrAddAbility(string name)
        {
            if (!abilityCache.TryGetValue(name, out var ability))
                abilityCache[name] = ability = new PokemonAbility { Name = name };
            return ability;
        }

        return new Pokemon
        {
            Number = dto.Number,
            Name = dto.Name ?? string.Empty,
            Generation = dto.Generation ?? string.Empty,
            Height = dto.Height,
            Weight = dto.Weight,
            ImageUrl = dto.Image ?? string.Empty,

            HP = statsDictionary.GetValueOrDefault("hp"),
            Attack = statsDictionary.GetValueOrDefault("attack"),
            Defense = statsDictionary.GetValueOrDefault("defense"),
            SpecialAttack = statsDictionary.GetValueOrDefault("special-attack"),
            SpecialDefense = statsDictionary.GetValueOrDefault("special-defense"),
            Speed = statsDictionary.GetValueOrDefault("speed"),

            Types = [.. (dto.Types ?? []).Select(t => GetOrAddType(t.ToLower()))],
            Moves = [.. (dto.Moves ?? []).Select(m => GetOrAddMove(m.ToLower()))],
            Abilities = [.. (dto.Abilities ?? []).Select(a => GetOrAddAbility(a.ToLower()))]
        };
    }

    public static PokemonDto ToDto(Pokemon p) =>
       new(
           Id: p.Id,
           Name: p.Name,
           Generation: p.Generation,
           Height: p.Height,
           Weight: p.Weight,
           ImageUrl: p.ImageUrl,
           HP: p.HP,
           Attack: p.Attack,
           Defense: p.Defense,
           SpecialAttack: p.SpecialAttack,
           SpecialDefense: p.SpecialDefense,
           Speed: p.Speed,
           Types: [.. p.Types.Select(t => t.Name)],
           Moves: [.. p.Moves.Select(m => m.Name)],
           Abilities: [.. p.Abilities.Select(a => a.Name)],
           EvolvesFrom: p.EvolvesFrom is null ? null : new EvolutionInfoDto(p.EvolvesFrom.Id, p.EvolvesFrom.Name),
       EvolvesTo: [.. p.EvolvesTo.Select(e => new EvolutionInfoDto(e.Id, e.Name))]
       );
}