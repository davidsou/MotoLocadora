using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MotoLocadora.Domain.Interfaces;
using MotoLocadora.Infrastructure.Context;
using MotoLocadora.Infrastructure.Repositories;
using MotoLocadora.BuildingBlocks.Options;

namespace MotoLocadora.Infraestructure.Ioc;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraestructure(this IServiceCollection services,  IConfiguration configuration)
    {
        var connectionStringSettings = new ConnectionStringOptions();
        configuration.GetSection(ConnectionStringOptions.SectionName)
                     .Bind(connectionStringSettings);

        DbContext(services, connectionStringSettings);
        Repositories(services);
        // builder.Services.AddRabbitMqSetup(builder.Configuration);

        return services;
    }
    private static void ApplicationServices(IServiceCollection services)
    {
    }
    private static void DbContext(IServiceCollection services, ConnectionStringOptions connectionStringSettings)
    {
        services.AddDbContext<AppSqlContext>(options => options.UseNpgsql(connectionStringSettings.SqlConnection));
    }
    private static void Repositories(IServiceCollection services)
    {
        services.AddTransient<IMotorcycleRepository, MotorcycleRepository>();
        services.AddTransient<INotificationRepository, NotificationRepository>();
        services.AddTransient<IRentRepository, RentRepository>();
        services.AddTransient<IRiderRepository, RiderRepository>();
        services.AddTransient<ITariffRepository, TariffRepository>();
    }
}
