namespace DailyTechDose.UnitTests.ContentFetchingTests;

[TestFixture]
public class ContentFilterTests
{
    private ContentFilter _filter;

    [SetUp]
    public void SetUp()
    {
        _filter = new ContentFilter();
    }

    [Test]
    public void FilterRecentContent_ContentPublishedAfterTime_ReturnsFilteredContent()
    {
        // Arrange
        var contentList = new List<FetchedContentDTO>
        {
            new("Title1", "Summary1", "Link1", DateTime.UtcNow.AddDays(1)),
            new("Title2", "Summary2", "Link2", DateTime.UtcNow.AddDays(3)),
            new("Title3", "Summary3", "Link3", DateTime.UtcNow.AddDays(-10))
        };
        var filterTime = DateTime.UtcNow;

        // Act
        var result = _filter.FilterRecentContent(contentList, filterTime);

        // Assert
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result.All(c => c.PublishDate > filterTime));
    }

    [Test]
    public void FilterRecentContent_ShouldReturnEmptyList_WhenNoRecentItems()
    {
        var contentList = new List<FetchedContentDTO>
        {
            new("Title1", "Summary1", "Link1", DateTime.UtcNow.AddDays(-1)),
            new("Title1", "Summary1", "Link1", DateTime.UtcNow.AddDays(-3))
        };
        var filterTime = DateTime.UtcNow;

        var result = _filter.FilterRecentContent(contentList, filterTime);
        
        Assert.That(result, Is.Empty);
    }
}
