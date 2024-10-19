namespace DailyTechDose.Infrastructure.Telegram;

// TextGeneration.cs
internal sealed partial class BotService
{
    /// <summary>
    /// A regex pattern to remove all HTML tags except those supported by Telegram
    /// </summary>
    private static readonly Regex HtmlTagRegex = new("<.*?>", RegexOptions.Compiled);

    private string GenerateHtmlMessage(ContentItem contentItem)
    {
        var summary = RemoveUnsupportedTags(contentItem.Summary);
        summary = TruncateText(summary, _settings.MaxSummaryLength);

        return $@"
<b>{contentItem.Title}</b>

<a href=""{contentItem.Source.SourceUrl}"">{contentItem.Source.SourceName}</a> | {contentItem.PublishDate:MMMM d, yyyy}

{summary}

{contentItem.Link}".Trim();
    }

    private static string RemoveUnsupportedTags(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        var sanitized = HtmlTagRegex.Replace(input, string.Empty);

        return sanitized.Trim();
    }

    private static string TruncateText(string input, int maxLength)
    {
        if (input.Length <= maxLength)
            return input;

        var truncated = input[..maxLength];

        var lastSpace = truncated.LastIndexOf(' ');
        if (lastSpace > 0)
            return truncated[..lastSpace] + "...";

        return truncated + "...";
    }
}