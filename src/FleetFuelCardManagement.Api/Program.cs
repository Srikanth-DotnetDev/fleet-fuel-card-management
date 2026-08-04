using FleetFuelCardManagement.Application.Services;
using FleetFuelCardManagement.Infrastructure.Repositories;
using FleetFuelCardManagement.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFuelCardRepository, InMemoryFuelCardRepository>();
builder.Services.AddScoped<ICardEventPublisher, InMemoryCardEventPublisher>();
builder.Services.AddScoped<FuelCardApplicationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();

public partial class Program { }

