using MediatR;
using Microsoft.AspNetCore.Mvc;
using MotoLocadora.Application.Features.Notifications.Dtos;
using MotoLocadora.Application.Features.Notifications;

namespace MotoLocadora.Api.Controllers;
[Route("api/[controller]")]
public class NotificationController(IMediator mediator) : BaseController(mediator)
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] NotificationDto dto)
    {
        var result = await _mediator.Send(new CreateNotification.Command(dto));
        return FromResult(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] NotificationDto dto)
    {
        var result = await _mediator.Send(new UpdateNotification.Command(id, dto));
        return FromResult(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteNotification.Command(id));
        return FromResult(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _mediator.Send(new GetNotificationById.Query(id));
        return FromResult(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllNotifications.Query());
        return FromResult(result);
    }
}