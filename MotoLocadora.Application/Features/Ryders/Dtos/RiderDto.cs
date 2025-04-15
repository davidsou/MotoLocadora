using MotoLocadora.Domain.Enums;

namespace MotoLocadora.Application.Features.Ryders.Dtos;

public record RiderDto(
    string Name,
    string CommpanyId,
    DateTime BirthDate,
    string LicenseDrive,
    LicenseDriveTypeEnum LicenseDriveType,
    string? LicenseDriveImageLink,
    int? Type,
    string Email,
    string? Phone ,
    string? UserId 
);

