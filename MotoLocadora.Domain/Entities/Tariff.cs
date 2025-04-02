using MotoLocadora.BuildingBlocks.Entities;

namespace MotoLocadora.Domain.Entities;

public class Tariff:BaseEntity
{
    public decimal Price { get; set; }
    public int Days { get; set; }
}
