namespace MotoLocadora.Application.Features.Ryders.Dtos;

public record RiderDto(
    string Name,
    string CommpanyId,
    DateTime BirthDate,
    string LicenseDrive,
    string LicenseDriveType,
    string LicenseDriveImageLink,
    int Type,
    string Email,
    string Phone
);

