namespace DailyTechDose.UnitTests;

internal static class MockSource
{
    internal static Source Mock(
        string sourceName = "TestSource",
        string sourceUrl = "https://Test",
        string feedUrl = "https://Test/feed/",
        bool isRssSupported = true)
    {
        return new Source(sourceName, sourceUrl, feedUrl, isRssSupported);
    }
}