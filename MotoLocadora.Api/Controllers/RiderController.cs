using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoLocadora.Application.Features.Ryders.Dtos;
using MotoLocadora.Application.Features.Ryders;

namespace MotoLocadora.Api.Controllers;

[Route("api/[controller]")]
public class RiderController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] RiderDto dto)
    {
        var result = await _mediator.Send(new CreateRider.Command(dto));
        return FromResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RiderDto dto)
    {
        var result = await _mediator.Send(new UpdateRider.Command(id, dto));
        return FromResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteRider.Command(id));
        return FromResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetRiderById.Query(id));
        return FromResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllRiders.Query());
        return FromResult(result);
    }
}
