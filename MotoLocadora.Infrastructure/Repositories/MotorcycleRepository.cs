﻿using MotoLocadora.BuildingBlocks.Repositories;
using MotoLocadora.Domain.Entities;
using MotoLocadora.Domain.Interfaces;
using MotoLocadora.Infrastructure.Context;

namespace MotoLocadora.Infrastructure.Repositories;

public class MotorcycleRepository(AppSqlContext context) : SqlBaseRepository<Motorcycle>(context), IMotorcycleRepository
{
}
