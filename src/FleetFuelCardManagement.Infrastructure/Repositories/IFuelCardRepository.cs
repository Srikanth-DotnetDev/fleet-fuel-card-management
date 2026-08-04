using FleetFuelCardManagement.Domain.Entities;

namespace FleetFuelCardManagement.Infrastructure.Repositories;

public interface IFuelCardRepository
{
    void Add(FuelCard card);
    FuelCard? GetById(Guid cardId);
    FuelCard? GetActiveByVehicle(Guid vehicleId);
}
