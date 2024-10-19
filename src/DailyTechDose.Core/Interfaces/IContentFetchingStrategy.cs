namespace DailyTechDose.Core.Interfaces;

public interface IContentFetchingStrategy
{
    Task<IReadOnlyList<FetchedContentDTO>> FetchContentAsync(Source source);
}