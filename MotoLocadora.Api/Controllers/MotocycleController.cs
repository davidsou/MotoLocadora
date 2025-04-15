using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoLocadora.Application.Features.Motorcycles;
using MotoLocadora.Application.Features.Motorcycles.Dtos;

namespace MotoLocadora.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MotorcycleController(IMediator mediator) : BaseController(mediator)
{

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MotorcycleDto dto)
    {
        var result = await _mediator.Send(new CreateMotorcycle.Command(dto));
        return FromResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MotorcycleDto dto)
    {
        var result = await _mediator.Send(new UpdateMotorcycle.Command(id, dto));
        return FromResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteMotorcycle.Command(id));
        return FromResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetMotorcycleById.Query(id));
        return FromResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllMotorcycles.Query());
        return FromResult(result);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPaged([FromQuery] MotorcycleQueryParams queryParams)
    {
        var result = await _mediator.Send(new QueryMotorcycles.Query(queryParams));
        return FromResult(result);
    }

    [HttpGet("detailed/{id}")]
    public async Task<IActionResult> GetDetailedById(int id)
    {
        var result = await _mediator.Send(new QueryMotorcycleById.Query(id));
        return FromResult(result);
    }
}
