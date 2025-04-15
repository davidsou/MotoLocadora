using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MotoLocadora.Application.Features.Auth;
using MotoLocadora.Application.Features.Auth.Dtos;
using MotoLocadora.Application.Interfaces;
using MotoLocadora.Application.Models.Auth;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.BuildingBlocks.Entities;

namespace MotoLocadora.Api.Controllers;


[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthController(UserManager<ApplicationUser> userManager,
                      RoleManager<IdentityRole> roleManager,
                      ITokenService tokenService,
                      IMediator mediator) : BaseController(mediator)
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var command = new RegisterUserWithRider.Command(request);
        var result = await _mediator.Send(command);
        return FromResult(result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            return FromResult(OperationResult.Failure("Credenciais inválidas."));
        }

        var roles = await userManager.GetRolesAsync(user);
        var tokenResult = await tokenService.GenerateTokenAsync(user, roles);

        return FromResult(tokenResult);
    }

    [HttpPost("promote")]
    [Authorize(Roles = "Administrator")] // Apenas administradores podem promover outros usuários
    public async Task<IActionResult> PromoteUser([FromBody] PromoteUserRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return FromResult(OperationResult.Failure("Usuário não encontrado."));
        }

        var isInRole = await userManager.IsInRoleAsync(user, "Administrador");
        if (isInRole)
        {
            return FromResult(OperationResult.Failure("Usuário já é Administrador."));
        }

        var roleExists = await roleManager.RoleExistsAsync("Administrador");
        if (!roleExists)
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole("Administrador"));
            if (!roleResult.Succeeded)
            {
                return FromResult(OperationResult.Failure(roleResult.Errors.Select(e => e.Description)));
            }
        }

        var result = await userManager.AddToRoleAsync(user, "Administrador");
        if (!result.Succeeded)
        {
            return FromResult(OperationResult.Failure(result.Errors.Select(e => e.Description)));
        }

        return FromResult(OperationResult.Success("Usuário promovido para Administrador com sucesso!"));
    }

}
