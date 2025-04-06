using MotoLocadora.BuildingBlocks.Core;

namespace MotoLocadora.Application.Features.Notifications.Dtos;

public class NotificationQueryParams : QueryParamsBase
{
    public string? FilterByType { get; set; }
    public string? FilterByDescription { get; set; }
}
