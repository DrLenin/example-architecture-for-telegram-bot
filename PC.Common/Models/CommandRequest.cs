namespace PC.Common.Models;

public interface IBaseRequest
{
    Person Person { get; set; }
    
    public long ChatId { get; set; }
}

public interface IMechanicRequest : IBaseRequest
{
    string CommandName { get; set; } 
    
    string? Data { get; set; } 
}

public interface IManualRequest : IBaseRequest
{
    string Message { get; set; }
}

public class CommandRequest : IMechanicRequest, IManualRequest
{
    public long ChatId { get; set; }
    
    public Person Person { get; set; } = null!;

    public string CommandName { get; set; } = null!;
    
    public string? UserName { get; set; }
    
    public string? FirstName { get; set; }
    
    public string? LastName { get; set; } 
    
    public string? Data { get; set; }
    
    public bool IsMechanicalCommand { get; set; }

    public string Message { get; set; } = null!;
    
    public bool PersonIsExists { get; set; }
    
    public bool IsUnknownCommand { get; set; }
    
    public int MessageId { get; set; }
}