using System.Text;

namespace PC.Models.Entities;

public class Person : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    
    public string FirstName { get; set; } = string.Empty;
    
    public string LastName { get; set; } = string.Empty; 
    
    public string? ReceiverName { get; set; } = string.Empty; 
 
    public long? ChatId { get; set; }
    
    public string? Email { get; set; }
    
    public string? Password { get; set; }
    
    public string Bio { get; set; } = string.Empty;

    public string Wishes { get; set; } = string.Empty;

    public RegistrationSteps CurrentRegistrationStep { get; set; } = RegistrationSteps.Created;
    
    public string CurrentCommandStep { get; set; } = string.Empty;

    [NotMapped]
    public Address ActualAddress => Addresses.First(x => !x.IsDeleted);
    
    public List<Address> Addresses { get; set; } = new();

    public virtual List<PostCard> SendingPostCards { get; set; } = new();
    
    public virtual List<PostCard> ReceivePostCards { get; set; } = new();

    public override string ToString()
    {
        var strBuilder = new StringBuilder();

        var name = ReceiverName ?? FirstName + " " + LastName;
        
        strBuilder.Append($"{Environment.NewLine}Информация о пользователе");
        strBuilder.AppendLine($"{Environment.NewLine}{Environment.NewLine}О себе:");
        strBuilder.AppendLine($"{Bio}");
        strBuilder.AppendLine($"{Environment.NewLine}Хотелось бы получить:");
        strBuilder.AppendLine($"{Wishes}");
        strBuilder.AppendLine($"{Environment.NewLine}Имя получателя: {name}");
        
        return strBuilder.ToString();
    }
}