namespace PC.Common.Settings;

public class TelegramSettings
{
    [Required]
    public string Token { get; set; } = string.Empty;
    
    [Required]
    public string Url { get; set; } = string.Empty;
}