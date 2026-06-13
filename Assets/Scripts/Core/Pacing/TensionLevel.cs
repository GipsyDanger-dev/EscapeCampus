namespace EscapeCampus.Core.Pacing
{
    public enum TensionState
    {
        Calm,       // 0-20
        Suspense,   // 20-40
        Unease,     // 40-60
        Fear,       // 60-80
        Panic       // 80-100
    }

    public enum NarrativeBeat
    {
        Exploration,    // Player explores, low tension
        Discovery,      // Finding documents/evidence
        Horror,         // Horror event active
        SetPiece,       // Scripted moment
        Revelation,     // Truth fragment or story reveal
        Silence         // Quiet moment, breathing space
    }

    public static class TensionLevelExtensions
    {
        public static TensionState GetState(float level)
        {
            if (level < 20f) return TensionState.Calm;
            if (level < 40f) return TensionState.Suspense;
            if (level < 60f) return TensionState.Unease;
            if (level < 80f) return TensionState.Fear;
            return TensionState.Panic;
        }

        public static string GetStateName(TensionState state)
        {
            switch (state)
            {
                case TensionState.Calm: return "Calm";
                case TensionState.Suspense: return "Suspense";
                case TensionState.Unease: return "Unease";
                case TensionState.Fear: return "Fear";
                case TensionState.Panic: return "Panic";
                default: return "Unknown";
            }
        }

        public static string GetBeatName(NarrativeBeat beat)
        {
            switch (beat)
            {
                case NarrativeBeat.Exploration: return "Exploration";
                case NarrativeBeat.Discovery: return "Discovery";
                case NarrativeBeat.Horror: return "Horror";
                case NarrativeBeat.SetPiece: return "SetPiece";
                case NarrativeBeat.Revelation: return "Revelation";
                case NarrativeBeat.Silence: return "Silence";
                default: return "Unknown";
            }
        }
    }
}
