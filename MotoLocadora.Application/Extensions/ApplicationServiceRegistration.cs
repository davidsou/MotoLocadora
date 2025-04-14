using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MotoLocadora.Application.Features.Ryders.Validators;
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

        var applicationAssembly = typeof(ApplicationServiceRegistration).Assembly;
        services.AddValidatorsFromAssembly(applicationAssembly);

        services.AddHttpContextAccessor();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;

    }
}

