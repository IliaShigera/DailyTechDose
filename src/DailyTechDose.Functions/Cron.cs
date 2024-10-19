namespace DailyTechDose.Functions;

internal static class Cron
{
    internal const string DailyAt9AM = "0 9 * * *";

    internal const string DailyAt10AM = "0 10 * * *";

    internal const string EveryMinute = "* * * * *";

    internal const string EveryFiveMinutes = "*/5 * * * *";
}