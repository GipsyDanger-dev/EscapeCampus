namespace EscapeCampus.Horror.SetPieces
{
    public enum SetPieceState
    {
        Idle,
        Triggering,
        Active,
        Ending,
        Completed
    }

    public enum SetPieceType
    {
        ForcedCamera,
        EnvironmentalCollapse,
        CorridorEvent,
        ObservationFreeze,
        EscapeSequence
    }

    public static class SetPieceStateExtensions
    {
        public static string GetStateName(SetPieceState state)
        {
            switch (state)
            {
                case SetPieceState.Idle: return "Idle";
                case SetPieceState.Triggering: return "Triggering";
                case SetPieceState.Active: return "Active";
                case SetPieceState.Ending: return "Ending";
                case SetPieceState.Completed: return "Completed";
                default: return "Unknown";
            }
        }
    }
}
