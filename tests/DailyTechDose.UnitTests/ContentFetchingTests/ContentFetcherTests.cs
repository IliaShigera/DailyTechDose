namespace DailyTechDose.UnitTests.ContentFetchingTests;

[TestFixture]
public class ContentFetcherTests
{
    private IContentFetchingStrategyResolver _strategyResolver;
    private IContentFetcher _contentFetcher;
    private ILogger<ContentFetcher> _logger;

    [SetUp]
    public void SetUp()
    {
        _strategyResolver = Substitute.For<IContentFetchingStrategyResolver>();
        _logger = Substitute.For<ILogger<ContentFetcher>>();
        _contentFetcher = new ContentFetcher(_strategyResolver, _logger);
    }
    
    [Test]
    public void FetchContentItemsAsync_ShouldThrowNotSupportedException_WhenStrategyNotResolved()
    {
        // Arrange
        var source = MockSource.Mock(sourceName: "UnsupportedSource");
        _strategyResolver.Resolve(source).Returns(default(IContentFetchingStrategy));

        // Act & Assert
        Assert.ThrowsAsync<NotSupportedException>(async () =>
            await _contentFetcher.FetchContentItemsAsync(source));
    }
    
    [Test]
    public async Task FetchContentItemsAsync_ShouldCallStrategyFetchContentAsync()
    {
        // Arrange
        var source = MockSource.Mock();
        var strategy = Substitute.For<IContentFetchingStrategy>();
        _strategyResolver.Resolve(source).Returns(strategy);

        var expectedContent = new List<FetchedContentDTO>
        {
            new("Title1", "Summary1", "Link1", DateTime.UtcNow),
            new("Title2", "Sammary2", "Link2", DateTime.UtcNow),
        };

        strategy.FetchContentAsync(source).Returns(expectedContent);

        // Act
        var result = await _contentFetcher.FetchContentItemsAsync(source);

        // Assert
        Assert.That(result, Is.EqualTo(expectedContent));
        await strategy.Received(1).FetchContentAsync(source);
    }
}