namespace PC.Models.Entities.Base;

public class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;

    public bool IsDeleted { get; set; }
}