using MotoLocadora.BuildingBlocks.Entities;
using MotoLocadoraBuildingBlocks.Entities;
using System.Linq.Expressions;

namespace MotoLocadoraBuildingBlocks.Interfaces;

public interface ISqlBaseRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    Task<IEnumerable<T>> FindWithIncludesAsync(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, object>>[]? includes = null,
        bool asNoTracking = true
    );

    Task<IEnumerable<T>> QueryAsync(QueryOptions<T> options);

    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);

    Task UpdateAsync(T entity);
    Task RemoveAsync(T entity);
    Task DeleteAsync(T entity);
}