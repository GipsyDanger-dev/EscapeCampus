namespace EscapeCampus.Horror
{
    public enum HorrorStage
    {
        Calm,       // 0-20
        Unease,     // 20-40
        Disturbance, // 40-60
        Paranoia,   // 60-80
        Collapse    // 80-100
    }

    public static class HorrorLevelExtensions
    {
        public static HorrorStage GetStage(float level)
        {
            if (level < 20f) return HorrorStage.Calm;
            if (level < 40f) return HorrorStage.Unease;
            if (level < 60f) return HorrorStage.Disturbance;
            if (level < 80f) return HorrorStage.Paranoia;
            return HorrorStage.Collapse;
        }

        public static string GetStageName(HorrorStage stage)
        {
            switch (stage)
            {
                case HorrorStage.Calm: return "Calm";
                case HorrorStage.Unease: return "Unease";
                case HorrorStage.Disturbance: return "Disturbance";
                case HorrorStage.Paranoia: return "Paranoia";
                case HorrorStage.Collapse: return "Collapse";
                default: return "Unknown";
            }
        }
    }
}
