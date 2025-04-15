using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MotoLocadora.Application.Interfaces;
using MotoLocadora.Application.Models.Auth;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.BuildingBlocks.Entities;
using MotoLocadora.BuildingBlocks.Options;
using MotoLocadora.Domain.Entities;
using MotoLocadora.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace MotoLocadora.Application.Services;


public class TokenService : ITokenService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtOptions _jwtOptions;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public TokenService(UserManager<ApplicationUser> userManager,
                        IOptions<JwtOptions> jwtOptions,
                        IRefreshTokenRepository refreshTokenRepository)
    {
        _userManager = userManager;
        _jwtOptions = jwtOptions.Value;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<OperationResult<AuthResponse>> GenerateTokenAsync(ApplicationUser user, IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id),
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtOptions.ExpireMinutes),
            signingCredentials: creds
        );

        var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        var refreshToken = new RefreshToken
        {
            Token = Guid.NewGuid().ToString(),
            UserId = user.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7) // por exemplo: 7 dias de validade para refresh
        };

        await _refreshTokenRepository.AddAsync(refreshToken);

        var response = new AuthResponse
        {
            Token = token,
            Expires = tokenDescriptor.ValidTo,
            RefreshToken = refreshToken.Token,
            Roles = roles
        };

        return OperationResult<AuthResponse>.Success(response);
    }

    public async Task<OperationResult<AuthResponse>> RefreshTokenAsync(string refreshToken)
    {
        var existingToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

        if (existingToken == null || existingToken.ExpiresAt < DateTime.UtcNow)
        {
            return OperationResult<AuthResponse>.Failure("Refresh token inválido ou expirado");
        }

        var user = await _userManager.FindByIdAsync(existingToken.UserId);
        if (user == null)
        {
            return OperationResult<AuthResponse>.Failure("Usuário não encontrado para este refresh token");
        }

        var roles = await _userManager.GetRolesAsync(user);

        return await GenerateTokenAsync(user, roles);
    }
}


