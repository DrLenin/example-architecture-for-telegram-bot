using System.ComponentModel.Design;
using System.Text;
using PC.Common.Commands.IncorrectMessageCommand;
using PC.Common.Commands.ProfileCommand;
using PC.Common.PassingType;

namespace PC.Common.Services;

public interface ICommandService
{
    Task Execute(CommandRequest update, CancellationToken cancellationToken);
}

public class CommandService: ICommandService
{
    private readonly RegisterCommand _exclusiveCommand;
    private readonly TelegramBotClient _telegramBotClient;
    private readonly IPersonService _personService;
    private readonly ICommandResolver _commandResolver;
    private readonly IIncorrectCommand _incorrectCommand;

    public CommandService(ICommandResolver commandResolver, RegisterCommand exclusiveCommand,
        TelegramBotClient telegramBotClient, IPersonService personService, IIncorrectCommand incorrectCommand)
    {
        _commandResolver = commandResolver;
        _exclusiveCommand = exclusiveCommand;
        _telegramBotClient = telegramBotClient;
        _personService = personService;
        _incorrectCommand = incorrectCommand;
    }

    public async Task Execute(CommandRequest commandRequest, CancellationToken cancellationToken)
    {
        if (commandRequest.IsUnknownCommand)
            return;
        
        try
        {
            CommandResult commandResult;
            
            if (commandRequest.IsMechanicalCommand)
            {
                await _telegramBotClient.EditMessageReplyMarkupAsync(commandRequest.ChatId, commandRequest.MessageId, null, cancellationToken: cancellationToken);
                commandResult = await ExecuteMechanicalCommand(commandRequest, cancellationToken);
            }
            else
            {
                if (!commandRequest.PersonIsExists)
                {
                    await NotifyChat(await _exclusiveCommand.ExecuteStartAsync(commandRequest, cancellationToken));
                    return;
                }

                if (commandRequest.Person.Username == string.Empty && commandRequest.Person.FirstName == string.Empty
                                                                   && commandRequest.Person.LastName == string.Empty)
                    await _personService.SetName(commandRequest.Person, commandRequest.UserName,
                        commandRequest.FirstName, commandRequest.LastName, cancellationToken);
                
                commandResult = await ExecuteManualCommand(commandRequest, cancellationToken);
            }

            await NotifyChat(commandResult);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            await NotifyChat(await _incorrectCommand.ExecuteManualAsync(commandRequest, cancellationToken));
        }
    }

    private async Task NotifyChat(CommandResult commandResult)
    {   
        if (commandResult.NeedSendMessage)
        {
            var message = commandResult.MessageResult;
            var messageCount = Math.Ceiling(Encoding.Unicode.GetByteCount(message.Text) / 3000M);
            var chunkSize = (int)Math.Truncate(message.Text.Length / messageCount);

            if (messageCount > 1)
            {
                for (var i = 0; i < messageCount; i++)
                {
                    var start = i * chunkSize;
                    var isLast = i + 1 == messageCount;
                    var length = !isLast ? chunkSize : message.Text.Length - start;
                    
                    var cutMessage = message.Text.Substring(start, length);
                    var buttons = !isLast ? null : message.ReplyMarkup;
                    
                    await _telegramBotClient.SendTextMessageAsync(message.ChatId, cutMessage, null,
                        message.ParseMode, replyMarkup: buttons);
                }
                
                return;
            }

            await _telegramBotClient.SendTextMessageAsync(message.ChatId, message.Text, null,
                message.ParseMode, replyMarkup: message.ReplyMarkup);
        }
    }

    private async Task<CommandResult> ExecuteMechanicalCommand(IMechanicRequest mechanicRequest, CancellationToken cancellationToken)
    {
        var command = _commandResolver.GetDefaultCommand<IBaseMechanicalCommand>(mechanicRequest.CommandName);
        return await command.ExecuteMechanicalAsync(mechanicRequest, cancellationToken);
    }
    
    private async Task<CommandResult> ExecuteManualCommand(IManualRequest manualRequest, CancellationToken cancellationToken)
    {
        var command = _commandResolver.GetDefaultCommand<IBaseManualCommand>(manualRequest.Person.CurrentCommandStep);
        return await command.ExecuteManualAsync(manualRequest, cancellationToken);
    }
}