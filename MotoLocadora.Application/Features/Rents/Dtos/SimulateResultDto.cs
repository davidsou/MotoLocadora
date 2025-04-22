using MotoLocadora.BuildingBlocks.Extensions;
using System.Text.Json.Serialization;

namespace MotoLocadora.Application.Features.Rents.Dtos;

public record SimulationResultDto(
    int MotorcycleId,
    [property: JsonConverter(typeof(JsonDateOnlyConverter))] DateTime Start,
    [property: JsonConverter(typeof(JsonDateOnlyConverter))] DateTime EstimateEnd,
    decimal SelectedPrice,
    List<SimulationAlternative> Alternatives,
    string Message
);


public record SimulationAlternative(int Days, decimal Price);

