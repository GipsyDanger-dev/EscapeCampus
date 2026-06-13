using UnityEngine;
using EscapeCampus.Interaction;

namespace EscapeCampus.Puzzle
{
    public class PuzzleTrigger : MonoBehaviour, IInteractable
    {
        [SerializeField] private PuzzleBase targetPuzzle;
        [SerializeField] private string lockedPrompt = "Puzzle Locked";
        [SerializeField] private string unlockedPrompt = "Start Puzzle";
        [SerializeField] private string inProgressPrompt = "Continue Puzzle";
        [SerializeField] private string solvedPrompt = "Puzzle Solved";

        public string InteractionPrompt
        {
            get
            {
                if (targetPuzzle == null) return "No Puzzle";

                switch (targetPuzzle.CurrentState)
                {
                    case PuzzleState.Locked:
                        return lockedPrompt;
                    case PuzzleState.Unlocked:
                        return unlockedPrompt;
                    case PuzzleState.InProgress:
                        return inProgressPrompt;
                    case PuzzleState.Solved:
                        return solvedPrompt;
                    default:
                        return "Unknown";
                }
            }
        }

        public PuzzleBase TargetPuzzle => targetPuzzle;

        public void Interact()
        {
            if (targetPuzzle == null)
            {
                Debug.LogWarning($"[PuzzleTrigger] No target puzzle assigned to {gameObject.name}");
                return;
            }

            PuzzleState state = targetPuzzle.CurrentState;

            if (state == PuzzleState.Locked)
            {
                Debug.Log($"[PuzzleTrigger] {targetPuzzle.PuzzleID} is locked.");
                OnPuzzleLocked();
                return;
            }

            if (state == PuzzleState.Solved)
            {
                Debug.Log($"[PuzzleTrigger] {targetPuzzle.PuzzleID} is already solved.");
                return;
            }

            targetPuzzle.StartPuzzle();
        }

        public void SetTargetPuzzle(PuzzleBase puzzle)
        {
            targetPuzzle = puzzle;
        }

        protected virtual void OnPuzzleLocked()
        {
            // Override for custom locked behavior
        }
    }
}
