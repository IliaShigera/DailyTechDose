namespace DailyTechDose.UnitTests.ContentFetchingTests;

[TestFixture]
public class ContentProcessingServiceTests
{
    private IContentProcessingService _service;
    private IRepository _repository;
    private IContentFetcher _fetcher;
    private IContentFilter _filter;
    private ILogger<ContentProcessingService> _logger;

    [SetUp]
    public void SetUp()
    {
        _repository = Substitute.For<IRepository>();
        _fetcher = Substitute.For<IContentFetcher>();
        _filter = Substitute.For<IContentFilter>();
        _logger = Substitute.For<ILogger<ContentProcessingService>>();
        _service = new ContentProcessingService(_repository, _fetcher, _filter, _logger);
    }

    [Test]
    public async Task ProcessAllSourcesAsync_ShouldFetchAndFilterContentForEachSource()
    {
        // Arrange
        var sources = new List<Source> { MockSource.Mock(sourceName: "source1"), };
        _repository.ListAllAsync().Returns(sources);

        var fetchedContent = new List<FetchedContentDTO>
        {
            new("Title1", "Summary1", "Link1", DateTime.UtcNow.AddDays(-2)),
            new("Title2", "Summary2", "Link2", DateTime.UtcNow.AddDays(-3))
        };
        _fetcher.FetchContentItemsAsync(Arg.Any<Source>()).Returns(fetchedContent);

        var recentContent = fetchedContent.Take(1).ToList();
        _filter.FilterRecentContent(fetchedContent, Arg.Any<DateTime>()).Returns(recentContent);

        // Act
        await _service.ProcessAllSourcesAsync();

        // Assert
        await _fetcher.Received(1).FetchContentItemsAsync(Arg.Is<Source>(s => s.SourceName == "source1"));
        _filter.Received(1).FilterRecentContent(fetchedContent, Arg.Any<DateTime>());

        Assert.That(sources[0].ContentItems, Has.Count.EqualTo(1));
        await _repository.Received(1).SaveChangesAsync();
    }
}