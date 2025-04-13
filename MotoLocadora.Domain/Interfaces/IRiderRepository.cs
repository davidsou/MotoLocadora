using MotoLocadora.Domain.Entities;
using MotoLocadora.BuildingBlocks.Interfaces;

namespace MotoLocadora.Domain.Interfaces;

public interface IRiderRepository : ISqlBaseRepository<Rider>
{
    Task<bool> ExistsByCompanyIdAsync(string companyId);
    Task<bool> ExistsByLicenseDriveAsync(string licenseDrive);
}
