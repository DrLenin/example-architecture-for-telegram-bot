namespace PC.Common.Commands;

public interface IMainMenuCommand : IBaseMechanicalCommand {}

public class MainMenuCommand : IMainMenuCommand
{
    private readonly IPersonService _personService;

    public MainMenuCommand(IPersonService personService)
    {
        _personService = personService;
    }

    public async Task<CommandResult> ExecuteMechanicalAsync(IMechanicRequest request, CancellationToken cancellationToken)
    {
        await _personService.SetCurrentCommandStep(request.Person, nameof(IMainMenuCommand), cancellationToken);
        
        return new MessageResult(request.ChatId,MainMenuMessage, ParseMode.Markdown, Buttons.ToKeyboard());
    }

    #region string
    public const string MessageButton = "Главное меню";
    public const string MainMenuMessage = "Главное меню.";
    private static readonly InlineKeyboardButton[] Buttons =
    {
        InlineKeyBoardService.StatisticsButton,
        InlineKeyBoardService.ChangeProfileButton
    };
    #endregion
}