using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace EscapeCampus.Puzzle
{
    public class PuzzleManager : MonoBehaviour
    {
        public static PuzzleManager Instance { get; private set; }

        private Dictionary<string, PuzzleState> puzzleStates = new Dictionary<string, PuzzleState>();
        private Dictionary<string, PuzzleBase> registeredPuzzles = new Dictionary<string, PuzzleBase>();

        public event System.Action<string, PuzzleState> OnPuzzleStateChanged;
        public event System.Action<string> OnPuzzleCompleted;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void RegisterPuzzle(PuzzleBase puzzle)
        {
            if (puzzle == null) return;

            string id = puzzle.PuzzleID;

            if (!registeredPuzzles.ContainsKey(id))
            {
                registeredPuzzles[id] = puzzle;

                if (!puzzleStates.ContainsKey(id))
                {
                    puzzleStates[id] = puzzle.InitialState;
                }

                Debug.Log($"[PuzzleManager] Registered puzzle: {id} (State: {puzzleStates[id]})");
            }
        }

        public void UnregisterPuzzle(PuzzleBase puzzle)
        {
            if (puzzle == null) return;

            registeredPuzzles.Remove(puzzle.PuzzleID);
        }

        public PuzzleState GetPuzzleState(string puzzleID)
        {
            if (puzzleStates.ContainsKey(puzzleID))
            {
                return puzzleStates[puzzleID];
            }

            Debug.LogWarning($"[PuzzleManager] Puzzle not found: {puzzleID}");
            return PuzzleState.Locked;
        }

        public void SetPuzzleState(string puzzleID, PuzzleState newState)
        {
            if (!puzzleStates.ContainsKey(puzzleID))
            {
                puzzleStates[puzzleID] = newState;
            }
            else
            {
                PuzzleState oldState = puzzleStates[puzzleID];

                if (oldState == PuzzleState.Solved && newState != PuzzleState.Solved)
                {
                    Debug.LogWarning($"[PuzzleManager] Cannot change state of solved puzzle: {puzzleID}");
                    return;
                }

                puzzleStates[puzzleID] = newState;
            }

            Debug.Log($"[PuzzleManager] {puzzleID} state -> {newState}");
            OnPuzzleStateChanged?.Invoke(puzzleID, newState);

            if (newState == PuzzleState.Solved)
            {
                OnPuzzleCompleted?.Invoke(puzzleID);
            }

            if (registeredPuzzles.ContainsKey(puzzleID))
            {
                registeredPuzzles[puzzleID].OnStateChanged(newState);
            }
        }

        public bool IsPuzzleCompleted(string puzzleID)
        {
            return GetPuzzleState(puzzleID) == PuzzleState.Solved;
        }

        public bool IsPuzzleUnlocked(string puzzleID)
        {
            PuzzleState state = GetPuzzleState(puzzleID);
            return state == PuzzleState.Unlocked || state == PuzzleState.InProgress;
        }

        public List<string> GetAllPuzzleIDs()
        {
            return puzzleStates.Keys.ToList();
        }

        public Dictionary<string, PuzzleState> GetAllPuzzleStates()
        {
            return new Dictionary<string, PuzzleState>(puzzleStates);
        }

        public int GetCompletedCount()
        {
            return puzzleStates.Count(kvp => kvp.Value == PuzzleState.Solved);
        }

        public int GetTotalCount()
        {
            return puzzleStates.Count;
        }

        // Debug
        public void UnlockAllPuzzles()
        {
            foreach (var id in puzzleStates.Keys.ToList())
            {
                if (puzzleStates[id] == PuzzleState.Locked)
                {
                    SetPuzzleState(id, PuzzleState.Unlocked);
                }
            }

            Debug.Log("[PuzzleManager] All puzzles unlocked.");
        }

        public void SolvePuzzle(string puzzleID)
        {
            SetPuzzleState(puzzleID, PuzzleState.Solved);
        }

        public void SolveCurrentPuzzle()
        {
            foreach (var kvp in puzzleStates)
            {
                if (kvp.Value == PuzzleState.InProgress)
                {
                    SolvePuzzle(kvp.Key);
                    Debug.Log($"[PuzzleManager] Solved current puzzle: {kvp.Key}");
                    return;
                }
            }

            Debug.Log("[PuzzleManager] No puzzle in progress to solve.");
        }

        // Save/Load integration
        public Dictionary<string, PuzzleState> GetSaveData()
        {
            return new Dictionary<string, PuzzleState>(puzzleStates);
        }

        public void LoadSaveData(Dictionary<string, PuzzleState> data)
        {
            if (data == null) return;

            puzzleStates = new Dictionary<string, PuzzleState>(data);

            foreach (var kvp in registeredPuzzles)
            {
                if (puzzleStates.ContainsKey(kvp.Key))
                {
                    kvp.Value.OnStateChanged(puzzleStates[kvp.Key]);
                }
            }

            Debug.Log($"[PuzzleManager] Loaded {data.Count} puzzle states.");
        }
    }
}
