namespace PC.Common.Settings;

public class DaDataSettings
{
    [Required]
    public string Token { get; set; } = null!;

    [Required]
    public string Secret { get; set; } = null!;
}