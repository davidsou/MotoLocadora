using Microsoft.AspNetCore.Http;
using MotoLocadora.Domain.Interfaces;
using System.Security.Claims;

namespace MotoLocadora.Application.Services;

public class CurrentUserService : ICurrentUserService
{
    public string? UserId { get; }

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
