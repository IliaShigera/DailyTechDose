namespace DailyTechDose.Core.Entities;

/// <summary>
/// Represents the source of the content, such as a blog or website, which provides content items (articles, posts, etc.).
/// </summary>
public sealed class Source : Entity
{
    private List<ContentItem> _contentItems = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="Source"/> class.
    /// </summary>
    /// <param name="sourceName">The human-readable name of the content source (e.g., "Microsoft Blog").</param>
    /// <param name="sourceUrl">The main URL of the content source.</param>
    /// <param name="feedUrl">The URL of the content feed (RSS or other).</param>
    /// <param name="isRssSupported">Indicates whether the content source supports RSS.</param>
    public Source(string sourceName, string sourceUrl, string feedUrl, bool isRssSupported)
    {
        SourceName = sourceName;
        SourceUrl = sourceUrl;
        FeedUrl = feedUrl;
        IsRssSupported = isRssSupported;
        _contentItems = new List<ContentItem>();
    }

    private Source()
    {
        // EF only
    }

    /// <summary>
    /// The human-readable name of the content source.
    /// </summary>
    public string SourceName { get; init; }

    /// <summary>
    /// The main URL of the content source.
    /// </summary>
    public string SourceUrl { get; init; }

    /// <summary>
    /// The URL of the content feed (RSS or other).
    /// </summary>
    public string FeedUrl { get; init; }

    /// <summary>
    /// Indicates whether the content source supports RSS.
    /// </summary>
    public bool IsRssSupported { get; init; }
    
    /// <summary>
    /// This tracks the last time content was successfully fetched.
    /// </summary>
    public DateTime? LastFetchedDate { get; private set; }
    
    public void UpdateLastFetchedDate() => LastFetchedDate = DateTime.UtcNow;

    public IReadOnlyList<ContentItem> ContentItems => _contentItems.AsReadOnly();

    /// <summary>
    /// Adds a new content item to this source.
    /// </summary>
    /// <param name="title">The title of the content item.</param>
    /// <param name="summary">A short summary or description of the content item.</param>
    /// <param name="link">The URL link to the full content item.</param>
    /// <param name="publishDate">The date the content item was published.</param>
    public void AddContentItem(string title, string summary, string link, DateTime publishDate)
    {
        var item = new ContentItem(title, summary, link, publishDate, source: this);

        _contentItems.Add(item);
    }
}