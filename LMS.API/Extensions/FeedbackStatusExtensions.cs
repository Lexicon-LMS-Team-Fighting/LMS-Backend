namespace LMS.API.Extensions
{
    public static class FeedbackStatusExtensions
    {
        private static readonly Dictionary<LMSActivityFeedbackStatus, string> ToDatabase = new()
        {
            { LMSActivityFeedbackStatus.Completed, "Genomförd" },
            { LMSActivityFeedbackStatus.Delayed, "Försenad" },
            { LMSActivityFeedbackStatus.Approved, "Godkänd" }
        };

        private static readonly Dictionary<string, LMSActivityFeedbackStatus> FromDatabase =
            ToDatabase.ToDictionary(kv => kv.Value, kv => kv.Key);

        public static string ToDbString(this LMSActivityFeedbackStatus status) => ToDatabase[status];

        public static LMSActivityFeedbackStatus FromDbString(string dbValue) => FromDatabase[dbValue];
    }
}
