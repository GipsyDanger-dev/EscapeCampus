namespace EscapeCampus.Horror.Semester14
{
    public enum ObservationType
    {
        Static,         // Entity visible in distance, not moving
        Peripheral,     // Only visible at camera edge
        Mirror,         // Only in reflections
        MissingFrame    // Appears only on certain frames (blink effect)
    }

    public static class ObservationTypeExtensions
    {
        public static string GetTypeName(ObservationType type)
        {
            switch (type)
            {
                case ObservationType.Static: return "Static";
                case ObservationType.Peripheral: return "Peripheral";
                case ObservationType.Mirror: return "Mirror";
                case ObservationType.MissingFrame: return "Missing Frame";
                default: return "Unknown";
            }
        }

        public static string GetDescription(ObservationType type)
        {
            switch (type)
            {
                case ObservationType.Static:
                    return "A figure stands in the distance. Motionless. Watching.";
                case ObservationType.Peripheral:
                    return "Something at the edge of your vision. Turn to look... gone.";
                case ObservationType.Mirror:
                    return "In the reflection. Behind you. But when you turn...";
                case ObservationType.MissingFrame:
                    return "Did you see that? No. You didn't. But you did.";
                default:
                    return "";
            }
        }
    }
}
