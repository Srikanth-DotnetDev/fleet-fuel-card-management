using FleetFuelCardManagement.Application.Services;
using FleetFuelCardManagement.Domain.Entities;
using FleetFuelCardManagement.Infrastructure.Repositories;
using FleetFuelCardManagement.Infrastructure.Services;

namespace FleetFuelCardManagement.Domain.Tests;

public class FuelCardTests
{
    [Fact]
    public void IssueCard_WithExistingActiveCardForVehicle_Throws()
    {
        var repository = new InMemoryFuelCardRepository();
        var publisher = new InMemoryCardEventPublisher();
        var service = new FuelCardApplicationService(repository, publisher);

        var vehicleId = Guid.NewGuid();
        var policy = SpendPolicy.Create(100m, 5, new[] { FuelType.Diesel, FuelType.Unleaded });
        service.IssueCard(vehicleId, "CARD-001", policy);

        var exception = Assert.Throws<InvalidOperationException>(() =>
            service.IssueCard(vehicleId, "CARD-002", policy));

        Assert.Contains("active card", exception.Message);
    }

    [Fact]
    public void AuthorizeTransaction_OnSuspendedCard_Throws()
    {
        var card = FuelCard.Issue(Guid.NewGuid(), "CARD-100", SpendPolicy.Create(100m, 5, new[] { FuelType.Unleaded }));
        card.Suspend("Fraud review");

        var exception = Assert.Throws<InvalidOperationException>(() => card.AuthorizeTransaction(25m, FuelType.Unleaded));

        Assert.Contains("suspended", exception.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void UpdatePolicy_WithInvalidLimits_Throws()
    {
        var card = FuelCard.Issue(Guid.NewGuid(), "CARD-101", SpendPolicy.Create(100m, 5, new[] { FuelType.Unleaded }));

        var exception = Assert.Throws<ArgumentException>(() => card.UpdatePolicy(SpendPolicy.Create(0m, 5, new[] { FuelType.Unleaded })));

        Assert.Contains("positive", exception.Message, StringComparison.OrdinalIgnoreCase);
    }
}
