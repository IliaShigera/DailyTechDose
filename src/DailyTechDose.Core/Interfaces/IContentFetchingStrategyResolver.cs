namespace DailyTechDose.Core.Interfaces;

public interface IContentFetchingStrategyResolver
{
    IContentFetchingStrategy Resolve(Source source);
}