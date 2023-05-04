var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterServices();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureSettings(builder.Configuration);

builder.Services.AddTransient<IApi, PostCardApi>();

var connectionString = builder.Configuration.GetConnectionString(Consts.DbConnectionString);
builder.Services.AddStore(connectionString);
builder.Services.AddSingleton(s =>
{
    var telegramSettings = s.GetRequiredService<IOptions<TelegramSettings>>().Value;

    var botClient = new TelegramBotClient(telegramSettings.Token);
    
    botClient.SetWebhookAsync(telegramSettings.Url).Wait();

    return botClient;
});

builder.Services.RegisterServices(); 

var app = builder.Build();

app.Services.GetService<TelegramBotClient>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.RegisterApis();
app.MigrationDb();
app.Run();