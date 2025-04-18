namespace MotoLocadora.Application.Features.Rents.Dtos;

public record CreateRentDto(
    int TariffId,
    int RiderId,
    int MotorcycleId,
    DateTime Start,
    DateTime EstimateEnd,
    DateTime End,
    decimal EstimatedPrice
);

