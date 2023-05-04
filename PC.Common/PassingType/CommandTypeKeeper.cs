namespace PC.Common.PassingType;

public interface ICommandTypeKeeper
{
    Type? GetTypeByName(string name);
}

public class CommandTypeKeeper : ICommandTypeKeeper
{
    private readonly Type[] _commandTypes;

    public CommandTypeKeeper()
    {
        _commandTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => p.GetInterfaces().Any(x => x == typeof(IBaseMechanicalCommand) || x == typeof(IBaseManualCommand)) && p.IsInterface)
            .ToArray();
    }

    public Type? GetTypeByName(string name)
    {
        return _commandTypes.FirstOrDefault(x => x.Name == name);
    }
}