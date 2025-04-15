using MotoLocadora.BuildingBlocks.Entities;

namespace MotoLocadora.Domain.Entities;

public  class Notification : BaseEntity
{
    public string Type { get; set; }
    public string Description { get; set; }
}
