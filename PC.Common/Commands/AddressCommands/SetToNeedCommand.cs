using System.Text.RegularExpressions;

namespace PC.Common.Commands.AddressCommands;

public interface ISetToNeedCommand : IBaseMechanicalCommand, IBaseManualCommand {}

public class SetToNeedCommand : ISetToNeedCommand
{
    private readonly IPersonService _personService;
    private readonly IDaDataClient _daDataClient;

    public SetToNeedCommand(IPersonService personService, IDaDataClient daDataClient)
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
            return new MessageResult(request.ChatId, SetFullStrAddressCommand.ErrorMessage, ParseMode.Markdown, InlineKeyBoardService.SwitchAddressButton.ToKeyboard("Назад"));
        }

        var number = Regex.Match(request.Message, "([0-9]+$)");

        var city = string.IsNullOrEmpty(addressResult.city) ? addressResult.settlement_with_type : addressResult.city;

        if (string.IsNullOrEmpty(city))
            city = addressResult.region_with_type;

        if (string.IsNullOrEmpty(city) || !number.Success)
            return new MessageResult(request.ChatId, SetFullStrAddressCommand.ErrorMessage, ParseMode.Markdown, InlineKeyBoardService.SwitchAddressButton.ToKeyboard("Назад"));
        
        var address = new Address
        {
            Country = addressResult.country,
            City = city, 
            FullAddress = $"{addressResult.country}, {city}, До востребования ",
            PostalCode = number.Groups[1].Value,
            PersonId = request.Person.Id
        };
        
        await _personService.SetAddress(request.Person, address, cancellationToken);

        return new MessageResult(request.ChatId, $"{SetFullStrAddressCommand.CheckAddressPlease}{Environment.NewLine}\"{address.FullStrAddress}\"",
            ParseMode.Markdown, _buttons.ToKeyboard());
    }
    
    public async Task<CommandResult> ExecuteMechanicalAsync(IMechanicRequest request, CancellationToken cancellationToken)
    {
        await _personService.SetCurrentCommandStep(request.Person, nameof(ISetToNeedCommand), cancellationToken);

        return new MessageResult(request.ChatId, MechanicalMessage, ParseMode.Markdown, InlineKeyBoardService.SwitchAddressButton.ToKeyboard("Назад"));
    }

    #region string
    public const string MessageButton = "До востребования";
    private const string MechanicalMessage = "Укажите город с индексом почтового отделения. Например \"Москва 000000\".";
    private readonly InlineKeyboardButton[] _buttons =
    {
        InlineKeyBoardService.AddressIsTrueButton, 
        InlineKeyBoardService.AddressIsWrongButton, 
    };
    #endregion
}