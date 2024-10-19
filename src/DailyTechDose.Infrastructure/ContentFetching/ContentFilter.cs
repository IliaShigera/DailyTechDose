namespace DailyTechDose.Infrastructure.ContentFetching;

internal sealed class ContentFilter : IContentFilter
{
    public IReadOnlyList<FetchedContentDTO> FilterRecentContent(IReadOnlyList<FetchedContentDTO> contentList, DateTime time)
    {
        return contentList.Where(content => content.PublishDate > time).ToList();
    }
}