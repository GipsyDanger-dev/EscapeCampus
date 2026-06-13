using UnityEngine;
using System;
using System.Collections.Generic;
using EscapeCampus.Documents;
using EscapeCampus.Evidence;
using EscapeCampus.Horror;
using EscapeCampus.Puzzle;

namespace EscapeCampus.Core
{
    public enum TriggerType
    {
        PuzzleCompleted,
        DocumentCollected,
        EvidenceCollected,
        HorrorLevelReached,
        StoryPhaseEntered,
        TimePlayed,
        Custom
    }

    [Serializable]
    public class NarrativeCondition
    {
        public TriggerType triggerType;
        public string targetID; // Puzzle ID, Document ID, etc.
        public float threshold; // For numeric conditions
        public ComparisonType comparison = ComparisonType.GreaterOrEqual;
    }

    public enum ComparisonType
    {
        Equal,
        NotEqual,
        Greater,
        GreaterOrEqual,
        Less,
        LessOrEqual
    }

    public class NarrativeTrigger : MonoBehaviour
    {
        [Header("Trigger Identity")]
        [SerializeField] private string triggerID;
        [SerializeField] private string triggerName;

        [Header("Conditions")]
        [SerializeField] private List<NarrativeCondition> conditions = new List<NarrativeCondition>();
        [SerializeField] private bool requireAllConditions = true; // AND vs OR

        [Header("Actions")]
        [SerializeField] private StoryPhase targetPhase = StoryPhase.Introduction;
        [SerializeField] private bool advanceToPhase = false;
        [SerializeField] private bool triggerHorrorEvent = false;
        [SerializeField] private string horrorEventID;
        [SerializeField] private bool changeWorldState = false;
        [SerializeField] private WorldState targetWorldState;

        [Header("Settings")]
        [SerializeField] private bool oneTimeOnly = true;
        [SerializeField] private bool isEnabled = true;

        private bool hasTriggered = false;

        public string TriggerID => triggerID;
        public bool HasTriggered => hasTriggered;

        private void Start()
        {
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        // ============================================
        // CONDITION CHECKING
        // ============================================

        public bool CheckConditions()
        {
            if (!isEnabled) return false;
            if (oneTimeOnly && hasTriggered) return false;

            if (conditions.Count == 0) return true;

            if (requireAllConditions)
            {
                // All conditions must be true (AND)
                foreach (var condition in conditions)
                {
                    if (!EvaluateCondition(condition))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                // At least one condition must be true (OR)
                foreach (var condition in conditions)
                {
                    if (EvaluateCondition(condition))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private bool EvaluateCondition(NarrativeCondition condition)
        {
            switch (condition.triggerType)
            {
                case TriggerType.PuzzleCompleted:
                    return EvaluatePuzzleCondition(condition);

                case TriggerType.DocumentCollected:
                    return EvaluateDocumentCondition(condition);

                case TriggerType.EvidenceCollected:
                    return EvaluateEvidenceCondition(condition);

                case TriggerType.HorrorLevelReached:
                    return EvaluateHorrorCondition(condition);

                case TriggerType.StoryPhaseEntered:
                    return EvaluatePhaseCondition(condition);

                case TriggerType.TimePlayed:
                    return EvaluateTimeCondition(condition);

                default:
                    return false;
            }
        }

        private bool EvaluatePuzzleCondition(NarrativeCondition condition)
        {
            if (PuzzleManager.Instance == null) return false;

            if (!string.IsNullOrEmpty(condition.targetID))
            {
                // Check specific puzzle
                bool completed = PuzzleManager.Instance.IsPuzzleCompleted(condition.targetID);
                return CompareValues(completed ? 1 : 0, (int)condition.threshold, condition.comparison);
            }
            else
            {
                // Check total completed count
                return CompareValues(PuzzleManager.Instance.GetCompletedCount(), (int)condition.threshold, condition.comparison);
            }
        }

        private bool EvaluateDocumentCondition(NarrativeCondition condition)
        {
            if (DocumentManager.Instance == null) return false;

            if (!string.IsNullOrEmpty(condition.targetID))
            {
                bool collected = DocumentManager.Instance.HasDocument(condition.targetID);
                return CompareValues(collected ? 1 : 0, (int)condition.threshold, condition.comparison);
            }
            else
            {
                return CompareValues(DocumentManager.Instance.TotalCollected, (int)condition.threshold, condition.comparison);
            }
        }

        private bool EvaluateEvidenceCondition(NarrativeCondition condition)
        {
            if (EvidenceManager.Instance == null) return false;

            if (!string.IsNullOrEmpty(condition.targetID))
            {
                bool collected = EvidenceManager.Instance.HasEvidence(condition.targetID);
                return CompareValues(collected ? 1 : 0, (int)condition.threshold, condition.comparison);
            }
            else
            {
                return CompareValues(EvidenceManager.Instance.TotalCollected, (int)condition.threshold, condition.comparison);
            }
        }

        private bool EvaluateHorrorCondition(NarrativeCondition condition)
        {
            if (HorrorManager.Instance == null) return false;
            return CompareValues(HorrorManager.Instance.GetHorrorLevel(), condition.threshold, condition.comparison);
        }

        private bool EvaluatePhaseCondition(NarrativeCondition condition)
        {
            if (LevelFlowManager.Instance == null) return false;

            StoryPhase currentPhase = LevelFlowManager.Instance.CurrentPhase;
            return CompareValues((int)currentPhase, (int)condition.threshold, condition.comparison);
        }

        private bool EvaluateTimeCondition(NarrativeCondition condition)
        {
            return CompareValues(Time.timeSinceLevelLoad, condition.threshold, condition.comparison);
        }

        private bool CompareValues(float current, float target, ComparisonType comparison)
        {
            switch (comparison)
            {
                case ComparisonType.Equal: return Mathf.Approximately(current, target);
                case ComparisonType.NotEqual: return !Mathf.Approximately(current, target);
                case ComparisonType.Greater: return current > target;
                case ComparisonType.GreaterOrEqual: return current >= target;
                case ComparisonType.Less: return current < target;
                case ComparisonType.LessOrEqual: return current <= target;
                default: return false;
            }
        }

        private bool CompareValues(int current, int target, ComparisonType comparison)
        {
            switch (comparison)
            {
                case ComparisonType.Equal: return current == target;
                case ComparisonType.NotEqual: return current != target;
                case ComparisonType.Greater: return current > target;
                case ComparisonType.GreaterOrEqual: return current >= target;
                case ComparisonType.Less: return current < target;
                case ComparisonType.LessOrEqual: return current <= target;
                default: return false;
            }
        }

        // ============================================
        // ACTIONS
        // ============================================

        public void ExecuteTrigger()
        {
            if (!CheckConditions()) return;

            Debug.Log($"[NarrativeTrigger] Executing trigger: {triggerID}");

            if (advanceToPhase && LevelFlowManager.Instance != null)
            {
                LevelFlowManager.Instance.SetStoryPhase(targetPhase);
            }

            if (triggerHorrorEvent && HorrorManager.Instance != null)
            {
                // Find and trigger horror event by ID
                HorrorEvent[] events = FindObjectsOfType<HorrorEvent>();
                foreach (var evt in events)
                {
                    if (evt.EventID == horrorEventID)
                    {
                        HorrorManager.Instance.TriggerHorrorEvent(evt);
                        break;
                    }
                }
            }

            if (changeWorldState && WorldStateManager.Instance != null)
            {
                WorldStateManager.Instance.SetWorldState(targetWorldState);
            }

            hasTriggered = true;
        }

        // ============================================
        // EVENT SUBSCRIPTIONS
        // ============================================

        private void SubscribeToEvents()
        {
            if (PuzzleManager.Instance != null)
            {
                PuzzleManager.Instance.OnPuzzleCompleted += OnPuzzleCompleted;
            }

            if (DocumentManager.Instance != null)
            {
                DocumentManager.Instance.OnDocumentCollected += OnDocumentCollected;
            }

            if (EvidenceManager.Instance != null)
            {
                EvidenceManager.Instance.OnEvidenceCollected += OnEvidenceCollected;
            }

            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.OnHorrorLevelChanged += OnHorrorLevelChanged;
            }

            if (LevelFlowManager.Instance != null)
            {
                LevelFlowManager.Instance.OnPhaseEntered += OnPhaseEntered;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (PuzzleManager.Instance != null)
            {
                PuzzleManager.Instance.OnPuzzleCompleted -= OnPuzzleCompleted;
            }

            if (DocumentManager.Instance != null)
            {
                DocumentManager.Instance.OnDocumentCollected -= OnDocumentCollected;
            }

            if (EvidenceManager.Instance != null)
            {
                EvidenceManager.Instance.OnEvidenceCollected -= OnEvidenceCollected;
            }

            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.OnHorrorLevelChanged -= OnHorrorLevelChanged;
            }

            if (LevelFlowManager.Instance != null)
            {
                LevelFlowManager.Instance.OnPhaseEntered -= OnPhaseEntered;
            }
        }

        private void OnPuzzleCompleted(string puzzleID)
        {
            ExecuteTrigger();
        }

        private void OnDocumentCollected(DocumentData document)
        {
            ExecuteTrigger();
        }

        private void OnEvidenceCollected(EvidenceData evidence)
        {
            ExecuteTrigger();
        }

        private void OnHorrorLevelChanged(float level)
        {
            ExecuteTrigger();
        }

        private void OnPhaseEntered(StoryPhase phase)
        {
            ExecuteTrigger();
        }
    }
}
