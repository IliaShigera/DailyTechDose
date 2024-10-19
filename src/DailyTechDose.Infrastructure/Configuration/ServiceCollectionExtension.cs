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

        return services;
    }
}