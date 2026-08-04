using FleetFuelCardManagement.Domain.Entities;

namespace FleetFuelCardManagement.Infrastructure.Repositories;

public sealed class InMemoryFuelCardRepository : IFuelCardRepository
{
    private readonly Dictionary<Guid, FuelCard> _cards = new();

    public void Add(FuelCard card) => _cards[card.Id] = card;

    public FuelCard? GetById(Guid cardId) => _cards.TryGetValue(cardId, out var card) ? card : null;

    public FuelCard? GetActiveByVehicle(Guid vehicleId) => _cards.Values.FirstOrDefault(card => card.VehicleId == vehicleId && card.Status == CardStatus.Active);
}
