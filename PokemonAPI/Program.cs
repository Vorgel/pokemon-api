using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using PokemonAPI.Data;
using PokemonAPI.Repositories;
using PokemonAPI.Services;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
var seedPath = Environment.GetEnvironmentVariable("POKEMON_SEED_PATH");

if (string.IsNullOrEmpty(connectionString))
    throw new InvalidOperationException("Missing CONNECTION_STRING environment variable.");

if (string.IsNullOrEmpty(seedPath))
    throw new InvalidOperationException("Missing POKEMON_SEED_PATH environment variable.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IPokemonRepository, PokemonRepository>();
builder.Services.AddScoped<IPokemonService, PokemonService>();

builder.Services.AddLogging();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.MapControllers();
app.UseRouting();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("DbInitializer");

    context.Database.Migrate();

    var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(10)).Token;

    await DbInitializer.InitializeAsync(context, seedPath, logger, CancellationToken.None);
}

app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.Run();