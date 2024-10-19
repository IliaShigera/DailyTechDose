namespace DailyTechDose.Core.Entities;

/// <summary>
/// Represents a content item (e.g., an article or blog post) that belongs to a specific content source.
/// </summary>
public sealed class ContentItem : Entity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ContentItem"/> class.
    /// </summary>
    /// <param name="title">The title of the content item.</param>
    /// <param name="summary">A short summary or description of the content item.</param>
    /// <param name="link">The URL link to the full content item.</param>
    /// <param name="publishDate">The date the content item was published.</param>
    /// <param name="source">The content source this item belongs to.</param>
    internal ContentItem(string title, string summary, string link, DateTime publishDate, Source source)
    {
        Title = title;
        Summary = summary;
        Link = link;
        PublishDate = publishDate;
        Source = source;
        IsPublished = false;
    }

    private ContentItem()
    {
        // EF only
    }

    /// <summary>
    /// Gets the title of the content item.
    /// </summary>
    public string Title { get; init; }
    
    /// <summary>
    /// Gets a short summary or description of the content item.
    /// </summary>
    public string Summary { get; init; }
    
    /// <summary>
    /// Gets the URL link to the full content item.
    /// </summary>
    public string Link { get; init; }
    
    /// <summary>
    /// Gets the date the content item was published.
    /// </summary>
    public DateTime PublishDate { get; init; }
    
    /// <summary>
    /// Gets a value indicating whether the content item has been delivered to the consumer (e.g., telegram channel).
    /// </summary>
    public bool IsPublished { get; private set; }
    
    /// <summary>
    /// Gets the content source this item belongs to.
    /// </summary>
    public Source Source { get; init; }
    public Guid SourceId { get; init; }
    
    public void MarkAsDelivered() => IsPublished = true;
}