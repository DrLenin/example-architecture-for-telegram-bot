namespace PC.Models.Entities;

public class Address : BaseEntity
{
    public Guid PersonId { get; set; }
    
    public string Country { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;
    
    public string FullAddress { get; set; } = string.Empty;
    
    public  Person Person { get; set; } = null!;
    
    public string PostalCode { get; set; } = null!;

    [NotMapped]
    public string FullStrAddress => $"{FullAddress}, {PostalCode}";
}