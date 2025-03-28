using System.Linq.Expressions;

namespace MotoLocadora.BuildingBlocks.Entities;

public class QueryOptions<T> where T : class
{
    public Expression<Func<T, bool>>? Filter { get; set; }
    public Expression<Func<T, object>>[]? Includes { get; set; }
    public string? OrderBy { get; set; }
    public bool OrderDescending { get; set; } = false;
    public int? Skip { get; set; }
    public int? Take { get; set; }
    public bool AsNoTracking { get; set; } = true;
}

