using System;

namespace EscapeCampus.Core.Ending
{
    [Serializable]
    public class EndingConditions
    {
        // Document requirements
        public int minDocumentsCollected;
        public int minCriticalDocuments;

        // Evidence requirements
        public int minEvidenceCollected;
        public int minCriticalEvidence;

        // Puzzle requirements
        public int minPuzzlesSolved;

        // Horror requirements
        public float minHorrorLevelPeak;
        public float maxHorrorLevelPeak; // For secret ending

        // SetPiece requirements
        public int minSetPiecesCompleted;

        // Observation requirements
        public int minObservations;

        // Story phase requirement
        public StoryPhase requiredPhase;

        public bool Evaluate(EndingEvaluationData data)
        {
            if (data.totalDocuments < minDocumentsCollected) return false;
            if (data.criticalDocuments < minCriticalDocuments) return false;
            if (data.totalEvidence < minEvidenceCollected) return false;
            if (data.criticalEvidence < minCriticalEvidence) return false;
            if (data.puzzlesSolved < minPuzzlesSolved) return false;
            if (data.horrorLevelPeak < minHorrorLevelPeak) return false;
            if (maxHorrorLevelPeak > 0 && data.horrorLevelPeak > maxHorrorLevelPeak) return false;
            if (data.setPiecesCompleted < minSetPiecesCompleted) return false;
            if (data.observations < minObservations) return false;
            if (data.currentPhase < requiredPhase) return false;

            return true;
        }
    }

    [Serializable]
    public class EndingEvaluationData
    {
        public int totalDocuments;
        public int criticalDocuments;
        public int totalEvidence;
        public int criticalEvidence;
        public int puzzlesSolved;
        public float horrorLevelPeak;
        public int setPiecesCompleted;
        public int observations;
        public StoryPhase currentPhase;
    }
}
