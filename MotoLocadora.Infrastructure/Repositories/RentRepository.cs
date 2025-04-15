using MotoLocadora.BuildingBlocks.Repositories;
using MotoLocadora.Domain.Entities;
using MotoLocadora.Domain.Interfaces;
using MotoLocadora.Infrastructure.Context;

namespace MotoLocadora.Infrastructure.Repositories;

public class RentRepository(AppSqlContext context) : SqlBaseRepository<Rent>(context),IRentRepository
{
}
