using Microsoft.EntityFrameworkCore;
using MotoLocadora.BuildingBlocks.Repositories;
using MotoLocadora.Domain.Entities;
using MotoLocadora.Domain.Interfaces;
using MotoLocadora.Infrastructure.Context;

namespace MotoLocadora.Infrastructure.Repositories;

public class RiderRepository(AppSqlContext context) : SqlBaseRepository<Rider>(context),IRiderRepository
{
    public async Task<bool> ExistsByCompanyIdAsync(string companyId)
    {
        return await context.Riders.AnyAsync(r => r.CommpanyId == companyId);
    }

    public async Task<bool> ExistsByLicenseDriveAsync(string licenseDrive)
    {
        return await context.Riders.AnyAsync(r => r.LicenseDrive == licenseDrive);
    }

    public async Task<Rider?> GetRiderByUserIdAsync(string userId)
    {
        return await context.Riders.FirstOrDefaultAsync( x=> x.UserId == userId);
    }
}
