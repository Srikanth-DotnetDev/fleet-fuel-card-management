using FleetFuelCardManagement.Domain.Entities;
using FleetFuelCardManagement.Infrastructure.Repositories;
using FleetFuelCardManagement.Infrastructure.Services;

namespace FleetFuelCardManagement.Application.Services;

public sealed class FuelCardApplicationService
{
    private readonly IFuelCardRepository _repository;
    private readonly ICardEventPublisher _publisher;

    public FuelCardApplicationService(IFuelCardRepository repository, ICardEventPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public FuelCard IssueCard(Guid vehicleId, string cardNumber, SpendPolicy policy)
    {
        var existing = _repository.GetActiveByVehicle(vehicleId);
        if (existing is not null)
        {
            throw new InvalidOperationException("A vehicle can only have one active card.");
        }

        var card = FuelCard.Issue(vehicleId, cardNumber, policy);
        _repository.Add(card);
        _publisher.PublishCardIssued(card);
        return card;
    }

    public FuelCard UpdatePolicy(Guid cardId, SpendPolicy policy)
    {
        var card = _repository.GetById(cardId) ?? throw new InvalidOperationException("Card not found.");
        card.UpdatePolicy(policy);
        _publisher.PublishPolicyChanged(card);
        return card;
    }

    public FuelCard SuspendCard(Guid cardId, string reason)
    {
        var card = _repository.GetById(cardId) ?? throw new InvalidOperationException("Card not found.");
        card.Suspend(reason);
        _publisher.PublishCardSuspended(card);
        return card;
    }

    public FuelCard ReactivateCard(Guid cardId, string reason)
    {
        var card = _repository.GetById(cardId) ?? throw new InvalidOperationException("Card not found.");
        card.Reactivate(reason);
        _publisher.PublishCardReactivated(card);
        return card;
    }

    public FuelCard GetCard(Guid cardId) => _repository.GetById(cardId) ?? throw new InvalidOperationException("Card not found.");
}
