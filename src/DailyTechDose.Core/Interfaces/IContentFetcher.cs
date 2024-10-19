namespace DailyTechDose.Core.Interfaces;

public interface IContentFetcher
{   
    Task<IReadOnlyList<FetchedContentDTO>> FetchContentItemsAsync(Source source);
}