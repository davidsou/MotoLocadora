using MotoLocadora.BuildingBlocks.Entities;

namespace MotoLocadora.Domain.Entities;

public class Rent:BaseEntity
{
    public int TariffId { get; set; }
    public int RiderId { get; set; }
    public int MotorcycleId { get; set; }
    public DateTime Start { get; set; }
    public DateTime EstimateEnd { get; set; }
    public DateTime End { get; set; }
    public decimal AppliedFine { get; set; }
    public string FineReason { get; set; }
    public Decimal FinalPrice { get; set; }


}
