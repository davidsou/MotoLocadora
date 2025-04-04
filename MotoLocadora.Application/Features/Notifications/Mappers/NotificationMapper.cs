using MotoLocadora.Application.Features.Notifications.Dtos;
using MotoLocadora.Domain.Entities;

namespace MotoLocadora.Application.Features.Notifications.Mappers;

public static class NotificationMapper
{
    public static Notification ToEntity(this NotificationDto dto) => new()
    {
        Type = dto.Type,
        Description = dto.Description
    };

    public static NotificationDto ToDto(this Notification entity) => new(entity.Type, entity.Description);
}
