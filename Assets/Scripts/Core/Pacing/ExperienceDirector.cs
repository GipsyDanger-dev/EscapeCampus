using UnityEngine;
using System;
using System.Collections.Generic;
using EscapeCampus.Documents;
using EscapeCampus.Evidence;
using EscapeCampus.Horror;
using EscapeCampus.Horror.Semester14;
using EscapeCampus.Horror.SetPieces;
using EscapeCampus.Puzzle;

namespace EscapeCampus.Core.Pacing
{
    public class ExperienceDirector : MonoBehaviour
    {
        public static ExperienceDirector Instance { get; private set; }

        [Header("Tension")]
        [SerializeField] private float tensionLevel = 10f;
        [SerializeField] private float tensionDecayRate = 2f;
        [SerializeField] private float tensionBuildRate = 5f;

        [Header("Current State")]
        [SerializeField] private TensionState currentTensionState = TensionState.Calm;
        [SerializeField] private NarrativeBeat currentBeat = NarrativeBeat.Exploration;
        [SerializeField] private NarrativeBeat nextExpectedBeat = NarrativeBeat.Discovery;

        [Header("Pacing Settings")]
        [SerializeField] private float beatDuration = 30f;
        [SerializeField] private float minSilenceDuration = 15f;
        [SerializeField] private float maxHorrorFrequency = 0.3f;

        [Header("Safe Zone")]
        [SerializeField] private bool isInSafeZone;
        [SerializeField] private float safeZoneDecayRate = 5f;

        // Tracking
        private float timeInCurrentBeat;
        private float timeSinceLastHorror;
        private float timeSinceLastSetPiece;
        private float timeSinceLastSilence;
        private float totalPlayTime;
        private int horrorEventsThisPhase;
        private int setPiecesThisPhase;

        // Beat rotation
        private Queue<NarrativeBeat> beatQueue = new Queue<NarrativeBeat>();
        private List<NarrativeBeat> beatHistory = new List<NarrativeBeat>();

        // Events
        public event Action<TensionState, TensionState> OnTensionStateChanged;
        public event Action<NarrativeBeat, NarrativeBeat> OnBeatChanged;
        public event Action<float> OnTensionLevelChanged;
        public event Action<bool> OnSafeZoneChanged;

        public float TensionLevel => tensionLevel;
        public TensionState CurrentTensionState => currentTensionState;
        public NarrativeBeat CurrentBeat => currentBeat;
        public NarrativeBeat NextExpectedBeat => nextExpectedBeat;
        public bool IsInSafeZone => isInSafeZone;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeBeatQueue();
        }

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
            totalPlayTime += Time.deltaTime;
            timeInCurrentBeat += Time.deltaTime;
            timeSinceLastHorror += Time.deltaTime;
            timeSinceLastSetPiece += Time.deltaTime;
            timeSinceLastSilence += Time.deltaTime;

            EvaluateGameState();
            AdjustPacing();
            UpdateTension();
        }

        // ============================================
        // PUBLIC API
        // ============================================

        public void EvaluateGameState()
        {
            // Calculate tension modifiers
            float tensionModifier = 0f;

            // Horror level influence
            if (HorrorManager.Instance != null)
            {
                float horrorLevel = HorrorManager.Instance.GetHorrorLevel();
                tensionModifier += horrorLevel * 0.3f;
            }

            // Recent horror events increase tension
            if (timeSinceLastHorror < 60f)
            {
                tensionModifier += (1f - timeSinceLastHorror / 60f) * 15f;
            }

            // SetPiece recency
            if (timeSinceLastSetPiece < 120f)
            {
                tensionModifier += (1f - timeSinceLastSetPiece / 120f) * 10f;
            }

            // Safe zone reduces tension
            if (isInSafeZone)
            {
                tensionModifier -= 20f;
            }

            // Story phase influence
            if (LevelFlowManager.Instance != null)
            {
                StoryPhase phase = LevelFlowManager.Instance.CurrentPhase;
                switch (phase)
                {
                    case StoryPhase.Introduction:
                    case StoryPhase.EarlyInvestigation:
                        tensionModifier -= 10f;
                        break;
                    case StoryPhase.FirstAnomaly:
                        tensionModifier += 5f;
                        break;
                    case StoryPhase.DeepInvestigation:
                        tensionModifier += 10f;
                        break;
                    case StoryPhase.RealityBreakdown:
                        tensionModifier += 20f;
                        break;
                    case StoryPhase.FinalPreparation:
                        tensionModifier += 25f;
                        break;
                    case StoryPhase.FinalChase:
                        tensionModifier += 40f;
                        break;
                }
            }

            // Apply modifier
            tensionLevel += tensionModifier * Time.deltaTime * 0.1f;
            tensionLevel = Mathf.Clamp(tensionLevel, 0f, 100f);
        }

        public void AdjustPacing()
        {
            // Check if beat should change
            if (timeInCurrentBeat >= beatDuration)
            {
                AdvanceToNextBeat();
            }

            // Force silence if too much horror
            if (horrorEventsThisPhase >= 3 && currentBeat != NarrativeBeat.Silence)
            {
                ForceBeat(NarrativeBeat.Silence);
            }

            // Force silence if too many setpieces
            if (setPiecesThisPhase >= 2 && currentBeat != NarrativeBeat.Silence)
            {
                ForceBeat(NarrativeBeat.Silence);
            }
        }

        public void TriggerNarrativeBeat(NarrativeBeat beat)
        {
            if (beat == currentBeat) return;

            NarrativeBeat previousBeat = currentBeat;
            currentBeat = beat;
            timeInCurrentBeat = 0f;

            beatHistory.Add(beat);
            if (beatHistory.Count > 20)
            {
                beatHistory.RemoveAt(0);
            }

            Debug.Log($"[ExperienceDirector] Beat changed: {previousBeat} → {beat}");
            OnBeatChanged?.Invoke(previousBeat, beat);

            // Update next expected beat
            nextExpectedBeat = PredictNextBeat();
        }

        public void ControlTensionCurve(float targetLevel, float duration)
        {
            // Smoothly move tension toward target
            tensionLevel = Mathf.Lerp(tensionLevel, targetLevel, Time.deltaTime / duration);
        }

        public void SetSafeZone(bool safe)
        {
            if (isInSafeZone == safe) return;

            isInSafeZone = safe;
            OnSafeZoneChanged?.Invoke(safe);

            Debug.Log($"[ExperienceDirector] Safe zone: {safe}");
        }

        // ============================================
        // TENSION MANAGEMENT
        // ============================================

        private void UpdateTension()
        {
            float previousLevel = tensionLevel;

            // Natural decay
            if (!isInSafeZone)
            {
                tensionLevel -= tensionDecayRate * Time.deltaTime;
            }
            else
            {
                tensionLevel -= safeZoneDecayRate * Time.deltaTime;
            }

            // Clamp
            tensionLevel = Mathf.Clamp(tensionLevel, 0f, 100f);

            // Check state change
            TensionState newState = TensionLevelExtensions.GetState(tensionLevel);
            if (newState != currentTensionState)
            {
                TensionState oldState = currentTensionState;
                currentTensionState = newState;
                OnTensionStateChanged?.Invoke(oldState, newState);
                Debug.Log($"[ExperienceDirector] Tension: {oldState} → {newState} ({tensionLevel:F1})");
            }

            // Notify level change
            if (!Mathf.Approximately(previousLevel, tensionLevel))
            {
                OnTensionLevelChanged?.Invoke(tensionLevel);
            }
        }

        public void AddTension(float amount)
        {
            tensionLevel = Mathf.Clamp(tensionLevel + amount, 0f, 100f);
        }

        public void RemoveTension(float amount)
        {
            tensionLevel = Mathf.Clamp(tensionLevel - amount, 0f, 100f);
        }

        // ============================================
        // BEAT MANAGEMENT
        // ============================================

        private void InitializeBeatQueue()
        {
            // Start with exploration
            beatQueue.Enqueue(NarrativeBeat.Exploration);
            beatQueue.Enqueue(NarrativeBeat.Discovery);
            beatQueue.Enqueue(NarrativeBeat.Exploration);
            beatQueue.Enqueue(NarrativeBeat.Silence);

            currentBeat = NarrativeBeat.Exploration;
            nextExpectedBeat = NarrativeBeat.Discovery;
        }

        private void AdvanceToNextBeat()
        {
            if (beatQueue.Count > 0)
            {
                NarrativeBeat nextBeat = beatQueue.Dequeue();
                TriggerNarrativeBeat(nextBeat);
            }
            else
            {
                // Regenerate queue based on state
                RegenerateBeatQueue();
                if (beatQueue.Count > 0)
                {
                    TriggerNarrativeBeat(beatQueue.Dequeue());
                }
            }
        }

        private void ForceBeat(NarrativeBeat beat)
        {
            beatQueue.Clear();
            TriggerNarrativeBeat(beat);
        }

        private void RegenerateBeatQueue()
        {
            List<NarrativeBeat> newBeats = new List<NarrativeBeat>();

            // Base rotation
            newBeats.Add(NarrativeBeat.Exploration);
            newBeats.Add(NarrativeBeat.Discovery);

            // Add horror based on tension
            if (tensionLevel > 30f && !isInSafeZone)
            {
                newBeats.Add(NarrativeBeat.Horror);
            }

            // Add silence periodically
            if (timeSinceLastSilence > 60f)
            {
                newBeats.Add(NarrativeBeat.Silence);
            }

            // Add revelation based on story phase
            if (LevelFlowManager.Instance != null)
            {
                StoryPhase phase = LevelFlowManager.Instance.CurrentPhase;
                if (phase >= StoryPhase.FirstAnomaly)
                {
                    newBeats.Add(NarrativeBeat.Revelation);
                }
            }

            // Shuffle
            for (int i = newBeats.Count - 1; i > 0; i--)
            {
                int j = UnityEngine.Random.Range(0, i + 1);
                NarrativeBeat temp = newBeats[i];
                newBeats[i] = newBeats[j];
                newBeats[j] = temp;
            }

            foreach (var beat in newBeats)
            {
                beatQueue.Enqueue(beat);
            }
        }

        private NarrativeBeat PredictNextBeat()
        {
            if (beatQueue.Count > 0)
            {
                return beatQueue.Peek();
            }

            // Predict based on state
            if (tensionLevel > 60f) return NarrativeBeat.Horror;
            if (timeSinceLastSilence > 60f) return NarrativeBeat.Silence;
            return NarrativeBeat.Exploration;
        }

        // ============================================
        // EVENT TRACKING
        // ============================================

        private void SubscribeToEvents()
        {
            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.OnHorrorEventTriggered += OnHorrorEventTriggered;
            }

            if (SetPieceManager.Instance != null)
            {
                SetPieceManager.Instance.OnSetPieceTriggered += OnSetPieceTriggered;
            }

            if (LevelFlowManager.Instance != null)
            {
                LevelFlowManager.Instance.OnPhaseEntered += OnPhaseEntered;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.OnHorrorEventTriggered -= OnHorrorEventTriggered;
            }

            if (SetPieceManager.Instance != null)
            {
                SetPieceManager.Instance.OnSetPieceTriggered -= OnSetPieceTriggered;
            }

            if (LevelFlowManager.Instance != null)
            {
                LevelFlowManager.Instance.OnPhaseEntered -= OnPhaseEntered;
            }
        }

        private void OnHorrorEventTriggered(string eventID)
        {
            timeSinceLastHorror = 0f;
            horrorEventsThisPhase++;
            AddTension(10f);
        }

        private void OnSetPieceTriggered(string setPieceID)
        {
            timeSinceLastSetPiece = 0f;
            setPiecesThisPhase++;
            TriggerNarrativeBeat(NarrativeBeat.SetPiece);
            AddTension(20f);
        }

        private void OnPhaseEntered(StoryPhase phase)
        {
            horrorEventsThisPhase = 0;
            setPiecesThisPhase = 0;

            // Adjust pacing for phase
            switch (phase)
            {
                case StoryPhase.FinalChase:
                    PrepareFinalChase();
                    break;
            }
        }

        // ============================================
        // FINAL CHASE PREPARATION
        // ============================================

        private void PrepareFinalChase()
        {
            Debug.Log("[ExperienceDirector] Preparing Final Chase...");

            // Escalate tension
            ControlTensionCurve(90f, 5f);

            // Lock world state
            WorldStateManager.Instance?.SetWorldState(WorldState.BrokenReality);

            // Force horror beat
            ForceBeat(NarrativeBeat.Horror);

            // S14 becomes permanent
            if (Semester14Observer.Instance != null)
            {
                // Disable cooldown for constant presence
                Semester14Observer.Instance.DebugForceSpawn();
            }

            // Suppress random events - only scripted
            beatDuration = 15f; // Faster beats
        }

        // ============================================
        // DYNAMIC EVENT CONTROL
        // ============================================

        public bool ShouldAllowHorrorEvent()
        {
            // Don't allow if in safe zone
            if (isInSafeZone) return false;

            // Don't allow if too many recent events
            if (horrorEventsThisPhase >= 3) return false;

            // Don't allow if in silence beat
            if (currentBeat == NarrativeBeat.Silence) return false;

            // Check frequency
            float horrorFrequency = horrorEventsThisPhase / Mathf.Max(1f, totalPlayTime / 60f);
            if (horrorFrequency > maxHorrorFrequency) return false;

            return true;
        }

        public bool ShouldAllowSetPiece()
        {
            // Don't allow if in safe zone
            if (isInSafeZone) return false;

            // Don't allow if too many this phase
            if (setPiecesThisPhase >= 2) return false;

            // Don't allow if in silence beat
            if (currentBeat == NarrativeBeat.Silence) return false;

            return true;
        }

        public void SuppressRandomness(float duration)
        {
            // Temporarily suppress random horror events
            StartCoroutine(SuppressRandomnessCoroutine(duration));
        }

        private System.Collections.IEnumerator SuppressRandomnessCoroutine(float duration)
        {
            float originalMax = maxHorrorFrequency;
            maxHorrorFrequency = 0f;
            yield return new WaitForSeconds(duration);
            maxHorrorFrequency = originalMax;
        }

        public void ForceNarrativeTrigger(string triggerID)
        {
            Debug.Log($"[ExperienceDirector] Force narrative trigger: {triggerID}");
            // Handled by NarrativeTrigger system
        }

        // ============================================
        // SAVE/LOAD
        // ============================================

        public PacingSaveData GetSaveData()
        {
            return new PacingSaveData
            {
                tensionLevel = tensionLevel,
                currentBeat = currentBeat.ToString(),
                totalPlayTime = totalPlayTime
            };
        }

        public void LoadSaveData(PacingSaveData data)
        {
            if (data == null) return;

            tensionLevel = data.tensionLevel;

            if (Enum.TryParse(data.currentBeat, out NarrativeBeat beat))
            {
                currentBeat = beat;
            }

            totalPlayTime = data.totalPlayTime;

            Debug.Log($"[ExperienceDirector] Loaded: Tension={tensionLevel:F1}, Beat={currentBeat}");
        }
    }

    [Serializable]
    public class PacingSaveData
    {
        public float tensionLevel;
        public string currentBeat;
        public float totalPlayTime;
    }
}
