using FleetFuelCardManagement.Domain.Entities;

namespace FleetFuelCardManagement.Infrastructure.Services;

public sealed class InMemoryCardEventPublisher : ICardEventPublisher
{
    public void PublishCardIssued(FuelCard card) { }
    public void PublishCardSuspended(FuelCard card) { }
    public void PublishCardReactivated(FuelCard card) { }
    public void PublishPolicyChanged(FuelCard card) { }
}
