﻿using Microsoft.EntityFrameworkCore;
using MotoLocadora.Domain.Entities;
using MotoLocadora.Infrastructure.EntityConfiguration;
using MotoLocadoraBuildingBlocks.Entities;
using System.Reflection;

namespace MotoLocadora.Infrastructure.Context;

public  class AppSqlContext(DbContextOptions<AppSqlContext> options) : DbContext(options)
{
    public DbSet<Motorcycle> Motorcycles { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Rent> Rents { get; set; }
    public DbSet<Rider> Riders { get; set; }
    public DbSet<Tariff> Tariffs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        //optionsBuilder.UseLazyLoadingProxies();


    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.ApplyConfiguration(new MotorcycleConfiguration());
        modelBuilder.ApplyConfiguration(new NotificationConfiguration());
        modelBuilder.ApplyConfiguration(new RentConfiguration());
        modelBuilder.ApplyConfiguration(new RiderConfiguration());
        modelBuilder.ApplyConfiguration(new TariffConfiguration());
    }

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.ChangedAt = DateTime.UtcNow;
                    break;
            }
        return base.SaveChanges();
    }


    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.ChangedAt = DateTime.UtcNow;
                    break;
            }

        return base.SaveChangesAsync(cancellationToken);
    }
}
