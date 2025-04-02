using Microsoft.EntityFrameworkCore;
using MotoLocadora.BuildingBlocks.Repositories;
using MotoLocadora.Domain.Entities;
using MotoLocadora.Domain.Interfaces;

namespace MotoLocadora.Infrastructure.Repositories;

public class MotorcycleRepository(DbContext context) : SqlBaseRepository<Motorcycle>(context), IMotorcycleRepository
{
}
