namespace PC.Common.Commands.AddressCommands;

public interface ISetFullStrAddressCommand : IBaseMechanicalCommand, IBaseManualCommand {}

public class SetFullStrAddressCommand : ISetFullStrAddressCommand
{
    private readonly IPersonService _personService;
    private readonly IDaDataClient _daDataClient;

    public SetFullStrAddressCommand(IPersonService personService, IDaDataClient daDataClient)
    {
        _personService = personService;
        _daDataClient = daDataClient;
    }
    
    public async Task<CommandResult> ExecuteManualAsync(IManualRequest request, CancellationToken cancellationToken)
    {
        AddressDaData addressResult;
        
        try
        {
            addressResult = await _daDataClient.CheckAddress(request.Message);
        }
        catch (Exception)
        {
            return new MessageResult(request.ChatId, ErrorMessage, ParseMode.Markdown, InlineKeyBoardService.SwitchAddressButton.ToKeyboard("Назад"));
        }

        if(addressResult.qc_house == "10" && int.Parse(addressResult.qc_geo) >= 2)
            return new MessageResult(request.ChatId, AddressWrong, ParseMode.Markdown, InlineKeyBoardService.SwitchAddressButton.ToKeyboard("Назад"));
        
        var address = new Address
        {
            Country = addressResult.country,
            City = addressResult.city ?? addressResult.region_with_type, 
            FullAddress = addressResult.result,
            PostalCode = addressResult.postal_code,
            PersonId = request.Person.Id
        };
        
        await _personService.SetAddress(request.Person, address, cancellationToken);

        return new MessageResult(request.ChatId, $"{CheckAddressPlease}{Environment.NewLine}\"{address.FullStrAddress}\"",
            ParseMode.Html, _buttons.ToKeyboard());
    }
    
    public async Task<CommandResult> ExecuteMechanicalAsync(IMechanicRequest request, CancellationToken cancellationToken)
    {
        await _personService.SetCurrentCommandStep(request.Person, nameof(ISetFullStrAddressCommand), cancellationToken);

        return new MessageResult(request.ChatId, MechanicalMessage, ParseMode.Markdown, InlineKeyBoardService.SwitchAddressButton.ToKeyboard("Назад"));
    }

    #region string
    public const string MessageButton = "Почтовый адрес";
    public const string MechanicalMessage = "Укажите полный адрес, например \"Москва ул Пушкина д 12 кв 13\".";
    public const string CheckAddressPlease = "Проверьте, пожалуйста, верно ли указан адрес?";
    private const string AddressWrong = "Вероятность доставки письма низкая! Укажите существующий адрес.";
    public const string ErrorMessage = "Ошибка во время стандартизации адреса, попробуйте ещё раз или обратитесь в поддержку.";
    private readonly InlineKeyboardButton[] _buttons =
    {
        InlineKeyBoardService.AddressIsTrueButton, 
        InlineKeyBoardService.AddressIsWrongButton, 
        InlineKeyBoardService.ChangePostalCodeButton
    };
    #endregion
}