namespace PC.Common.Commands.Base;

public interface IBaseManualCommand
{
    /// <summary>
    /// В мануал переходим, если пользователь ввёл что-то руками
    /// -> следующую команду вызывает mechanical
    /// -> manual никогда не вызывает следующий manual, исключение exclusive
    /// </summary>
    public Task<CommandResult> ExecuteManualAsync(IManualRequest update, CancellationToken cancellationToken);
}