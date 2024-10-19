namespace DailyTechDose.Functions.Triggers;

public sealed class PublishContentToTelegram
{
    private readonly IContentPublishingService _publishingService;
    private readonly ILogger<PublishContentToTelegram> _logger;

    public PublishContentToTelegram(IContentPublishingService publishingService, ILogger<PublishContentToTelegram> logger)
    {
        _publishingService = publishingService;
        _logger = logger;
    }

    [Function(nameof(PublishContentToTelegram))]
    public async Task Run([TimerTrigger(Cron.DailyAt10AM)] TimerInfo timer)
    {
        _logger.LogInformation("Start publishing content to Telegram");

        await _publishingService.PublishPendingContentAsync();

        _logger.LogInformation("Finished publishing content to Telegram");
    }
}