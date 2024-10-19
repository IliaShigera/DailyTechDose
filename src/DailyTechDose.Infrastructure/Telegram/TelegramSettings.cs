namespace DailyTechDose.Infrastructure.Telegram;

/// <summary>
/// Configuration settings for the Telegram bot, including credentials and message constraints.
/// </summary>
internal sealed class TelegramSettings
{
    internal TelegramSettings(string botToken, long chatId, int maxSummaryLength)
    {
        BotToken = botToken;
        ChatId = chatId;
        MaxSummaryLength = maxSummaryLength;
    }

    /// <summary>
    /// Bot token for authenticating API requests to Telegram.
    /// </summary>
    internal string BotToken { get; }


    /// <summary>
    /// Chat ID specifying the target chat for message delivery.
    /// </summary>
    internal long ChatId { get; }


    /// <summary>
    /// Maximum allowable length for message summaries.
    /// </summary>
    internal int MaxSummaryLength { get; }
}

