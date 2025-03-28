namespace MotoLocadoraBuildingBlocks.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; } 
    public bool Active { get; set; } = true;
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? ChangedBy { get; set; }
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}
