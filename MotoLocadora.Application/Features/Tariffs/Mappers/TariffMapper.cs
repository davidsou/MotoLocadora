using MotoLocadora.Application.Features.Tariffs.Dtos;
using MotoLocadora.Domain.Entities;

namespace MotoLocadora.Application.Features.Tariffs.Mappers;
public static class TariffMapper
{
    public static Tariff ToEntity(this TariffDto dto) => new()
    {
        Price = dto.Price,
        Days = dto.Days
    };

    public static TariffDto ToDto(this Tariff entity) => new(
        entity.Price,
        entity.Days
    );
}