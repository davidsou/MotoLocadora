using MotoLocadora.BuildingBlocks.Core;

namespace MotoLocadora.Application.Features.Rents.Dtos;

public class RentQueryParams : QueryParamsBase
{
    public int? FilterByRiderId { get; set; }
    public int? FilterByMotorcycleId { get; set; }
    public int? FilterByTariffId { get; set; }
}
