using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MotoLocadora.Application.Interfaces;
using MotoLocadora.Application.Models.Auth;
using MotoLocadora.BuildingBlocks.Core;
using MotoLocadora.BuildingBlocks.Entities;
using MotoLocadora.BuildingBlocks.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

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
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            return FromResult(OperationResult.Failure(result.Errors.Select(e => e.Description)));
        }

        await userManager.AddToRoleAsync(user, "Client");

        return FromResult(OperationResult.Success("Usuário registrado com sucesso!"));
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
