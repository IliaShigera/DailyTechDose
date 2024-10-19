namespace DailyTechDose.Functions.Triggers;

public sealed class FetchAndProcessNewContent
{
    private readonly IContentProcessingService _contentProcessingService;
    private readonly ILogger<FetchAndProcessNewContent> _logger;

    public FetchAndProcessNewContent(IContentProcessingService contentProcessingService, ILogger<FetchAndProcessNewContent> logger)
    {
        _contentProcessingService = contentProcessingService;
        _logger = logger;
    }

    [Function(nameof(FetchAndProcessNewContent))]
    public async Task Run([TimerTrigger(Cron.DailyAt9AM)] TimerInfo timer)
    {
        _logger.LogInformation("Starting content fetch and processing.");

        await _contentProcessingService.ProcessAllSourcesAsync();

        _logger.LogInformation("Finished content fetch and processing.");
    }
}