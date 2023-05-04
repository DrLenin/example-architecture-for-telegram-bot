namespace PC.Common.Commands.ProfileCommand;

public interface IChangeProfileCommand : IBaseMechanicalCommand {}

public class ChangeProfileCommand : IChangeProfileCommand
{
    private readonly IPersonService _personService;

    public ChangeProfileCommand(IPersonService personService)
    {
        _personService = personService;
    }
    
    public async Task<CommandResult> ExecuteMechanicalAsync(IMechanicRequest request, CancellationToken cancellationToken)
    {
        await _personService.SetCurrentCommandStep(request.Person, nameof(IChangeProfileCommand), cancellationToken);
        
        return new MessageResult(request.ChatId, request.Person 
                                                 + $"{Environment.NewLine}Адрес: {request.Person.ActualAddress.FullStrAddress}",
            ParseMode.Html, ChangeMenu.ToKeyboard());
    } 

    #region string
    public const string MessageButton = "Изменить профиль";
    public const string MechanicalMessage = "Окно редактирования.\n";
    public static readonly InlineKeyboardButton[] ChangeMenu =
    {
        InlineKeyBoardService.SetBiographyButton,
        InlineKeyBoardService.SetWishesButton,
        InlineKeyBoardService.SwitchAddressButton,
        InlineKeyBoardService.SetNameButton,
        InlineKeyBoardService.MainMenuButton,
    };
    #endregion
}