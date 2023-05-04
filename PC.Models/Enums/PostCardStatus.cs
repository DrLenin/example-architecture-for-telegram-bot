namespace PC.Models.Enums;

public enum PostCardStatus
{
    [Display(Name = "Создано")]
    Created,
    
    [Display(Name = "Отправлено")]
    Sent,
    
    [Display(Name = "Дошло")]
    Received,
    
    [Display(Name = "Просрочено")]
    Overdue
}