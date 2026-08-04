using System.Net;
using System.Net.Http.Json;
using FleetFuelCardManagement.Api;
using Microsoft.AspNetCore.Mvc.Testing;

namespace FleetFuelCardManagement.Api.Tests;

public class FuelCardApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public FuelCardApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetHealth_ReturnsOk()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task PostIssueCard_ReturnsCreated()
    {
        var client = _factory.CreateClient();
        var payload = new
        {
            vehicleId = Guid.NewGuid(),
            cardNumber = "CARD-200",
            dailyLimit = 100m,
            transactionCountLimit = 5,
            fuelTypes = new[] { "Unleaded" }
        };

        var response = await client.PostAsJsonAsync("/api/v1/fuel-cards", payload);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
