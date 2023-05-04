namespace PC.Common.Mapping;

public class EnrichCommandRequest : IMappingAction<Update, CommandRequest>
{
    private readonly IPersonRepository _personRepository;

    public EnrichCommandRequest(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }
    
    public void Process(Update source, CommandRequest destination, ResolutionContext context)
    {
        if(destination.IsUnknownCommand)
            return;
        
        var message = destination.IsMechanicalCommand 
            ? source.CallbackQuery!.Message 
            : source.Message;
        
        var chatId = message!.Chat.Id;

        destination.Message = message.Text!;
        
        if (destination.IsMechanicalCommand)
        {
            var date = source.CallbackQuery!.Data!.Split('_');

            destination.CommandName = date[0];
            
            if(date.Length > 1)
                destination.Data = date[1];
        }
        
        destination.MessageId = message.MessageId;
        
        destination.UserName = message.Chat.Username;
        destination.FirstName = message.Chat.FirstName;
        destination.LastName = message.Chat.LastName;
        
        destination.ChatId = chatId;
        
        var person = _personRepository.GetByChatId(chatId).Result;

        if (person == null) return;
        
        destination.PersonIsExists = true;
        destination.Person = person;
    }
}