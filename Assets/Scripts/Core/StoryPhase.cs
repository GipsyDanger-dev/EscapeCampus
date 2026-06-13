namespace EscapeCampus.Core
{
    public enum StoryPhase
    {
        Introduction,       // Player arrives, orientation
        EarlyInvestigation, // First documents, exploring
        FirstAnomaly,       // Something feels wrong
        DeepInvestigation,  // Serious research, puzzles
        RealityBreakdown,   // Horror escalates, reality扭曲
        FinalPreparation,   // Before the climax
        FinalChase,         // The escape sequence
        Ending              // Resolution
    }

    public static class StoryPhaseExtensions
    {
        public static string GetPhaseName(StoryPhase phase)
        {
            switch (phase)
            {
                case StoryPhase.Introduction: return "Introduction";
                case StoryPhase.EarlyInvestigation: return "Early Investigation";
                case StoryPhase.FirstAnomaly: return "First Anomaly";
                case StoryPhase.DeepInvestigation: return "Deep Investigation";
                case StoryPhase.RealityBreakdown: return "Reality Breakdown";
                case StoryPhase.FinalPreparation: return "Final Preparation";
                case StoryPhase.FinalChase: return "Final Chase";
                case StoryPhase.Ending: return "Ending";
                default: return "Unknown";
            }
        }

        public static string GetPhaseDescription(StoryPhase phase)
        {
            switch (phase)
            {
                case StoryPhase.Introduction:
                    return "You find yourself on campus. Something feels... off.";
                case StoryPhase.EarlyInvestigation:
                    return "There are documents to find. Secrets to uncover.";
                case StoryPhase.FirstAnomaly:
                    return "Wait... did that just happen? No, it couldn't have.";
                case StoryPhase.DeepInvestigation:
                    return "The pieces are coming together. The truth is darker than you thought.";
                case StoryPhase.RealityBreakdown:
                    return "Nothing is what it seems anymore. Reality itself is fracturing.";
                case StoryPhase.FinalPreparation:
                    return "You know what you must do. There's no turning back.";
                case StoryPhase.FinalChase:
                    return "RUN.";
                case StoryPhase.Ending:
                    return "It's over... or is it?";
                default:
                    return "";
            }
        }

        public static int GetPhaseIndex(StoryPhase phase)
        {
            return (int)phase;
        }

        public static StoryPhase GetPhaseFromIndex(int index)
        {
            if (index < 0) return 0;
            if (index > (int)StoryPhase.Ending) return StoryPhase.Ending;
            return (StoryPhase)index;
        }
    }
}
