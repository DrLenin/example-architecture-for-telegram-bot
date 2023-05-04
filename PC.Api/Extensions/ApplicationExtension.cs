namespace PC.Api.Extensions;

public static class ApplicationExtension
{
    public static void RegisterApis(this WebApplication application)
    {
        foreach (var api in application.Services.GetServices<IApi>())
            api.RegisterActions(application);
    }

    public static void MigrationDb(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();

        try
        {
            var db = scope.ServiceProvider.GetRequiredService<PostCrossContext>().Database;
            db.Migrate();

            using var connection = (NpgsqlConnection)db.GetDbConnection();
        
            connection.Open();
            connection.ReloadTypes();
        }
        catch (Exception e)
        {
            throw;
        }
        
    }
    
    public static void AddStore(this IServiceCollection services, string? connectionString)
    {
        if (connectionString == null)
            return;

        services.AddDbContext<PostCrossContext>((provider, options) =>
        {
            var loggerFactory = provider.GetService<ILoggerFactory>();
            
            options.ConfigureWarnings(warning => warning
                    .Log(RelationalEventId.CommandExecuting))
                .UseLoggerFactory(loggerFactory)
                .EnableSensitiveDataLogging();
            
            options.UseNpgsql(connectionString,
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", Consts.DbSchema));
        }, ServiceLifetime.Scoped, ServiceLifetime.Singleton);

        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IPostCardRepository, PostCardRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
    }

    public static void ConfigureSettings(this IServiceCollection services, IConfiguration config)
    {
        services.ConfigureSettings<TelegramSettings>(config, nameof(TelegramSettings));
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<RegisterCommand, RegisterCommand>();
        services.AddSingleton<ICommandTypeKeeper, CommandTypeKeeper>();
        services.AddScoped<IDaDataClient, DaDataClient>();
        services.AddScoped<ICommandResolver, CommandResolver>();
        services.AddScoped<EnrichCommandRequest>();
        
        #region Commands
        services.AddScoped<IChangePostalCodeCommand, ChangePostalCodeCommand>();
        services.AddScoped<IAddressIsWrongCommand, AddressIsWrongCommand>();
        services.AddScoped<IIncorrectCommand, IncorrectCommand>();
        services.AddScoped<IAddressIsTrueCommand, AddressIsTrueCommand>();
        services.AddScoped<ISetFullStrAddressCommand, SetFullStrAddressCommand>();
        services.AddScoped<ISetBiographyCommand, SetBiographyCommand>();
        services.AddScoped<ISetWishesCommand, SetWishesCommand>();
        services.AddScoped<IChangeProfileCommand, ChangeProfileCommand>();
        services.AddScoped<IMainMenuCommand, MainMenuCommand>();
        services.AddScoped<ISetMailboxCommand, SetMailboxCommand>();
        services.AddScoped<ISwitchAddressCommand, SwitchAddressCommand>();
        services.AddScoped<ISetNameCommand, SetNameCommand>();
        services.AddScoped<ISetToNeedCommand, SetToNeedCommand>();
        services.AddScoped<IStatisticsCommand, StatisticsCommand>();
        #endregion
        
        #region Service
        services.AddScoped<IPostCardService, PostCardService>();
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<ICommandService, CommandService>();
        services.AddScoped<IAddressService, AddressService>();
        #endregion
        
        services.AddSingleton(p =>
        {
            var settings = p.GetService<IOptions<DaDataSettings>>()!.Value;
            return new CleanClientAsync(settings.Token, settings.Secret);
        });
        
        services.AddAutoMapper(typeof(CommandRequestProfile));
    }
}