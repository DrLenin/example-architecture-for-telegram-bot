namespace PC.Common.Commands.Base;

public interface IBaseMechanicalCommand
{
    /// <summary>
    /// В mechanical переходим если пользователь взаимодействовал кнопками -> mechanical может вызывать как mechanical так и manual
    /// </summary>
    public Task<CommandResult> ExecuteMechanicalAsync(IMechanicRequest commandRequest, CancellationToken cancellationToken);
}