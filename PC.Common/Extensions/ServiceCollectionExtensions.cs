namespace PC.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureSettings<T>(
        this IServiceCollection services,
        IConfiguration config,
        string sectionName)
        where T : class, new()
    {
        services.ConfigureSettings<T>(config, sectionName, _ => { });
    }

    private static void ConfigureSettings<T>(
        this IServiceCollection services,
        IConfiguration config,
        string sectionName,
        Action<T> configure)
        where T : class, new()
    {
        var section = config.GetSection(sectionName);
        var settings = section.Get<T>();
            
        if (settings == null)
            throw new InvalidOperationException($"{sectionName} section not found in configuration file");

        services.AddOptions<T>()
            .Bind(section)
            .Configure(configure)
            .ValidateDataAnnotations();

        var serviceProvider = services.BuildServiceProvider();
        _ = serviceProvider.GetRequiredService<IOptions<T>>().Value; //validate inside
    }
}