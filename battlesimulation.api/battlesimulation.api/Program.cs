using battlesimulation.api.Interfaces;
using battlesimulation.api.SeedData;
using battlesimulation.api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddSingleton<IFighterService, FighterService>();
builder.Services.AddScoped<IBattleSimulator, BattleService>();

var app = builder.Build();

// Seed initial fighters
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<IFighterService>();
    SeedData.Initialize(service);
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
