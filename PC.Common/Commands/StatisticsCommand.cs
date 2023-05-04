namespace PC.Common.Commands;

public interface IStatisticsCommand : IBaseMechanicalCommand {}

public class StatisticsCommand : IStatisticsCommand
{
    private readonly IPersonService _personService;
    private readonly IPostCardService _postCardService;

    public StatisticsCommand(IPersonService personService, IPostCardService postCardService)
    {
        _personService = personService;
        _postCardService = postCardService;
    }

    public async Task<CommandResult> ExecuteMechanicalAsync(IMechanicRequest request, CancellationToken cancellationToken)
    {
        await _personService.SetCurrentCommandStep(request.Person, nameof(IStatisticsCommand), cancellationToken);

        var senderPostCard = await _postCardService.CountForSenderPostCard(request.Person.Id, cancellationToken);
        var receiverPostCard = await _postCardService.CountForReceiverPostCard(request.Person.Id, cancellationToken);
        
        return new MessageResult(request.ChatId,   string.Format(_template, senderPostCard.countCreated, senderPostCard.countSent, senderPostCard.countReceived,
                receiverPostCard.countCreated, receiverPostCard.countSent, receiverPostCard.countReceived), ParseMode.Markdown, InlineKeyBoardService.MainMenuButton.ToKeyboard());
    }

    #region string
    public const string MessageButton = "Статистика";
    private readonly string _template = $"Статистика отправлений\n- ожидают отправления: {{0}}\n- в пути: {{1}}\n- получено: {{2}}\n\nСтатистика получений\n- ожидают отправления: {{3}}\n- в пути: {{4}}\n- получено: {{5}}";
    #endregion
}