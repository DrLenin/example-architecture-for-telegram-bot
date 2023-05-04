namespace PC.Common.Services;

public static class InlineKeyBoardService
{
    #region Seter
    public static readonly InlineKeyboardButton SetBiographyButton = 
        new(SetBiographyCommand.MessageButton) {CallbackData = nameof(ISetBiographyCommand)};
    
    public static readonly InlineKeyboardButton SetFullStrAddressButton = 
        new(SetFullStrAddressCommand.MessageButton) {CallbackData = nameof(ISetFullStrAddressCommand)};
    
    public static readonly InlineKeyboardButton SetWishesButton = 
        new(SetWishesCommand.MessageButton) {CallbackData = nameof(ISetWishesCommand)};
    
    public static readonly InlineKeyboardButton SetMailboxButton = 
        new(SetMailboxCommand.MessageButton) {CallbackData = nameof(ISetMailboxCommand)};
    
    public static readonly InlineKeyboardButton SetToNeedButton = 
        new(SetToNeedCommand.MessageButton) {CallbackData = nameof(ISetToNeedCommand)};
    
    public static readonly InlineKeyboardButton SetNameButton = 
        new(SetNameCommand.MessageButton) {CallbackData = nameof(ISetNameCommand)};
    #endregion
    
    #region Change
    public static readonly InlineKeyboardButton ChangePostalCodeButton = 
        new(ChangePostalCodeCommand.MessageButton) {CallbackData = nameof(IChangePostalCodeCommand)};
    public static readonly InlineKeyboardButton ChangeProfileButton = 
        new(ChangeProfileCommand.MessageButton) {CallbackData = nameof(IChangeProfileCommand)};
    #endregion
    
    #region IsAsk
    public static readonly InlineKeyboardButton AddressIsTrueButton =
        new(AddressIsTrueCommand.MessageButton) {CallbackData = nameof(IAddressIsTrueCommand)};

    public static readonly InlineKeyboardButton AddressIsWrongButton 
        = new(AddressIsWrongCommand.MessageButton) {CallbackData = nameof(IAddressIsWrongCommand)};
    #endregion
    
    public static readonly InlineKeyboardButton MainMenuButton = 
        new(MainMenuCommand.MessageButton) {CallbackData = nameof(IMainMenuCommand)};

    public static readonly InlineKeyboardButton SwitchAddressButton =
        new(SwitchAddressCommand.MessageButton) { CallbackData = nameof(ISwitchAddressCommand) };
    
    public static readonly InlineKeyboardButton StatisticsButton =
        new(StatisticsCommand.MessageButton) { CallbackData = nameof(IStatisticsCommand) };
    
    public static InlineKeyboardMarkup ToKeyboard(this InlineKeyboardButton[] buttons)
    {
        return buttons.Length switch
        {
            > 6 => new InlineKeyboardMarkup(new[]
            {
                buttons[..2],
                buttons[2..4],
                buttons[4..6],
                buttons[buttons.Length..],
            }),
            > 4 => new InlineKeyboardMarkup(new[]
            {
                buttons[..2],
                buttons[2..4],
                buttons[4..buttons.Length],
            }),
            > 2 => new InlineKeyboardMarkup(new[]
            {
                buttons[..2],
                buttons[2..buttons.Length],
            }),
            <= 2 => new InlineKeyboardMarkup(new[]
            {
                buttons,
            })
        };
    }
    
    public static InlineKeyboardMarkup ToKeyboard(this InlineKeyboardButton button, string? text = null)
    {
        text ??= button.Text;
        
        var newButton = new InlineKeyboardButton(text) {CallbackData = button.CallbackData};
        
        return new InlineKeyboardMarkup(new [] {newButton});
    }
    
    // public static InlineKeyboardMarkup ConcatKeyBoard(this InlineKeyboardMarkup a, InlineKeyboardMarkup b)
    // {
    //     var buttons = a.InlineKey
    // }
}