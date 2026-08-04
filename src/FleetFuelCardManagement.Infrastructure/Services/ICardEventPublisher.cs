using FleetFuelCardManagement.Domain.Entities;

namespace FleetFuelCardManagement.Infrastructure.Services;

public interface ICardEventPublisher
{
    void PublishCardIssued(FuelCard card);
    void PublishCardSuspended(FuelCard card);
    void PublishCardReactivated(FuelCard card);
    void PublishPolicyChanged(FuelCard card);
}
