using MotoLocadora.BuildingBlocks.Core;

namespace MotoLocadora.Application.Features.Ryders.Dtos;

public class RiderQueryParams : QueryParamsBase
{
    public string? FilterByName { get; set; }
    public string? FilterByCompanyId { get; set; }
    public string? FilterByLicenseDrive { get; set; }
}
