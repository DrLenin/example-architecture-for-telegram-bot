using System.Text.RegularExpressions;

namespace PC.Common.Commands.ProfileCommand;

public class RegisterCommand
{
    private readonly IPersonService _personService;
    private readonly IDaDataClient _daDataClient;
    
    public RegisterCommand(IPersonService personService, IDaDataClient daDataClient)
    {
        _personService = personService;
        _daDataClient = daDataClient;
    }
    
    public async Task<CommandResult> ExecuteStartAsync(CommandRequest request, CancellationToken cancellationToken)
    {
        var person = await _personService.CreateByChatId(request.ChatId, cancellationToken);
        
        await _personService.SetName(person, request.UserName, request.FirstName, request.LastName, cancellationToken);
        await _personService.SetCurrentCommandStep(person, nameof(ISetNameCommand), cancellationToken);

        return new MessageResult(request.ChatId, RegisterMessage + SetNameCommand.MechanicalMessage,
            ParseMode.Markdown);
    }
    
    public async Task<CommandResult> ExecuteFinishAsync(CommandRequest request, CancellationToken cancellationToken)
    {
        var date = request.Message.Split(' ');

        if (date.Length != 2)
            return new MessageResult(request.ChatId, RegisterErrorMessage, ParseMode.Markdown);
        
        if (request.Message.Contains("test@test.ru Password123", StringComparison.OrdinalIgnoreCase))
            return new MessageResult(request.ChatId, RegisterErrorCloneMessage, ParseMode.Markdown);

        var email = await _daDataClient.CheckEmail(date[0]);
        
        if (email.qc != "0")
            return new MessageResult(request.ChatId, RegisterErrorEmailMessage, ParseMode.Markdown);
        
        if (!Regex.IsMatch(date[1], PatternRegex))
            return new MessageResult(request.ChatId, RegisterErrorPasswordMessage, ParseMode.Markdown);
        
        //await _personService.SetEmail(request.Person, email.email, cancellationToken);
        //await _personService.SetPassword(request.Person, date[1], cancellationToken);
        await _personService.SetName(request.Person, request.UserName, request.FirstName, request.LastName, cancellationToken);
        await _personService.SetCurrentCommandStep(request.Person, nameof(ISetFullStrAddressCommand), cancellationToken);
        
        return new MessageResult(request.ChatId, SetFullStrAddressCommand.MechanicalMessage,
            ParseMode.Markdown);
    }

    #region string
    private const string RegisterMessage = "Добро пожаловать в Почтовичок.\n\nДля того чтобы начать обмен, необходимо заполнить информацию о себе.\n";
    private const string RegisterErrorMessage = "Вы ввели некорректные данные.\n" +
                                                "Попробуйте ещё раз.\n" +
                                                "Пример:\ntest@test.ru pAssword123";
    private const string RegisterErrorEmailMessage = "Некорректный логин.\n" +
                                                     "Попробуйте ещё раз.\n" +
                                                     "Пример:\ntest@test.ru pAssword123";
    private const string RegisterErrorPasswordMessage = "Некорректный пароль.\n" +
                                                        "Пароль должен быть длиной от 8 до 15 символов, содержать как минимум одно число и одну английскую букву.\n" +
                                                        "Попробуйте ещё раз.\n" +
                                                        "Пример:\ntest@test.ru pAssword123";
    private const string RegisterErrorCloneMessage = "Вы ввели данные указанные в примере, попробуйте ещё раз.";
    private const string PatternRegex = @"^(?=.*[aA-zZ])(?=.*\d).{8,15}$";
    #endregion
}