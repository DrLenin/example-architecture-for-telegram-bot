namespace PC.Common.Commands.AddressCommands;

public interface IAddressIsTrueCommand : IBaseMechanicalCommand {}

public class AddressIsTrueCommand : IAddressIsTrueCommand
{
    private readonly IPersonService _personService;

    public AddressIsTrueCommand(IPersonService personService)
    {
        _personService = personService;
    }
    
    public async Task<CommandResult> ExecuteMechanicalAsync(IMechanicRequest request, CancellationToken cancellationToken)
    {
        var isRegistrationStep = request.Person.CurrentRegistrationStep == RegistrationSteps.Created;

        if(!isRegistrationStep)
            return new MessageResult(request.ChatId, ChangeProfileCommand.MechanicalMessage,
                ParseMode.Markdown, ChangeProfileCommand.ChangeMenu.ToKeyboard()); 
        
        await _personService.SetCurrentCommandStep(request.Person, nameof(IMainMenuCommand), cancellationToken);
        await _personService.SetRegisterStep(request.Person, RegistrationSteps.Registered, cancellationToken);
        
        return new MessageResult(request.ChatId, MechanicalMessage,
            ParseMode.Markdown, InlineKeyBoardService.MainMenuButton.ToKeyboard());
    }
    
    #region string
    public const string MessageButton = "Да";
    private const string MechanicalMessage = "Регистрация пройдена.";
    #endregion
}