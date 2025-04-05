using MotoLocadora.Application.Features.Ryders.Dtos;
using MotoLocadora.Domain.Entities;

namespace MotoLocadora.Application.Features.Ryders.Mappers;
public static class RiderMapper
{
    public static Rider ToEntity(this RiderDto dto) => new()
    {
        Name = dto.Name,
        CommpanyId = dto.CommpanyId,
        BirthDate = dto.BirthDate,
        LicenseDrive = dto.LicenseDrive,
        LicenseDriveType = dto.LicenseDriveType,
        LicenseDriveImageLink = dto.LicenseDriveImageLink,
        Type = dto.Type,
        Email = dto.Email,
        Phone = dto.Phone
    };

    public static RiderDto ToDto(this Rider entity) => new(
        entity.Name,
        entity.CommpanyId,
        entity.BirthDate,
        entity.LicenseDrive,
        entity.LicenseDriveType,
        entity.LicenseDriveImageLink,
        entity.Type,
        entity.Email,
        entity.Phone
    );
}
