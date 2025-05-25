using Microsoft.EntityFrameworkCore;
using PokemonAPI.Entities;

namespace PokemonAPI.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Pokemon> Pokemons { get; set; } = null!;
    public DbSet<PokemonType> PokemonTypes { get; set; } = null!;
    public DbSet<PokemonMove> PokemonMoves { get; set; } = null!;
    public DbSet<PokemonAbility> PokemonAbilities { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pokemon>()
            .HasOne(p => p.EvolvesFrom)
            .WithMany(p => p!.EvolvesTo)
            .HasForeignKey(p => p.EvolvesFromId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Pokemon>()
            .HasMany(p => p.Types)
            .WithMany(t => t.Pokemons)
            .UsingEntity(j => j.ToTable("PokemonToTypes"));

        modelBuilder.Entity<Pokemon>()
            .HasMany(p => p.Moves)
            .WithMany(m => m.Pokemons)
            .UsingEntity(j => j.ToTable("PokemonToMoves"));

        modelBuilder.Entity<Pokemon>()
            .HasMany(p => p.Abilities)
            .WithMany(a => a.Pokemons)
            .UsingEntity(j => j.ToTable("PokemonToAbilities"));

        modelBuilder.Entity<PokemonType>()
           .HasIndex(t => t.Name)
           .IsUnique();

        modelBuilder.Entity<PokemonMove>()
            .HasIndex(m => m.Name)
            .IsUnique();

        modelBuilder.Entity<PokemonAbility>()
            .HasIndex(a => a.Name)
            .IsUnique();
    }
}