namespace DailyTechDose.Infrastructure.ContentFetching;

internal sealed class ContentProcessingService : IContentProcessingService
{
    private readonly IRepository _repository;
    private readonly IContentFetcher _fetcher;
    private readonly IContentFilter _filter;
    private readonly ILogger<ContentProcessingService> _logger;

    public ContentProcessingService(
        IRepository repository,
        IContentFetcher fetcher,
        IContentFilter filter,
        ILogger<ContentProcessingService> logger)
    {
        _repository = repository;
        _fetcher = fetcher;
        _filter = filter;
        _logger = logger;
    }

    public async Task ProcessAllSourcesAsync()
    {
        var sources = await _repository.Sources.ToListAsync();

        _logger.LogDebug("Fetched {Count} sources.", sources.Count);

        var processTasks = sources.Select(ProcessSourceAsync);
        await Task.WhenAll(processTasks);

        await _repository.SaveChangesAsync();
    }

    private async Task ProcessSourceAsync(Source source)
    {
        try
        {
            _logger.LogDebug("Processing source: {SourceName}, ID: {SourceId}", source.SourceName, source.Id);

            var fetchedContent = await _fetcher.FetchContentItemsAsync(source);

            _logger.LogDebug("Fetched {Count} new content items for source: {SourceName}",
                fetchedContent.Count, source.SourceName);

            source.UpdateLastFetchedDate();

            var recentContent = _filter.FilterRecentContent(fetchedContent, source.LastFetchedDate ?? DateTime.UtcNow);

            foreach (var content in recentContent)
                source.AddContentItem(content.Title, content.Summary, content.Link, content.PublishDate);

            _repository.Sources.Update(source);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing source: {SourceName}, ID: {SourceId}",
                source.SourceName, source.Id);
        }
    }
}