namespace PC.Common.Models;

public class CommandResult
{
    public InlineKeyboardMarkup? Keyboard { get; private set; }
    public MessageResult MessageResult { get; private set; }

    public bool NeedSendMessage { get; private set; }

    public bool NeedSendKeyboard => Keyboard != null;
   
    public static CommandResult InlineKeyboard(InlineKeyboardMarkup keyboard) => new() {Keyboard = keyboard};
    
    public static CommandResult Message(MessageResult messageResult) => new() {MessageResult = messageResult, NeedSendMessage = true};

    
    public static implicit operator CommandResult(InlineKeyboardMarkup keyboard) => new() {Keyboard = keyboard};

    
    public static implicit operator CommandResult(MessageResult messageResult) => new() {MessageResult = messageResult, NeedSendMessage = true};

}

public class MessageResult
{
    public long ChatId { get; }
    public string Text { get; }
    public ParseMode? ParseMode { get; }
    public IReplyMarkup? ReplyMarkup { get; }

    public MessageResult(long chatId, string text, ParseMode? parseMode = default, IReplyMarkup? replyMarkup = default)
    {
        ChatId = chatId;
        Text = text;
        ParseMode = parseMode;
        ReplyMarkup = replyMarkup;
    }
}
