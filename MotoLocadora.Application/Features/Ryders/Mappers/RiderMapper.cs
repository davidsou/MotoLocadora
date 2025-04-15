using MotoLocadora.Application.Features.Ryders.Dtos;
using MotoLocadora.Domain.Entities;
using MotoLocadora.Domain.Enums;

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
        LicenseDriveImageLink = dto.LicenseDriveImageLink ?? string.Empty,
        Type = dto.Type.GetValueOrDefault(),
        Email = dto.Email,
        Phone = dto.Phone ?? string.Empty,
        UserId = dto.UserId ?? string.Empty,

    };
    public static Rider ToEntity(this RiderDto dto, string userId) => new()
    {
        Name = dto.Name,
        CommpanyId = dto.CommpanyId,
        BirthDate = dto.BirthDate,
        LicenseDrive = dto.LicenseDrive,
        LicenseDriveType = dto.LicenseDriveType,
        LicenseDriveImageLink = dto.LicenseDriveImageLink ?? string.Empty,
        Type = dto.Type.GetValueOrDefault(),
        Email = dto.Email ?? string.Empty,
        Phone = dto.Phone ?? string.Empty,
        UserId = userId ?? string.Empty
    };

    public static RiderDto ToDto(this Rider entity) => new(
        entity.Name,
        entity.CommpanyId,
        entity.BirthDate,
        entity.LicenseDrive,
        Enum.TryParse<LicenseDriveTypeEnum>(entity.LicenseDriveType.ToString(), out var licenseType) ? licenseType : LicenseDriveTypeEnum.A,
        entity.LicenseDriveImageLink ,
        entity.Type,
        entity.Email,
        entity.Phone,
        entity.UserId
    );
}
