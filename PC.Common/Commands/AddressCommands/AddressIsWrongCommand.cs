namespace PC.Common.Commands.AddressCommands;

public interface IAddressIsWrongCommand : IBaseMechanicalCommand {}

public class AddressIsWrongCommand : IAddressIsWrongCommand
{
    public Task<CommandResult> ExecuteMechanicalAsync(IMechanicRequest request, CancellationToken cancellationToken) => 
        Task.FromResult<CommandResult>(new MessageResult(request.ChatId, MechanicalMessage, ParseMode.Markdown));

    #region string
    public const string MessageButton = "Нет";
    private const string MechanicalMessage = "Пожалуйста, укажите корректный адрес или обратитесь в поддержку.";    
    #endregion
}