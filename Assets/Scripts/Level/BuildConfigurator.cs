using UnityEngine;

namespace EscapeCampus.Level
{
    public class BuildConfigurator : MonoBehaviour
    {
        [Header("Build Settings")]
        [SerializeField] private bool isReleaseBuild = true;

        private void Awake()
        {
            if (isReleaseBuild)
            {
                DisableDebugTools();
            }
        }

        private void DisableDebugTools()
        {
            // Disable PuzzleDebugTool
            var puzzleDebug = FindObjectOfType<EscapeCampus.Puzzle.PuzzleDebugTool>();
            if (puzzleDebug != null)
            {
                puzzleDebug.enabled = false;
            }

            // Disable HorrorDebugTool
            var horrorDebug = FindObjectOfType<EscapeCampus.Horror.HorrorDebugTool>();
            if (horrorDebug != null)
            {
                horrorDebug.enabled = false;
            }

            // Disable Semester14DebugTool
            var s14Debug = FindObjectOfType<EscapeCampus.Horror.Semester14.Semester14DebugTool>();
            if (s14Debug != null)
            {
                s14Debug.enabled = false;
            }

            // Disable LevelFlowDebugTool
            var levelDebug = FindObjectOfType<EscapeCampus.Core.LevelFlowDebugTool>();
            if (levelDebug != null)
            {
                levelDebug.enabled = false;
            }

            // Disable PacingDebugTool
            var pacingDebug = FindObjectOfType<EscapeCampus.Core.Pacing.PacingDebugTool>();
            if (pacingDebug != null)
            {
                pacingDebug.enabled = false;
            }

            // Disable SaveDebugTool
            var saveDebug = FindObjectOfType<EscapeCampus.UI.SaveDebugTool>();
            if (saveDebug != null)
            {
                saveDebug.enabled = false;
            }

            Debug.Log("[BuildConfigurator] All debug tools disabled for release build.");
        }
    }
}
