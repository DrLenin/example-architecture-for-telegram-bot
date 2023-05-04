using PC.Common.Commands.IncorrectMessageCommand;

namespace PC.Common.PassingType;

public interface ICommandResolver
{
    T GetDefaultCommand<T>(string commandName);
}

public class CommandResolver : ICommandResolver
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ICommandTypeKeeper _commandTypeKeeper;

    public CommandResolver(IServiceProvider serviceProvider, ICommandTypeKeeper commandTypeKeeper)
    {
        _serviceProvider = serviceProvider;
        _commandTypeKeeper = commandTypeKeeper;
    }

    public T GetDefaultCommand<T>(string commandName)
    {
        var type = _commandTypeKeeper.GetTypeByName(commandName);

        if (type is null)
            return (T)_serviceProvider.GetService(typeof(IIncorrectCommand))!;
        
        if (typeof(T) == typeof(IBaseManualCommand) && !type.GetInterfaces().Contains(typeof(IBaseManualCommand)))
            return (T)_serviceProvider.GetService(typeof(IIncorrectCommand))!;

        return (T)_serviceProvider.GetService(type!)!;
    }
}