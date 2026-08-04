namespace FleetFuelCardManagement.Domain.Entities;

public enum CardStatus
{
    Active,
    Suspended,
    Expired
}

public enum FuelType
{
    Unleaded,
    Diesel,
    Hybrid,
    Electric
}

public sealed class FuelCard
{
    private readonly List<CardStatusEvent> _statusHistory = new();

    public FuelCard(Guid vehicleId, string cardNumber, SpendPolicy spendPolicy)
    {
        VehicleId = vehicleId;
        CardNumber = cardNumber;
        SpendPolicy = spendPolicy;
        Status = CardStatus.Active;
        _statusHistory.Add(new CardStatusEvent(Status, DateTimeOffset.UtcNow, "Issued"));
    }

    public Guid Id { get; } = Guid.NewGuid();
    public Guid VehicleId { get; private set; }
    public string CardNumber { get; private set; }
    public CardStatus Status { get; private set; }
    public SpendPolicy SpendPolicy { get; private set; }
    public IReadOnlyCollection<CardStatusEvent> StatusHistory => _statusHistory.AsReadOnly();

    public static FuelCard Issue(Guid vehicleId, string cardNumber, SpendPolicy spendPolicy) => new(vehicleId, cardNumber, spendPolicy);

    public void UpdatePolicy(SpendPolicy newPolicy)
    {
        if (!newPolicy.IsValid())
        {
            throw new ArgumentException("Policy limits must be positive and valid.", nameof(newPolicy));
        }

        SpendPolicy = newPolicy;
    }

    public void Suspend(string reason)
    {
        if (Status == CardStatus.Suspended)
        {
            return;
        }

        Status = CardStatus.Suspended;
        _statusHistory.Add(new CardStatusEvent(Status, DateTimeOffset.UtcNow, reason));
    }

    public void Reactivate(string reason)
    {
        if (Status != CardStatus.Suspended)
        {
            return;
        }

        Status = CardStatus.Active;
        _statusHistory.Add(new CardStatusEvent(Status, DateTimeOffset.UtcNow, reason));
    }

    public void AuthorizeTransaction(decimal amount, FuelType fuelType)
    {
        if (Status == CardStatus.Suspended)
        {
            throw new InvalidOperationException("The card is suspended and cannot authorize transactions.");
        }

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Transaction amount must be positive.");
        }

        if (!SpendPolicy.AllowsFuelType(fuelType))
        {
            throw new InvalidOperationException("This fuel type is not allowed by the spend policy.");
        }

        if (amount > SpendPolicy.DailyLimit)
        {
            throw new InvalidOperationException("Transaction exceeds the daily spend limit.");
        }
    }
}

public sealed class SpendPolicy
{
    private SpendPolicy(decimal dailyLimit, int transactionCountLimit, IReadOnlyCollection<FuelType> allowedFuelTypes)
    {
        DailyLimit = dailyLimit;
        TransactionCountLimit = transactionCountLimit;
        AllowedFuelTypes = allowedFuelTypes.ToArray();
    }

    public decimal DailyLimit { get; }
    public int TransactionCountLimit { get; }
    public IReadOnlyCollection<FuelType> AllowedFuelTypes { get; }

    public static SpendPolicy Create(decimal dailyLimit, int transactionCountLimit, IEnumerable<FuelType> allowedFuelTypes)
    {
        if (dailyLimit <= 0 || transactionCountLimit <= 0 || allowedFuelTypes is null || !allowedFuelTypes.Any())
        {
            throw new ArgumentException("Policy limits must be positive and valid.");
        }

        return new SpendPolicy(dailyLimit, transactionCountLimit, allowedFuelTypes.Distinct().ToArray());
    }

    public bool IsValid() => DailyLimit > 0 && TransactionCountLimit > 0 && AllowedFuelTypes.Any();
    public bool AllowsFuelType(FuelType fuelType) => AllowedFuelTypes.Contains(fuelType);
}

public sealed record CardStatusEvent(CardStatus Status, DateTimeOffset Timestamp, string Reason);
