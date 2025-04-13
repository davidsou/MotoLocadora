using Microsoft.EntityFrameworkCore;
using MotoLocadora.BuildingBlocks.Entities;
using MotoLocadora.BuildingBlocks.Interfaces;
using System.Linq.Expressions;
using System.Reflection;

namespace MotoLocadora.BuildingBlocks.Repositories;

public class SqlBaseRepository<T>(DbContext context) : ISqlBaseRepository<T> where T : BaseEntity
{
    protected readonly DbSet<T> dbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(int id) => await dbSet.FindAsync(id);

    public async Task<IEnumerable<T>> GetAllAsync() => await dbSet.Where(e => e.Active).ToListAsync();

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await dbSet.Where(e => e.Active).Where(predicate).ToListAsync();
    }

    public async Task<IEnumerable<T>> FindWithIncludesAsync(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>>[]? includes = null,
        bool asNoTracking = true)
    {
        IQueryable<T> query = dbSet.Where(e => e.Active).Where(predicate);

        if (includes != null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        if (asNoTracking)
            query = query.AsNoTracking();

        return await query.ToListAsync();
    }

    public async Task<IEnumerable<T>> QueryAsync(QueryOptions<T> options)
    {
        IQueryable<T> query = dbSet.Where(e => e.Active);

        if (options.Filter != null)
            query = query.Where(options.Filter);

        if (options.Includes != null && options.Includes.Any())
        {
            foreach (var include in options.Includes)
            {
                query = query.Include(include);
            }
        }

        if (!string.IsNullOrWhiteSpace(options.OrderBy))
        {
            var param = Expression.Parameter(typeof(T), "x");
            var property = typeof(T).GetProperty(options.OrderBy!, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property != null)
            {
                var body = Expression.Property(param, property);
                var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(body, typeof(object)), param);
                query = options.OrderDescending ? query.OrderByDescending(lambda) : query.OrderBy(lambda);
            }
        }

        if (options.Skip.HasValue)
            query = query.Skip(options.Skip.Value);

        if (options.Take.HasValue)
            query = query.Take(options.Take.Value);

        if (options.AsNoTracking)
            query = query.AsNoTracking();

        return await query.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        entity.Active = true;
        entity.CreatedAt = DateTime.UtcNow;
        await dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        var now = DateTime.UtcNow;
        foreach (var entity in entities)
        {
            entity.Active = true;
            entity.CreatedAt = now;
        }
        await dbSet.AddRangeAsync(entities);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        entity.ChangedAt = DateTime.UtcNow;
        dbSet.Update(entity);
        await context.SaveChangesAsync();        
    }

    public async Task RemoveAsync(T entity)
    {
        entity.Active = false;
        entity.ChangedAt = DateTime.UtcNow;
        dbSet.Update(entity);
        await context.SaveChangesAsync();        
    }

    public async Task DeleteAsync(T entity)
    {
        dbSet.Remove(entity);
        await context.SaveChangesAsync();
        
    }
    public async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null)
    {
        IQueryable<T> query = dbSet.Where(e => e.Active);
        if (filter != null)
            query = query.Where(filter);

        return await query.CountAsync();
    }
}
