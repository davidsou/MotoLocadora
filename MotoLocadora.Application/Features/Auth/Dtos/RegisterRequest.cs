using MotoLocadora.Application.Features.Ryders.Dtos;

namespace MotoLocadora.Application.Features.Auth.Dtos;

public record RegisterRequest(
    string FullName,
    string Email,
    string Password,
    RiderDto Rider
);