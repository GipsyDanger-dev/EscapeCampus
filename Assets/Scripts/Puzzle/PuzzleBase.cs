using UnityEngine;
using System.Collections.Generic;
using EscapeCampus.Documents;
using EscapeCampus.Evidence;

namespace EscapeCampus.Puzzle
{
    public abstract class PuzzleBase : MonoBehaviour
    {
        [Header("Puzzle Identity")]
        [SerializeField] protected string puzzleID;
        [SerializeField] protected string puzzleName;

        [Header("Initial State")]
        [SerializeField] protected PuzzleState initialState = PuzzleState.Locked;

        [Header("Requirements")]
        [SerializeField] protected List<DocumentData> requiredDocuments = new List<DocumentData>();
        [SerializeField] protected List<EvidenceData> requiredEvidence = new List<EvidenceData>();

        public string PuzzleID => puzzleID;
        public string PuzzleName => puzzleName;
        public PuzzleState InitialState => initialState;
        public PuzzleState CurrentState => PuzzleManager.Instance?.GetPuzzleState(puzzleID) ?? initialState;

        protected virtual void Start()
        {
            if (PuzzleManager.Instance != null)
            {
                PuzzleManager.Instance.RegisterPuzzle(this);
            }
            else
            {
                Debug.LogError($"[PuzzleBase] PuzzleManager not found! Puzzle {puzzleID} cannot register.");
            }
        }

        protected virtual void OnDestroy()
        {
            if (PuzzleManager.Instance != null)
            {
                PuzzleManager.Instance.UnregisterPuzzle(this);
            }
        }

        public virtual bool ValidateConditions()
        {
            // Check required documents
            if (requiredDocuments != null && requiredDocuments.Count > 0)
            {
                DocumentManager docManager = DocumentManager.Instance;
                if (docManager == null) return false;

                foreach (DocumentData doc in requiredDocuments)
                {
                    if (doc != null && !docManager.HasDocument(doc))
                    {
                        return false;
                    }
                }
            }

            // Check required evidence
            if (requiredEvidence != null && requiredEvidence.Count > 0)
            {
                EvidenceManager evManager = EvidenceManager.Instance;
                if (evManager == null) return false;

                foreach (EvidenceData ev in requiredEvidence)
                {
                    if (ev != null && !evManager.HasEvidence(ev))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public virtual void StartPuzzle()
        {
            if (CurrentState == PuzzleState.Locked)
            {
                Debug.Log($"[Puzzle] {puzzleID} is locked. Cannot start.");
                return;
            }

            if (CurrentState == PuzzleState.Solved)
            {
                Debug.Log($"[Puzzle] {puzzleID} is already solved.");
                return;
            }

            if (!ValidateConditions())
            {
                Debug.Log($"[Puzzle] {puzzleID} requirements not met.");
                OnRequirementsNotMet();
                return;
            }

            PuzzleManager.Instance?.SetPuzzleState(puzzleID, PuzzleState.InProgress);
            OnPuzzleStarted();
        }

        public virtual void CompletePuzzle()
        {
            if (CurrentState != PuzzleState.InProgress)
            {
                Debug.LogWarning($"[Puzzle] {puzzleID} is not in progress. Current state: {CurrentState}");
                return;
            }

            PuzzleManager.Instance?.SetPuzzleState(puzzleID, PuzzleState.Solved);
            OnPuzzleCompleted();
        }

        public void Unlock()
        {
            if (CurrentState == PuzzleState.Locked)
            {
                PuzzleManager.Instance?.SetPuzzleState(puzzleID, PuzzleState.Unlocked);
            }
        }

        public virtual void OnStateChanged(PuzzleState newState)
        {
            // Override in subclasses to react to state changes
        }

        protected virtual void OnPuzzleStarted()
        {
            Debug.Log($"[Puzzle] {puzzleID} started.");
        }

        protected virtual void OnPuzzleCompleted()
        {
            Debug.Log($"[Puzzle] {puzzleID} completed!");
        }

        protected virtual void OnRequirementsNotMet()
        {
            Debug.Log($"[Puzzle] {puzzleID} - Requirements not met.");
        }
    }
}
