namespace PC.Common.Commands.AddressCommands;

public interface ISwitchAddressCommand : IBaseMechanicalCommand {}

public class SwitchAddressCommand : ISwitchAddressCommand
{
    public Task<CommandResult> ExecuteMechanicalAsync(IMechanicRequest request, CancellationToken cancellationToken)
    {
        var end = new InlineKeyboardButton("Назад") { CallbackData = nameof(IChangeProfileCommand) };
        
        var buttons = Buttons.Append(end).ToArray();
        
        return Task.FromResult<CommandResult>(new MessageResult(request.ChatId,MechanicalMessage, ParseMode.Markdown,
            buttons.ToKeyboard()));
    }

    #region string
    public const string MessageButton = "Изменить адрес";
    public const string MechanicalMessage = "Выберите какой адрес вы хотите указать";
    public static readonly InlineKeyboardButton[] Buttons =
    {
        InlineKeyBoardService.SetFullStrAddressButton, 
        InlineKeyBoardService.SetMailboxButton,
        InlineKeyBoardService.SetToNeedButton,
    };
    #endregion
}