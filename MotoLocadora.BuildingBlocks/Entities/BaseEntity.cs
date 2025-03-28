namespace MotoLocadoraBuildingBlocks.Entities;

public class BaseEntity
{
    public Guid Id { get; set; }
    public bool Active { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? ChangedBy { get; set; }
    public DateTime ChangedAt { get; set; }

}
