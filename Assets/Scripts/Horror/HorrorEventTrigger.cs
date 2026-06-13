using UnityEngine;
using System.Collections.Generic;

namespace EscapeCampus.Horror
{
    public class HorrorEventTrigger : MonoBehaviour
    {
        [Header("Event Pool")]
        [SerializeField] private List<HorrorEvent> eventPool = new List<HorrorEvent>();

        [Header("Trigger Settings")]
        [SerializeField] private float baseTriggerChance = 0.3f;
        [SerializeField] private float minCooldownBetweenEvents = 30f;
        [SerializeField] private float timePlayedThreshold = 60f; // Minimum 60s before first event

        [Header("Weight by Stage")]
        [SerializeField] private float calmWeight = 0.1f;
        [SerializeField] private float uneaseWeight = 0.3f;
        [SerializeField] private float disturbanceWeight = 0.5f;
        [SerializeField] private float paranoiaWeight = 0.7f;
        [SerializeField] private float collapseWeight = 0.9f;

        private float lastEventTime;
        private float timePlayed;

        private void Start()
        {
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void Update()
        {
            timePlayed += Time.deltaTime;
        }

        private void SubscribeToEvents()
        {
            if (Documents.DocumentManager.Instance != null)
            {
                Documents.DocumentManager.Instance.OnDocumentCollected += OnDocumentCollected;
            }

            if (Evidence.EvidenceManager.Instance != null)
            {
                Evidence.EvidenceManager.Instance.OnEvidenceCollected += OnEvidenceCollected;
            }

            if (Puzzle.PuzzleManager.Instance != null)
            {
                Puzzle.PuzzleManager.Instance.OnPuzzleCompleted += OnPuzzleCompleted;
            }

            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.OnHorrorStageChanged += OnHorrorStageChanged;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (Documents.DocumentManager.Instance != null)
            {
                Documents.DocumentManager.Instance.OnDocumentCollected -= OnDocumentCollected;
            }

            if (Evidence.EvidenceManager.Instance != null)
            {
                Evidence.EvidenceManager.Instance.OnEvidenceCollected -= OnEvidenceCollected;
            }

            if (Puzzle.PuzzleManager.Instance != null)
            {
                Puzzle.PuzzleManager.Instance.OnPuzzleCompleted -= OnPuzzleCompleted;
            }

            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.OnHorrorStageChanged -= OnHorrorStageChanged;
            }
        }

        // ============================================
        // TRIGGER HANDLERS
        // ============================================

        private void OnDocumentCollected(Documents.DocumentData document)
        {
            TryTriggerRandomEvent("document_collected");
        }

        private void OnEvidenceCollected(Evidence.EvidenceData evidence)
        {
            TryTriggerRandomEvent("evidence_collected");
        }

        private void OnPuzzleCompleted(string puzzleID)
        {
            TryTriggerRandomEvent("puzzle_solved");
        }

        private void OnHorrorStageChanged(HorrorStage newStage)
        {
            // Higher chance to trigger on stage change
            if (newStage >= HorrorStage.Disturbance)
            {
                TryTriggerRandomEvent("stage_change", 0.5f);
            }
        }

        // ============================================
        // TRIGGER LOGIC
        // ============================================

        private void TryTriggerRandomEvent(string context, float chanceOverride = -1f)
        {
            // Check time threshold
            if (timePlayed < timePlayedThreshold) return;

            // Check cooldown
            if (Time.time - lastEventTime < minCooldownBetweenEvents) return;

            // Calculate chance
            float chance = chanceOverride > 0 ? chanceOverride : GetWeightedChance();

            // Random check
            if (Random.value > chance) return;

            // Find available event
            HorrorEvent selectedEvent = SelectRandomEvent();
            if (selectedEvent == null) return;

            // Trigger
            bool success = HorrorManager.Instance?.TriggerHorrorEvent(selectedEvent) ?? false;

            if (success)
            {
                lastEventTime = Time.time;
                Debug.Log($"[HorrorEventTrigger] Triggered {selectedEvent.EventID} (context: {context})");
            }
        }

        private float GetWeightedChance()
        {
            if (HorrorManager.Instance == null) return baseTriggerChance;

            HorrorStage stage = HorrorManager.Instance.GetHorrorStage();

            switch (stage)
            {
                case HorrorStage.Calm:
                    return baseTriggerChance * calmWeight;
                case HorrorStage.Unease:
                    return baseTriggerChance * uneaseWeight;
                case HorrorStage.Disturbance:
                    return baseTriggerChance * disturbanceWeight;
                case HorrorStage.Paranoia:
                    return baseTriggerChance * paranoiaWeight;
                case HorrorStage.Collapse:
                    return baseTriggerChance * collapseWeight;
                default:
                    return baseTriggerChance;
            }
        }

        private HorrorEvent SelectRandomEvent()
        {
            if (eventPool.Count == 0) return null;

            // Filter available events
            List<HorrorEvent> available = new List<HorrorEvent>();
            foreach (HorrorEvent evt in eventPool)
            {
                if (evt != null && evt.CanExecute())
                {
                    available.Add(evt);
                }
            }

            if (available.Count == 0) return null;

            return available[Random.Range(0, available.Count)];
        }

        // ============================================
        // PUBLIC API
        // ============================================

        public void AddEvent(HorrorEvent horrorEvent)
        {
            if (horrorEvent != null && !eventPool.Contains(horrorEvent))
            {
                eventPool.Add(horrorEvent);
            }
        }

        public void RemoveEvent(HorrorEvent horrorEvent)
        {
            eventPool.Remove(horrorEvent);
        }
    }
}
