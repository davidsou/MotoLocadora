using MotoLocadora.BuildingBlocks.Core;

namespace MotoLocadora.Application.Features.Tariffs.Dtos;

public class TariffQueryParams : QueryParamsBase
{
    public decimal? FilterByPrice { get; set; }
    public int? FilterByDays { get; set; }
}

