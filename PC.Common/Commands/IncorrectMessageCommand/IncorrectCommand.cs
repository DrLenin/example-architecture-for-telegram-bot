namespace PC.Common.Commands.IncorrectMessageCommand;

public interface IIncorrectCommand : IBaseManualCommand {}

public class IncorrectCommand : IIncorrectCommand
{
    public Task<CommandResult> ExecuteManualAsync(IManualRequest request, CancellationToken cancellationToken)
        => Task.FromResult<CommandResult>(new MessageResult(request.ChatId, ManualMessage,
            ParseMode.Markdown, InlineKeyBoardService.MainMenuButton.ToKeyboard()));
    
    #region string
    private const string ManualMessage = "Мы не можем обработать этот запрос.\nОбратитесь в поддержку или перейдите в следующие разделы.";
    #endregion
}