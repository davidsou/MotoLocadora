using Microsoft.EntityFrameworkCore;

namespace MotoLocadora.Infrastructure.Context;

public  class AppSqlContext : DbContext
{
    public AppSqlContext(DbContextOptions<AppSqlContext> options) : base(options) { }

    //public DbSet<Product> Products => Set<Product>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        
    }
}
