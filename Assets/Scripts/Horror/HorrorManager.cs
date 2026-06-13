using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace EscapeCampus.Horror
{
    public class HorrorManager : MonoBehaviour
    {
        public static HorrorManager Instance { get; private set; }

        [Header("Horror Level")]
        [SerializeField] private float horrorLevel = 0f;
        [SerializeField] private float maxHorrorLevel = 100f;

        [Header("Escalation")]
        [SerializeField] private float escalationPerPuzzle = 10f;
        [SerializeField] private float escalationPerDocument = 3f;
        [SerializeField] private float escalationPerEvidence = 5f;

        private HorrorStage currentStage;
        private List<string> triggeredEventIDs = new List<string>();
        private Dictionary<string, float> eventCooldowns = new Dictionary<string, float>();

        // Events
        public event System.Action<float> OnHorrorLevelChanged;
        public event System.Action<HorrorStage> OnHorrorStageChanged;
        public event System.Action<string> OnHorrorEventTriggered;

        // Entity hooks (Semester 14)
        public event System.Action<float> OnEscalationForEntity;
        public event System.Action<HorrorStage> OnStageEscalation;

        public float HorrorLevel => horrorLevel;
        public HorrorStage CurrentStage => currentStage;
        public IReadOnlyList<string> TriggeredEventIDs => triggeredEventIDs;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            currentStage = HorrorLevelExtensions.GetStage(horrorLevel);
        }

        private void Start()
        {
            SubscribeToGameEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromGameEvents();
        }

        private void Update()
        {
            UpdateCooldowns();
        }

        // ============================================
        // PUBLIC API
        // ============================================

        public void SetHorrorLevel(float level)
        {
            float oldLevel = horrorLevel;
            horrorLevel = Mathf.Clamp(level, 0f, maxHorrorLevel);

            if (!Mathf.Approximately(oldLevel, horrorLevel))
            {
                OnHorrorLevelChanged?.Invoke(horrorLevel);
                CheckStageChange();
            }
        }

        public void AddHorrorLevel(float amount)
        {
            SetHorrorLevel(horrorLevel + amount);
        }

        public float GetHorrorLevel()
        {
            return horrorLevel;
        }

        public HorrorStage GetHorrorStage()
        {
            return currentStage;
        }

        public bool TriggerHorrorEvent(HorrorEvent horrorEvent)
        {
            if (horrorEvent == null) return false;

            string eventID = horrorEvent.EventID;

            // Check cooldown
            if (IsOnCooldown(eventID))
            {
                Debug.Log($"[HorrorManager] Event {eventID} is on cooldown.");
                return false;
            }

            // Check if already triggered (for one-time events)
            if (horrorEvent.IsOneTime && triggeredEventIDs.Contains(eventID))
            {
                Debug.Log($"[HorrorManager] One-time event {eventID} already triggered.");
                return false;
            }

            // Check horror level requirement
            if (horrorLevel < horrorEvent.MinHorrorLevel)
            {
                Debug.Log($"[HorrorManager] Horror level too low for {eventID}. Need {horrorEvent.MinHorrorLevel}, have {horrorLevel}.");
                return false;
            }

            // Execute event
            bool success = horrorEvent.Execute();

            if (success)
            {
                if (!triggeredEventIDs.Contains(eventID))
                {
                    triggeredEventIDs.Add(eventID);
                }

                SetCooldown(eventID, horrorEvent.Cooldown);
                OnHorrorEventTriggered?.Invoke(eventID);

                Debug.Log($"[HorrorManager] Triggered event: {eventID} (Level: {horrorLevel:F1}, Stage: {currentStage})");
            }

            return success;
        }

        public bool HasTriggeredEvent(string eventID)
        {
            return triggeredEventIDs.Contains(eventID);
        }

        public void ResetEventCooldown(string eventID)
        {
            eventCooldowns.Remove(eventID);
        }

        // ============================================
        // COOLDOWN SYSTEM
        // ============================================

        private bool IsOnCooldown(string eventID)
        {
            return eventCooldowns.ContainsKey(eventID) && eventCooldowns[eventID] > 0f;
        }

        private void SetCooldown(string eventID, float duration)
        {
            eventCooldowns[eventID] = duration;
        }

        private void UpdateCooldowns()
        {
            List<string> keys = eventCooldowns.Keys.ToList();
            foreach (string key in keys)
            {
                eventCooldowns[key] -= Time.deltaTime;
                if (eventCooldowns[key] <= 0f)
                {
                    eventCooldowns.Remove(key);
                }
            }
        }

        // ============================================
        // STAGE MANAGEMENT
        // ============================================

        private void CheckStageChange()
        {
            HorrorStage newStage = HorrorLevelExtensions.GetStage(horrorLevel);

            if (newStage != currentStage)
            {
                HorrorStage oldStage = currentStage;
                currentStage = newStage;

                Debug.Log($"[HorrorManager] Stage changed: {oldStage} -> {newStage} (Level: {horrorLevel:F1})");
                OnHorrorStageChanged?.Invoke(currentStage);
                OnStageEscalation?.Invoke(currentStage);
            }
        }

        // ============================================
        // GAME EVENT SUBSCRIPTIONS
        // ============================================

        private void SubscribeToGameEvents()
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
        }

        private void UnsubscribeFromGameEvents()
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
        }

        private void OnDocumentCollected(Documents.DocumentData document)
        {
            AddHorrorLevel(escalationPerDocument);
            OnEscalationForEntity?.Invoke(horrorLevel);
        }

        private void OnEvidenceCollected(Evidence.EvidenceData evidence)
        {
            AddHorrorLevel(escalationPerEvidence);
            OnEscalationForEntity?.Invoke(horrorLevel);
        }

        private void OnPuzzleCompleted(string puzzleID)
        {
            AddHorrorLevel(escalationPerPuzzle);
            OnEscalationForEntity?.Invoke(horrorLevel);
        }

        // ============================================
        // SEMESTER 14 HOOKS (DO NOT IMPLEMENT ENTITY)
        // ============================================

        public void RegisterEntityEscalationCallback(System.Action<float> callback)
        {
            OnEscalationForEntity += callback;
        }

        public void RegisterStageEscalationCallback(System.Action<HorrorStage> callback)
        {
            OnStageEscalation += callback;
        }

        // ============================================
        // SAVE/LOAD
        // ============================================

        public HorrorSaveData GetSaveData()
        {
            return new HorrorSaveData
            {
                horrorLevel = horrorLevel,
                triggeredEventIDs = new List<string>(triggeredEventIDs),
                cooldowns = new Dictionary<string, float>(eventCooldowns)
            };
        }

        public void LoadSaveData(HorrorSaveData data)
        {
            if (data == null) return;

            horrorLevel = data.horrorLevel;
            triggeredEventIDs = new List<string>(data.triggeredEventIDs ?? new List<string>());
            eventCooldowns = new Dictionary<string, float>(data.cooldowns ?? new Dictionary<string, float>());
            currentStage = HorrorLevelExtensions.GetStage(horrorLevel);

            OnHorrorLevelChanged?.Invoke(horrorLevel);
            OnHorrorStageChanged?.Invoke(currentStage);

            Debug.Log($"[HorrorManager] Loaded: Level={horrorLevel:F1}, Stage={currentStage}, Events={triggeredEventIDs.Count}");
        }

        // ============================================
        // DEBUG
        // ============================================

        public void DebugSetLevel(float level)
        {
            SetHorrorLevel(level);
        }

        public void DebugResetAll()
        {
            horrorLevel = 0f;
            triggeredEventIDs.Clear();
            eventCooldowns.Clear();
            currentStage = HorrorStage.Calm;

            OnHorrorLevelChanged?.Invoke(horrorLevel);
            OnHorrorStageChanged?.Invoke(currentStage);

            Debug.Log("[HorrorManager] Debug reset all horror state.");
        }
    }

    [System.Serializable]
    public class HorrorSaveData
    {
        public float horrorLevel;
        public List<string> triggeredEventIDs;
        public Dictionary<string, float> cooldowns;
    }
}
