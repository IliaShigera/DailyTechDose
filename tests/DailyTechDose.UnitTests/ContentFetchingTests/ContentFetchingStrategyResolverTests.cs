namespace DailyTechDose.UnitTests.ContentFetchingTests;

[TestFixture]
public class ContentFetchingStrategyResolverTests
{
    private IReadOnlyList<IContentFetchingStrategy> _strategies = [];

    [SetUp]
    public void SetUp()
    {
        var httpClientFactory = Substitute.For<IHttpClientFactory>();
        
        var rssStrategy = new RssContentFetchingStrategy(httpClientFactory);
        var customScraper = Substitute.For<IContentFetchingStrategy>();

        _strategies = [rssStrategy, customScraper];
    }

    [Test]
    public void Resolve_ReturnsRssContentFetchingStrategy_WhenSourceSupportsRss()
    {
        // Arrange
        var source = MockSource.Mock(isRssSupported: true);
        var resolver = new ContentFetchingStrategyResolver(_strategies);

        // Act
        var result = resolver.Resolve(source);

        // Assert
        Assert.That(result, Is.InstanceOf<RssContentFetchingStrategy>());
    }

    // [Test]
    // public void Resolve_ReturnsCustomScraperStrategy_WhenSourceDoesNotSupportRss()
    // {
    //     // Arrange
    //     var source = SourceFactory.Create(isRssSupported: false);
    //     var resolver = new ContentFetchingStrategyResolver(_strategies);
    //
    //     // Act
    //     var result = resolver.Resolve(source);
    //
    //     // Assert
    //     Assert.That(result, Is.InstanceOf<CustomScraperStrategy>());
    // }

    [Test]
    public void Resolve_ThrowsNotSupportedException_WhenNoMatchingStrategyIsFound()
    {
        // Arrange
        var source = MockSource.Mock();

        IList<IContentFetchingStrategy> strategies = []; 

        var resolver = new ContentFetchingStrategyResolver(strategies);

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => resolver.Resolve(source));
    }

    [Test]
    public void Resolve_ThrowsNotSupportedException_WhenStrategyIsNull()
    {
        // Arrange
        var strategy = Substitute.For<IContentFetchingStrategy>();
        var strategies = new List<IContentFetchingStrategy> { strategy };

        var source = MockSource.Mock(isRssSupported: true); // But no RSS strategy in the list

        var resolver = new ContentFetchingStrategyResolver(strategies);

        // Act & Assert
        Assert.Throws<NotSupportedException>(() => resolver.Resolve(source));
    }
}