using MotoLocadora.Application.Features.Motorcycles.Dtos;
using MotoLocadora.Domain.Entities;

namespace MotoLocadora.Application.Features.Motorcycles.Mappers;

public static class MotorcycleMapper
{
    public static Domain.Entities.Motorcycle ToEntity(this MotorcycleDto dto)
    {
        return new Domain.Entities.Motorcycle
        {
            Ano = dto.Ano,
            Modelo = dto.Modelo,
            Placa = dto.Placa
        };
    }

    public static MotorcycleDto ToDto(this Domain.Entities.Motorcycle entity)
    {
        return new MotorcycleDto(entity.Ano, entity.Modelo, entity.Placa);
    }
}