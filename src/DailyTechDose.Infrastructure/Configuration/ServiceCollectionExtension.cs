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
        
        return services;
    }
}