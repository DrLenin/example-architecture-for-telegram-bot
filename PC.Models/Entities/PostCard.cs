namespace PC.Models.Entities;

public class PostCard : BaseEntity
{
    public int Code { get; set; } 

    [NotMapped]
    public string StrCode => $"ПЧ-{Code}";
    
    public Guid SenderId { get; set; }
    
    public Person Sender { get; set; } = null!;

    public Guid ReceiverId { get; set; }
    
    public Person Receiver { get; set; } = null!;

    public Guid AddressId { get; set; }
    
    public Address Address { get; set; } = null!;

    public PostCardStatus Status { get; set; } = PostCardStatus.Created;
}