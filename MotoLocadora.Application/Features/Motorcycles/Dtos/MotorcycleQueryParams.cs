using MotoLocadora.BuildingBlocks.Core;

namespace MotoLocadora.Application.Features.Motorcycles.Dtos;

public class MotorcycleQueryParams : QueryParamsBase
{
    public string? FilterByModelo { get; set; }
    public int? FilterByAno { get; set; }
}
