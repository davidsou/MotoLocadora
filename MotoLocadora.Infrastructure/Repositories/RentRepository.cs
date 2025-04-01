using Microsoft.EntityFrameworkCore;
using MotoLocadora.BuildingBlocks.Repositories;
using MotoLocadora.Domain.Entities;

namespace MotoLocadora.Infrastructure.Repositories;

public class RentRepository(DbContext context) : SqlBaseRepository<Rent>(context)
{
}
