# Fleet Fuel Card Management

Fleet Fuel Card Management is a .NET 8 starter solution for managing company vehicles, fuel cards, spend policies, and card status history. The project follows a layered architecture with a domain model, application service, infrastructure abstractions, and a REST API.

## Repository

- GitHub: [Add repository link here](https://github.com/your-username/your-repository)

## Tech Stack

- .NET 8
- C#
- ASP.NET Core Web API
- xUnit for automated testing
- Swagger / OpenAPI for API documentation
- Layered architecture with Domain, Application, Infrastructure, and API projects

## Project Goals

The solution is designed to support the following Week 1 capabilities:

- Register vehicles and assigned fuel cards
- Track card state such as active, suspended, and expired
- Enforce spend rules such as daily limits, transaction count limits, and fuel-type restrictions
- Publish card lifecycle events when cards are issued, suspended, or policy changes occur
- Expose the core behavior through a simple API

## Solution Structure

The solution is organized into the following projects:

- `src/FleetFuelCardManagement.Domain`
  - Core domain entities and rules
  - Includes `FuelCard`, `SpendPolicy`, and card status history
- `src/FleetFuelCardManagement.Application`
  - Application services that orchestrate use cases
- `src/FleetFuelCardManagement.Infrastructure`
  - Repository and event publisher abstractions plus in-memory implementations
- `src/FleetFuelCardManagement.Api`
  - ASP.NET Core Web API hosting the endpoints
- `tests/FleetFuelCardManagement.Domain.Tests`
  - Unit tests for domain rules and invariants
- `tests/FleetFuelCardManagement.Api.Tests`
  - API smoke tests and endpoint validation

## Core Domain Rules

The current implementation enforces these core rules:

- Only one active card can be assigned per vehicle
- Spend policy values must be positive and valid
- Suspended cards cannot authorize transactions
- Fuel type restrictions are enforced by the spend policy

## Getting Started

### Prerequisites

- .NET 8 SDK
- A terminal with access to `dotnet`

### Restore and Build

```bash
dotnet build FleetFuelCardManagement.sln
```

### Run Tests

```bash
dotnet test FleetFuelCardManagement.sln
```

### Run the API

```bash
cd src/FleetFuelCardManagement.Api
dotnet run
```

The API will start locally and expose the following endpoints:

- `GET /health`
- `POST /api/v1/fuel-cards`
- `GET /api/v1/fuel-cards/{cardId}`

## Example API Request

Issue a card:

```bash
curl -X POST "http://localhost:5000/api/v1/fuel-cards" \
  -H "Content-Type: application/json" \
  -d '{
    "vehicleId": "00000000-0000-0000-0000-000000000001",
    "cardNumber": "CARD-100",
    "dailyLimit": 100,
    "transactionCountLimit": 5,
    "fuelTypes": ["Unleaded", "Diesel"]
  }'
```

## Notes

This project is a strong Week 1 foundation. It currently uses in-memory infrastructure for storage and event publication and is ready to evolve into persistence and event bus integration in later iterations.
