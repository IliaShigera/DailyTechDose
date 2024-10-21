namespace DailyTechDose.UnitTests.ContentFetchingTests;

[TestFixture]
public class RssContentFetchingStrategyTests
{
    private IHttpClientFactory _httpClientFactory;
    private RssContentFetchingStrategy _rssContentFetchingStrategy;

    [SetUp]
    public void Setup()
    {
        _httpClientFactory = Substitute.For<IHttpClientFactory>();
        _rssContentFetchingStrategy = new RssContentFetchingStrategy(_httpClientFactory);
    }

    [Test]
    public async Task FetchContentAsync_ValidFeedUrl_ReturnsContentList()
    {
        // Arrange
        var source = MockSource.Mock();

        // Item
        const string title = "Example Rss";
        const string link = "https://example.com/article";
        const string desc = "Test description";
        const string pubDate = "Tue, 15 Oct 2024 15:15:00 +0000";

        var xmlContent = $"""
                          
                                      <rss version='2.0'>
                                          <channel>
                                              <title>example Blog</title>
                                              <link>http://example.com</link>
                                              <description>Example RSS feed</description>
                                              <item>
                                                      <title>{title}</title>
                                                      <link>{link}</link>
                                                      <description>{desc}</description>
                                                      <pubDate>{pubDate}</pubDate>
                                              </item>
                                          </channel>
                                      </rss>
                          """;

        var httpMessageHandler = new MockHttpMessageHandler(xmlContent);
        var httpClient = new HttpClient(httpMessageHandler);
        _httpClientFactory.CreateClient().Returns(httpClient);

        // Act
        var result = await _rssContentFetchingStrategy.FetchContentAsync(source);

        // Assert
        Assert.That(result, Has.Count.EqualTo(1));
        Assert.Multiple(() =>
        {
            Assert.That(result[0].Title, Is.EqualTo(title));
            Assert.That(result[0].Link, Is.EqualTo(link));
            Assert.That(result[0].Summary, Is.EqualTo(desc));
            Assert.That(result[0].PublishDate, Is.EqualTo(DateTime.Parse(pubDate).ToUniversalTime()));
        });
    }

    [Test]
    public void FetchContentAsync_HttpRequestFails_ThrowsHttpRequestException()
    {
        // Arrange
        var source = MockSource.Mock();

        var httpClient = new HttpClient(new MockHttpMessageHandler(string.Empty, HttpStatusCode.InternalServerError));
        _httpClientFactory.CreateClient().Returns(httpClient);

        // Act & Assert
        Assert.ThrowsAsync<HttpRequestException>(() => _rssContentFetchingStrategy.FetchContentAsync(source));
    }

    [Test]
    public async Task FetchContentAsync_EmptyFeed_ReturnsEmptyList()
    {
        // Arrange
        var source = MockSource.Mock();

        var xmlContent = """
                         
                                     <rss version='2.0'>
                                         <channel>
                                         </channel>
                                     </rss>
                         """;

        var httpMessageHandler = new MockHttpMessageHandler(xmlContent);
        var httpClient = new HttpClient(httpMessageHandler);
        _httpClientFactory.CreateClient().Returns(httpClient);

        // Act
        var result = await _rssContentFetchingStrategy.FetchContentAsync(source);

        // Assert
        Assert.That(result, Is.Empty);
    }
}