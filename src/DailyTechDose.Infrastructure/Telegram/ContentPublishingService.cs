namespace DailyTechDose.Infrastructure.Telegram;

// ─────────────────────────────────────────────────────────────────────
// NOTE:
// 
//  CURRENT STATE:
//    - This service is currently designed to publish content exclusively to Telegram using IBotService.
//    - It's tailored to my personal needs, where I only interact with Telegram for content publishing.
//
//  IMPORTANT:
//    - For now, this service relies on Telegram as the sole publishing platform, but the code can be easily 
//      adapted to support more platforms by following a strategy or factory pattern.
// 
// ─────────────────────────────────────────────────────────────────────

// Example for future logic:
// 1. Retrieve pending content from the repository
// 2. Allow users to choose their preferred platform for publishing
// 3. Dynamically resolve the appropriate IPublisher (Telegram, Discord, etc.) and publish content.

internal sealed class ContentPublishingService : IContentPublishingService
{
    private readonly IRepository _repository;
    private readonly IBotService _botService;
    private readonly ILogger<ContentPublishingService> _logger;

    public ContentPublishingService(IRepository repository, IBotService botService, ILogger<ContentPublishingService> logger)
    {
        _repository = repository;
        _botService = botService;
        _logger = logger;
    }

    public async Task PublishPendingContentAsync()
    {
        var sources = await _repository.ListAllAsync(includePendingContent: true);

        var unpublishedContentList = sources.SelectMany(src => src.ContentItems).ToList();
        _logger.LogDebug("Fetched {Count} unpublished content items.", unpublishedContentList.Count);

        if (unpublishedContentList.Count == 0)
        {
            _logger.LogInformation("No unpublished content.");
            return;
        }

        var publishTasks = unpublishedContentList.Select(PublishAndMarkAsync);
        await Task.WhenAll(publishTasks);

        _repository.UpdateRange(sources);
        await _repository.SaveChangesAsync();
    }

    private async Task PublishAndMarkAsync(ContentItem contentItem)
    {
        try
        {
            _logger.LogDebug("Publishing content item: {Title}, ID: {ContentId}", contentItem.Title, contentItem.Id);

            await _botService.PublishContentAsync(contentItem);

            contentItem.MarkAsDelivered();
            _logger.LogDebug("Successfully published content item: {Title}, ID: {ContentId}", contentItem.Title, contentItem.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing content item: {Title}, ID: {ContentId}", contentItem.Title, contentItem.Id);
        }
    }
}