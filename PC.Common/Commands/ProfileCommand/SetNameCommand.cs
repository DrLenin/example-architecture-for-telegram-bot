namespace PC.Common.Commands.ProfileCommand;

public interface ISetNameCommand : IBaseMechanicalCommand, IBaseManualCommand {}

public class SetNameCommand : ISetNameCommand
{
    private readonly IPersonService _personService;
    private readonly IDaDataClient _daDataClient;

    public SetNameCommand(IPersonService personService, IDaDataClient daDataClient)
    {
        _personService = personService;
        _daDataClient = daDataClient;
    }

    public async Task<CommandResult> ExecuteManualAsync(IManualRequest request, CancellationToken cancellationToken)
    {
        var fulName = await _daDataClient.CheckFullname(request.Message);

        if (fulName.qc != "0")
            return new MessageResult(request.ChatId, FulNameWrong, ParseMode.Markdown);

        await _personService.SetFullName(request.Person, fulName.result, cancellationToken);

        var idRegistrationStep = request.Person.CurrentRegistrationStep == RegistrationSteps.Created;

        var messageResult = new MessageResult(request.ChatId,
            idRegistrationStep ? SetBiographyCommand.MechanicalMessage : ChangeProfileCommand.MechanicalMessage,
            ParseMode.Markdown,
            idRegistrationStep
                ? null
                : ChangeProfileCommand.ChangeMenu.ToKeyboard());

         if (!idRegistrationStep)
            await _personService.SetCurrentCommandStep(request.Person, nameof(ChangeProfileCommand), cancellationToken);
        
        if (!idRegistrationStep)
            return messageResult;

        await _personService.SetCurrentCommandStep(request.Person, nameof(ISetBiographyCommand),
            cancellationToken);

        return messageResult;
    }

    public async Task<CommandResult> ExecuteMechanicalAsync(IMechanicRequest request,
        CancellationToken cancellationToken)
    {
        await _personService.SetCurrentCommandStep(request.Person, nameof(ISetNameCommand), cancellationToken);

        var idRegistrationStep = request.Person.CurrentRegistrationStep == RegistrationSteps.Created;
        
        return new MessageResult(request.ChatId, MechanicalMessage, ParseMode.Markdown, idRegistrationStep ? null : InlineKeyBoardService.ChangeProfileButton.ToKeyboard("Назад"));
    }

    #region string

    public const string MessageButton = "Указать получателя";
    public const string MechanicalMessage = "Укажите имя и фамилию получателя";
    private const string FulNameWrong = "Имя и фамилия указанные получателя не распознанны. Попробуйте ещё раз.";

    #endregion
}