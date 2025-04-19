using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoLocadora.Application.Features.Rents.Dtos;
using MotoLocadora.Application.Features.Rents;

namespace MotoLocadora.Api.Controllers;

[Route("api/[controller]")]
public class RentController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRentDto dto)
    {
        var result = await _mediator.Send(new CreateRent.Command(dto));
        return FromResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RentDto dto)
    {
        var result = await _mediator.Send(new UpdateRent.Command(id, dto));
        return FromResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteRent.Command(id));
        return FromResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetRentById.Query(id));
        return FromResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllRents.Query());
        return FromResult(result);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged([FromQuery] RentQueryParams queryParams)
    {
        var result = await _mediator.Send(new QueryRents.Query(queryParams));
        return FromResult(result);
    }

    [HttpGet("detailed/{id}")]
    public async Task<IActionResult> GetDetailedById(int id)
    {
        var result = await _mediator.Send(new QueryRentById.Query(id));
        return FromResult(result);
    }

    [HttpGet("simular")]
    public async Task<IActionResult> Simulate([FromQuery] int motorcycleId, [FromQuery] DateTime start, [FromQuery] DateTime estimateEnd)
    {
        var result = await _mediator.Send(new SimulateRent.Query(motorcycleId, start, estimateEnd));
        return FromResult(result);
    }
}