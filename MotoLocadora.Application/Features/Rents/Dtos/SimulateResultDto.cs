namespace MotoLocadora.Application.Features.Rents.Dtos;

public record SimulationResultDto(
    int MotorcycleId,
    DateTime Start,
    DateTime EstimateEnd,
    decimal SelectedPrice,
    List<SimulationAlternative> Alternatives,
    string Message
);

public record SimulationAlternative(int Days, decimal Price);

