namespace DailyTechDose.Core.Interfaces;

public interface IBotService
{
    Task SendMessageAsync(long chatId, string message, ParseMode parseMode = ParseMode.Html);

    /// <summary>
    /// Publishing content to specified channel.
    /// </summary>
    /// <param name="contentItem"></param>
    /// <returns></returns>
    Task PublishContentAsync(ContentItem contentItem);
}