namespace PC.Common.Commands.AddressCommands;

public interface IChangePostalCodeCommand : IBaseManualCommand, IBaseMechanicalCommand {}

public class ChangePostalCodeCommand : IChangePostalCodeCommand
{
    private readonly IPersonService _personService;
    private readonly IAddressService _addressService;

    public ChangePostalCodeCommand(IPersonService personService, IAddressService addressService)
    {
        _personService = personService;
        _addressService = addressService;
    }

    public async Task<CommandResult> ExecuteManualAsync(IManualRequest request, CancellationToken cancellationToken)
    {
        var address = await _addressService.SetPostalCode(request.Person.ActualAddress, request.Message, cancellationToken);
        
        await _personService.SetCurrentCommandStep(request.Person, nameof(ISetFullStrAddressCommand), cancellationToken);
        
        return new MessageResult(request.ChatId, $"{CheckAddressPlease}{Environment.NewLine}\"{address.FullStrAddress}\"",
            ParseMode.Markdown, _buttons.ToKeyboard());
    }

    public async Task<CommandResult> ExecuteMechanicalAsync(IMechanicRequest request, CancellationToken cancellationToken)
    {
        await _personService.SetCurrentCommandStep(request.Person, nameof(IChangePostalCodeCommand), cancellationToken);
        
        return new MessageResult(request.ChatId, MechanicalMessage, ParseMode.Markdown);
    }

    #region string
    public const string MessageButton = "Изменить почтовый индекс";
    private const string MechanicalMessage = "Укажите индекс почтового отделения.";
    private const string CheckAddressPlease = "Проверьте, пожалуйста, верно ли указан адрес?";
    private readonly InlineKeyboardButton[] _buttons =
    {
        InlineKeyBoardService.AddressIsTrueButton, 
        InlineKeyBoardService.AddressIsWrongButton, 
        InlineKeyBoardService.ChangePostalCodeButton
    };
    #endregion
}