namespace DailyTechDose.Core.Interfaces;

public interface IContentPublishingService
{
    Task PublishPendingContentAsync();
}