namespace DailyTechDose.Infrastructure.ContentFetching;

internal sealed class RssContentFetchingStrategy : IContentFetchingStrategy
{
    private readonly IHttpClientFactory _clientFactory;

    public RssContentFetchingStrategy(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }

    public async Task<IReadOnlyList<FetchedContentDTO>> FetchContentAsync(Source source)
    {
        using var httpClient = _clientFactory.CreateClient();
        var response = await httpClient.GetAsync(source.FeedUrl);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var xmlReader = XmlReader.Create(stream);
        var feed = SyndicationFeed.Load(xmlReader);

        var contentList = feed.Items
            .Select(item => new FetchedContentDTO(
                Title: item.Title.Text,
                Summary: item.Summary.Text,
                Link: item.Links.First().Uri.ToString(),
                PublishDate: item.PublishDate.UtcDateTime)
            ).ToList();

        return contentList;
    }
}