namespace DailyTechDose.Core.Interfaces;

public interface IContentFilter
{
    IReadOnlyList<FetchedContentDTO> FilterRecentContent(IReadOnlyList<FetchedContentDTO> contentList, DateTime time);
}