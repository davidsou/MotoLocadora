namespace MotoLocadora.Application.Features.Rents.Dtos;

public record RentDto(
    int TariffId,
    int RiderId,
    int MotorcycleId,
    DateTime Start,
    DateTime EstimateEnd,
    DateTime End,
    decimal AppliedFine,
    string FineReason,
    decimal FinalPrice
);