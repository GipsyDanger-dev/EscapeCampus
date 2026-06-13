namespace EscapeCampus.Core.Ending
{
    public enum EndingType
    {
        None,
        GoodEnding,     // Player destroys ritual, Raka loop ends
        BadEnding,      // Player escapes but loop continues
        SecretEnding,   // Player partially joins system
        TrueEnding      // Full truth revealed, Semester 14 identity explained
    }

    public enum EndingPhase
    {
        NotStarted,
        FinalSetPiece,
        WorldBreakdown,
        ConstantPresence,
        GraduationHall,
        FinalDecision,
        EndingSequence,
        Credits
    }

    public enum FinalDecision
    {
        None,
        DestroyRitual,      // Good ending path
        ContinueLoop,       // Bad ending path
        EscapeWithoutTruth  // Secret ending path
    }

    public static class EndingTypeExtensions
    {
        public static string GetEndingName(EndingType type)
        {
            switch (type)
            {
                case EndingType.GoodEnding: return "The End of the Loop";
                case EndingType.BadEnding: return "The Cycle Continues";
                case EndingType.SecretEnding: return "The Third Path";
                case EndingType.TrueEnding: return "The Truth Revealed";
                default: return "Unknown";
            }
        }

        public static string GetEndingDescription(EndingType type)
        {
            switch (type)
            {
                case EndingType.GoodEnding:
                    return "You destroyed the ritual. The loop ends. Raka is free. But at what cost?";
                case EndingType.BadEnding:
                    return "You escaped. But the campus remains. The loop continues. Someone else will take your place.";
                case EndingType.SecretEnding:
                    return "You chose the third path. Neither destroy nor escape. You became part of the system. Is this freedom?";
                case EndingType.TrueEnding:
                    return "The full truth revealed. Semester 14 was never the enemy. It was trying to help you see. And now you understand.";
                default: return "";
            }
        }

        public static string GetDecisionDescription(FinalDecision decision)
        {
            switch (decision)
            {
                case FinalDecision.DestroyRitual:
                    return "Destroy the ritual circle. End the loop forever. But the campus will collapse.";
                case FinalDecision.ContinueLoop:
                    return "Escape through the gate. The loop continues. But you survive.";
                case FinalDecision.EscapeWithoutTruth:
                    return "Walk away. Don't look back. Some truths are better left buried.";
                default: return "";
            }
        }
    }
}
