using FleetFuelCardManagement.Application.Services;
using FleetFuelCardManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FleetFuelCardManagement.Api.Controllers;

[ApiController]
[Route("api/v1/fuel-cards")]
public sealed class FuelCardsController : ControllerBase
{
    private readonly FuelCardApplicationService _service;

    public FuelCardsController(FuelCardApplicationService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult IssueCard([FromBody] IssueCardRequest request)
    {
        var policy = SpendPolicy.Create(request.DailyLimit, request.TransactionCountLimit, request.FuelTypes.Select(Enum.Parse<FuelType>));
        var card = _service.IssueCard(request.VehicleId, request.CardNumber, policy);
        return Created($"/api/v1/fuel-cards/{card.Id}", new { card.Id, card.CardNumber, card.Status });
    }

    [HttpGet("{cardId:guid}")]
    public IActionResult GetCard(Guid cardId)
    {
        var card = _service.GetCard(cardId);
        return Ok(new { card.Id, card.CardNumber, card.Status, card.SpendPolicy, card.StatusHistory });
    }

    [HttpGet("health")]
    public IActionResult Health() => Ok(new { status = "ok" });
}

public sealed record IssueCardRequest(Guid VehicleId, string CardNumber, decimal DailyLimit, int TransactionCountLimit, IEnumerable<string> FuelTypes);
