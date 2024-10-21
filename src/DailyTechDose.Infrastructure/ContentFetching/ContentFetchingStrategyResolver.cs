namespace DailyTechDose.Infrastructure.ContentFetching;

internal sealed class ContentFetchingStrategyResolver : IContentFetchingStrategyResolver
{
    private readonly IReadOnlyList<IContentFetchingStrategy> _strategies;

    public ContentFetchingStrategyResolver(IEnumerable<IContentFetchingStrategy> strategies)
    {
        _strategies = strategies.ToList().AsReadOnly();
    }

    public IContentFetchingStrategy Resolve(Source source)
    {
        IContentFetchingStrategy? strategy = default;
        
        if (source.IsRssSupported)
            strategy = _strategies.OfType<RssContentFetchingStrategy>().FirstOrDefault();
        
        // else
        //  return _strategies.OfType<CustomScraperStrategy>().FirstOrDefault();
        
        return strategy ?? throw new NotSupportedException($"Content fetching strategy not supported: {source}");
    }
}