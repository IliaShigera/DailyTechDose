namespace DailyTechDose.Infrastructure.Configuration;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        #region Database

        var connection = Environment.GetEnvironmentVariable("Database:ConnectionString");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connection));

        services.AddTransient<IRepository, AppDbContext>();

        #endregion

        #region ContentFetching

        services.AddHttpClient();

        services.AddSingleton<IContentFetchingStrategy, RssContentFetchingStrategy>();
        services.AddSingleton<IContentFetchingStrategyResolver, ContentFetchingStrategyResolver>();
        services.AddSingleton<IContentFilter, ContentFilter>();
        services.AddSingleton<IContentFetcher, ContentFetcher>();
        services.AddSingleton<IContentProcessingService, ContentProcessingService>();

        #endregion

        #region Telegram

        const int maxContentSummaryLength = 200;

        services.AddSingleton<TelegramBotClient>(_ =>
        {
            var botToken = Environment.GetEnvironmentVariable("TelegramSettings:BotToken")!;

            return new TelegramBotClient(botToken);
        });

        services.AddSingleton<TelegramSettings>(_ =>
            new TelegramSettings(
                botToken: Environment.GetEnvironmentVariable("TelegramSettings:BotToken")!,
                chatId: long.Parse(Environment.GetEnvironmentVariable("TelegramSettings:ChannelId")!),
                maxSummaryLength: maxContentSummaryLength));

        services.AddSingleton<IBotService, BotService>();
        services.AddSingleton<IContentPublishingService, ContentPublishingService>();

        #endregion

        return services;
    }
}