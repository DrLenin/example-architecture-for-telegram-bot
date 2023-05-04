namespace PC.Common.Commands.ProfileCommand;

public interface ISetBiographyCommand : IBaseMechanicalCommand, IBaseManualCommand {}

public class SetBiographyCommand : ISetBiographyCommand
{
    private readonly IPersonService _personService;
    
    public SetBiographyCommand(IPersonService personService)
    {
        _personService = personService;
    }
    
    public async Task<CommandResult> ExecuteManualAsync(IManualRequest request, CancellationToken cancellationToken)
    {
        var countWord = request.Message.Split(' ').Length;
        
        if(countWord < 6)
            return new MessageResult(request.ChatId, SetBiographyErrorMessage, ParseMode.Markdown);
        
        if(request.Message.Contains("<") || request.Message.Contains(">"))
            return new MessageResult(request.ChatId, "Содержит запрещенные символы!", ParseMode.Markdown);
        
        await _personService.SetBiography(request.Person, request.Message, cancellationToken);
        
        var idRegistrationStep = request.Person.CurrentRegistrationStep == RegistrationSteps.Created;

        if (idRegistrationStep)
            await _personService.SetCurrentCommandStep(request.Person, nameof(ISetWishesCommand), cancellationToken);
        else
            await _personService.SetCurrentCommandStep(request.Person, nameof(IChangeProfileCommand), cancellationToken);
        
        return new MessageResult(request.ChatId,
            idRegistrationStep ? SetWishesCommand.SetWishesMessage : ChangeProfileCommand.MechanicalMessage,
            ParseMode.Markdown, 
            idRegistrationStep ? null : ChangeProfileCommand.ChangeMenu.ToKeyboard());
    }
    
    public async Task<CommandResult> ExecuteMechanicalAsync(IMechanicRequest request, CancellationToken cancellationToken)
    {
        await _personService.SetCurrentCommandStep(request.Person, nameof(ISetBiographyCommand), cancellationToken);

        var idRegistrationStep = request.Person.CurrentRegistrationStep == RegistrationSteps.Created;
        
        return new MessageResult(request.ChatId, MechanicalMessage,
            ParseMode.Markdown, idRegistrationStep ? null : InlineKeyBoardService.ChangeProfileButton.ToKeyboard("Назад"));
    }

    #region string
    public const string MessageButton = "Указать биографию";
    public const string MechanicalMessage = "Расскажите о себе. (Символы \"<\" и \">\" запрещены)";
    private const string SetBiographyErrorMessage = "Информации недостаточно. Укажите минимум пару предложений.";
    #endregion
}