using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MotoLocadora.Application.Interfaces;
using MotoLocadora.Application.Services;
using MotoLocadora.BuildingBlocks.Extensions;
using MotoLocadora.Domain.Interfaces;
using System.Reflection;

namespace MotoLocadora.Application.Extensions;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddBuildingBlocksBehaviors();
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddScoped<ITokenService, TokenService>();
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;

    }
}

