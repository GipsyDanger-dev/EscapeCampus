using UnityEngine;

namespace EscapeCampus.Puzzle
{
    public class PuzzleDebugTool : MonoBehaviour
    {
        [SerializeField] private KeyCode unlockAllKey = KeyCode.F2;
        [SerializeField] private KeyCode solveCurrentKey = KeyCode.F3;

        private void Update()
        {
            if (Input.GetKeyDown(unlockAllKey))
            {
                UnlockAllPuzzles();
            }

            if (Input.GetKeyDown(solveCurrentKey))
            {
                SolveCurrentPuzzle();
            }
        }

        private void UnlockAllPuzzles()
        {
            if (PuzzleManager.Instance != null)
            {
                PuzzleManager.Instance.UnlockAllPuzzles();
                Debug.Log("[PuzzleDebug] All puzzles unlocked!");
                LogAllPuzzleStates();
            }
        }

        private void SolveCurrentPuzzle()
        {
            if (PuzzleManager.Instance != null)
            {
                PuzzleManager.Instance.SolveCurrentPuzzle();
                LogAllPuzzleStates();
            }
        }

        private void LogAllPuzzleStates()
        {
            if (PuzzleManager.Instance == null) return;

            var states = PuzzleManager.Instance.GetAllPuzzleStates();
            Debug.Log("=== PUZZLE STATES ===");

            foreach (var kvp in states)
            {
                Debug.Log($"  {kvp.Key}: {kvp.Value}");
            }

            Debug.Log($"Completed: {PuzzleManager.Instance.GetCompletedCount()}/{PuzzleManager.Instance.GetTotalCount()}");
            Debug.Log("====================");
        }
    }
}
