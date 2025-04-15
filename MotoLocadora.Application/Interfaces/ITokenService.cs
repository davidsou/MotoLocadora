using MotoLocadora.Application.Models.Auth;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.BuildingBlocks.Entities;

namespace MotoLocadora.Application.Interfaces;
public interface ITokenService
{
    Task<OperationResult<AuthResponse>> GenerateTokenAsync(ApplicationUser user, IList<string> roles);
    Task<OperationResult<AuthResponse>> RefreshTokenAsync(string refreshToken);
}
