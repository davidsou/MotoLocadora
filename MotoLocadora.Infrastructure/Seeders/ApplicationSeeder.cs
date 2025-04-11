using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MotoLocadora.BuildingBlocks.Entities;
using MotoLocadora.BuildingBlocks.Options;
using MotoLocadora.Domain.Entities;
using MotoLocadora.Infrastructure.Context;
using Polly;

namespace MotoLocadora.Infrastructure.Seeders;
public class ApplicationSeeder(
    IServiceProvider serviceProvider,
    IOptions<AdminSeedOptions> adminOptions,
    ILogger<ApplicationSeeder> logger)
{
    private static readonly AsyncPolicy RetryPolicy = Policy
       .Handle<Exception>()
       .WaitAndRetryAsync(
           retryCount: 5,
           sleepDurationProvider: attempt => TimeSpan.FromSeconds(5),
           onRetry: (exception, timeSpan, retryCount, context) =>
           {
               Console.WriteLine($"⚠️ Retry {retryCount} after {timeSpan.TotalSeconds}s due to: {exception.Message}");
           });

    public async Task SeedAsync()
    {
        using var scope = serviceProvider.CreateScope();

        //  Contexto de Identity
        var identityDbContext = scope.ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>();

        //  Contexto da aplicação
        var appDbContext = scope.ServiceProvider.GetRequiredService<AppSqlContext>();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        logger.LogInformation("Start database migration");

        await RetryPolicy.ExecuteAsync(async () =>
        {
            await identityDbContext.Database.MigrateAsync();
            await appDbContext.Database.MigrateAsync();
        });

        logger.LogInformation("Database migration completed");

        logger.LogInformation("Ensure roles are created");
        await EnsureRoleExists(roleManager, "Administrator");
        await EnsureRoleExists(roleManager, "Client");

        logger.LogInformation("Ensure admin user exists");
        var adminUser = await userManager.FindByEmailAsync(adminOptions.Value.Email);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminOptions.Value.Email,
                Email = adminOptions.Value.Email,
                FullName = "Administrator Account"
            };

            var result = await userManager.CreateAsync(adminUser, adminOptions.Value.Password);
            if (!result.Succeeded)
            {
                logger.LogError("Failed to create admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                return;
            }
            logger.LogInformation("Admin user created");
        }

        if (!await userManager.IsInRoleAsync(adminUser, "Administrator"))
        {
            await userManager.AddToRoleAsync(adminUser, "Administrator");
            logger.LogInformation("Admin user assigned to Administrator role");
        }

        logger.LogInformation("Ensure tariffs are created");
        var tariffsToSeed = new List<Tariff>
        {
            new() { Days = 7, Price = 30.00m },
            new() { Days = 15, Price = 28.00m },
            new() { Days = 30, Price = 22.00m },
            new() { Days = 45, Price = 20.00m },
            new() { Days = 50, Price = 18.00m }
        };

        foreach (var tariff in tariffsToSeed)
        {
            var exists = await appDbContext.Tariffs.AnyAsync(t => t.Days == tariff.Days);
            if (!exists)
            {
                appDbContext.Tariffs.Add(tariff);
                logger.LogInformation("Added tariff: {Days} days at {Price:C}", tariff.Days, tariff.Price);
            }
        }

        await appDbContext.SaveChangesAsync();

        logger.LogInformation("Database seeding completed");
    }

    private static async Task EnsureRoleExists(RoleManager<IdentityRole> roleManager, string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (roleResult.Succeeded)
            {
                roleManager.Logger.LogInformation("Role {RoleName} created", roleName);
            }
            else
            {
                roleManager.Logger.LogError("Failed to create role {RoleName}: {Errors}", roleName, string.Join(", ", roleResult.Errors.Select(e => e.Description)));
            }
        }
    }
}