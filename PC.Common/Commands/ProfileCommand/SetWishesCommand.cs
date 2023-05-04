namespace PC.Common.Commands.ProfileCommand;

public interface ISetWishesCommand : IBaseMechanicalCommand, IBaseManualCommand {}

public class SetWishesCommand : ISetWishesCommand
{
    private readonly IPersonService _personService;

    public SetWishesCommand(IPersonService personService)
    {
        _personService = personService;
    }

    public async Task<CommandResult> ExecuteManualAsync(IManualRequest request, CancellationToken cancellationToken)
    {
        var countWord = request.Message.Split(' ').Length;

        if (countWord < 4)
            return new MessageResult(request.ChatId, SetWishesErrorMessage, ParseMode.Markdown);

        if(request.Message.Contains("<") || request.Message.Contains(">"))
            return new MessageResult(request.ChatId, "Содержит запрещенные символы!", ParseMode.Markdown);

        await _personService.SetWishes(request.Person, request.Message, cancellationToken);

        var idRegistrationStep = request.Person.CurrentRegistrationStep == RegistrationSteps.Created;

        var messageResult = new MessageResult(request.ChatId,
            idRegistrationStep ? SwitchAddressCommand.MechanicalMessage : ChangeProfileCommand.MechanicalMessage,
            ParseMode.Markdown,
            idRegistrationStep
                ? SwitchAddressCommand.Buttons.ToKeyboard()
                : ChangeProfileCommand.ChangeMenu.ToKeyboard());

        if (!idRegistrationStep)
            await _personService.SetCurrentCommandStep(request.Person, nameof(ChangeProfileCommand), cancellationToken);
        
        if (!idRegistrationStep)
            return messageResult;

        await _personService.SetCurrentCommandStep(request.Person, nameof(ISetFullStrAddressCommand),
            cancellationToken);

        return messageResult;
    }

    public async Task<CommandResult> ExecuteMechanicalAsync(IMechanicRequest request,
        CancellationToken cancellationToken)
    {
        await _personService.SetCurrentCommandStep(request.Person, nameof(ISetWishesCommand), cancellationToken);

        var idRegistrationStep = request.Person.CurrentRegistrationStep == RegistrationSteps.Created;
        
        return new MessageResult(request.ChatId, SetWishesMessage, ParseMode.Markdown, idRegistrationStep ? null : InlineKeyBoardService.ChangeProfileButton.ToKeyboard("Назад"));
    }

    #region string

    public const string MessageButton = "Указать предпочтения";
    public const string SetWishesMessage = "Какие открытки вы бы хотели получить? (Символы \"<\" и \">\" запрещены)";
    private const string SetWishesErrorMessage = "Информации недостаточно. Укажите минимум пару предложений.";

    #endregion
}