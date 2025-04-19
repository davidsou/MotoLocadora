using MotoLocadora.Application.Features.Rents.Dtos;
using MotoLocadora.Domain.Entities;

namespace MotoLocadora.Application.Features.Rents.Mappers;

public static class RentMapper
{
    public static Rent ToEntity(this RentDto dto) => new()
    {
        TariffId = dto.TariffId,
        RiderId = dto.RiderId,
        MotorcycleId = dto.MotorcycleId,
        Start = dto.Start,
        EstimateEnd = dto.EstimateEnd,
        End = dto.End,
        EstimatedPrice = dto.EstimatedPrice,
        AppliedFine = dto.AppliedFine,
        FineReason = dto.FineReason,
        FinalPrice = dto.FinalPrice
    };

    public static RentDto ToDto(this Rent entity) => new(
        entity.TariffId,
        entity.RiderId,
        entity.MotorcycleId,
        entity.Start,
        entity.EstimateEnd,
        entity.End,
        entity.EstimatedPrice,
        entity.AppliedFine,
        entity.FineReason,
        entity.FinalPrice
    );
}
