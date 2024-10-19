namespace DailyTechDose.Infrastructure.Telegram;

internal sealed partial class BotService : IBotService
{
    private readonly TelegramBotClient _botClient;
    private readonly TelegramSettings _settings;

    public BotService(TelegramBotClient botClient, TelegramSettings settings)
    {
        _botClient = botClient;
        _settings = settings;
    }

    public async Task SendMessageAsync(long chatId, string message, ParseMode parseMode = ParseMode.Html)
    {
        await _botClient.SendTextMessageAsync(chatId, message, parseMode: parseMode, protectContent: true);
    }

    public async Task PublishContentAsync(ContentItem contentItem)
    {
        var message = GenerateHtmlMessage(contentItem);

        await SendMessageAsync(_settings.ChatId, message, ParseMode.Html);
    }
}