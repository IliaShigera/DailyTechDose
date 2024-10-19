namespace DailyTechDose.Infrastructure.ContentFetching;

internal sealed class ContentFetcher : IContentFetcher
{
    private readonly IContentFetchingStrategyResolver _strategyResolver;
    private readonly ILogger<ContentFetcher> _logger;

    public ContentFetcher(IContentFetchingStrategyResolver strategyResolver, ILogger<ContentFetcher> logger)
    {
        _strategyResolver = strategyResolver;
        _logger = logger;
    }

    public async Task<IReadOnlyList<FetchedContentDTO>> FetchContentItemsAsync(Source source)
    {
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning($"Retry {retryCount} for source {source.SourceName} due to {exception.Message}");
                });

        return await retryPolicy.ExecuteAsync(async () =>
        {
            var strategy = _strategyResolver.Resolve(source)
                           ?? throw new NotSupportedException($"Fetching for the source {source.SourceName} is not supported.");

            var contentList = await strategy.FetchContentAsync(source);
            return contentList;
        });
    }
}