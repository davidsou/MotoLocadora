using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoLocadora.Api.Controllers;
using MotoLocadora.Application.Features.Tariffs.Dtos;
using MotoLocadora.Application.Features.Tariffs;

[Route("api/[controller]")]
public class TariffController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TariffDto dto)
    {
        var result = await _mediator.Send(new CreateTariff.Command(dto));
        return FromResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TariffDto dto)
    {
        var result = await _mediator.Send(new UpdateTariff.Command(id, dto));
        return FromResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteTariff.Command(id));
        return FromResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetTariffById.Query(id));
        return FromResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllTariffs.Query());
        return FromResult(result);
    }
}

