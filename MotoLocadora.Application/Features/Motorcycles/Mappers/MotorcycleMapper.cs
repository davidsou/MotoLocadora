using MotoLocadora.Application.Features.Motorcycles.Dtos;
using MotoLocadora.Domain.Entities;

namespace MotoLocadora.Application.Features.Motorcycles.Mappers;

public static class MotorcycleMapper
{
    public static Motorcycle ToEntity(this CreateMotorcycleDto dto)
    {
        return new Motorcycle
        {
            Ano = dto.Ano,
            Modelo = dto.Modelo,
            Placa = dto.Placa
        };
    }

    public static CreateMotorcycleDto ToCreateDto(this Motorcycle entity)
    {
        return new CreateMotorcycleDto(entity.Ano, entity.Modelo, entity.Placa);
    }

    public static Motorcycle ToEntity(this MotorcycleDto dto)
    {
        return new Motorcycle
        {
            Id = dto.Id,
            Ano = dto.Ano,
            Modelo = dto.Modelo,
            Placa = dto.Placa
        };
    }

    public static MotorcycleDto ToDto(this Motorcycle entity)
    {
        return new MotorcycleDto(entity.Id, entity.Ano, entity.Modelo, entity.Placa);
    }
}