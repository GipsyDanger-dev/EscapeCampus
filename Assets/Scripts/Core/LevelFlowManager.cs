using UnityEngine;
using System;
using System.Collections.Generic;
using EscapeCampus.Documents;
using EscapeCampus.Evidence;
using EscapeCampus.Horror;
using EscapeCampus.Puzzle;

namespace EscapeCampus.Core
{
    public class LevelFlowManager : MonoBehaviour
    {
        public static LevelFlowManager Instance { get; private set; }

        [Header("Current State")]
        [SerializeField] private StoryPhase currentPhase = StoryPhase.Introduction;

        [Header("Progression Rules")]
        [SerializeField] private List<PhaseTransitionRule> transitionRules = new List<PhaseTransitionRule>();

        private Dictionary<StoryPhase, bool> phaseCompleted = new Dictionary<StoryPhase, bool>();

        // Events
        public event Action<StoryPhase, StoryPhase> OnPhaseChanged;
        public event Action<StoryPhase> OnPhaseEntered;
        public event Action<StoryPhase> OnPhaseCompleted;

        public StoryPhase CurrentPhase => currentPhase;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeDefaultRules();
            InitializePhaseTracking();
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
            CheckTransitionRules();
        }

        // ============================================
        // PUBLIC API
        // ============================================

        public void SetStoryPhase(StoryPhase phase)
        {
            if (phase == currentPhase) return;

            StoryPhase oldPhase = currentPhase;
            currentPhase = phase;

            Debug.Log($"[LevelFlowManager] Phase changed: {oldPhase} -> {phase}");
            OnPhaseChanged?.Invoke(oldPhase, phase);
            OnPhaseEntered?.Invoke(phase);

            // Trigger world state changes based on phase
            ApplyPhaseWorldState(phase);
        }

        public void AdvancePhase()
        {
            int nextIndex = (int)currentPhase + 1;
            if (nextIndex <= (int)StoryPhase.Ending)
            {
                SetStoryPhase((StoryPhase)nextIndex);
            }
            else
            {
                Debug.Log("[LevelFlowManager] Already at final phase.");
            }
        }

        public void GoToPreviousPhase()
        {
            int prevIndex = (int)currentPhase - 1;
            if (prevIndex >= 0)
            {
                SetStoryPhase((StoryPhase)prevIndex);
            }
            else
            {
                Debug.Log("[LevelFlowManager] Already at first phase.");
            }
        }

        public StoryPhase GetCurrentPhase()
        {
            return currentPhase;
        }

        public bool HasCompletedPhase(StoryPhase phase)
        {
            return phaseCompleted.ContainsKey(phase) && phaseCompleted[phase];
        }

        public void MarkPhaseCompleted(StoryPhase phase)
        {
            if (!phaseCompleted.ContainsKey(phase))
            {
                phaseCompleted[phase] = true;
                OnPhaseCompleted?.Invoke(phase);
                Debug.Log($"[LevelFlowManager] Phase completed: {phase}");
            }
        }

        // ============================================
        // TRANSITION RULES
        // ============================================

        private void InitializeDefaultRules()
        {
            // Introduction -> EarlyInvestigation: Automatic after 30 seconds
            transitionRules.Add(new PhaseTransitionRule
            {
                fromPhase = StoryPhase.Introduction,
                toPhase = StoryPhase.EarlyInvestigation,
                condition = () => Time.timeSinceLevelLoad > 30f
            });

            // EarlyInvestigation -> FirstAnomaly: 1+ puzzle AND horror > 30
            transitionRules.Add(new PhaseTransitionRule
            {
                fromPhase = StoryPhase.EarlyInvestigation,
                toPhase = StoryPhase.FirstAnomaly,
                condition = () =>
                    PuzzleManager.Instance != null &&
                    HorrorManager.Instance != null &&
                    PuzzleManager.Instance.GetCompletedCount() >= 1 &&
                    HorrorManager.Instance.GetHorrorLevel() > 30f
            });

            // FirstAnomaly -> DeepInvestigation: 3+ evidence AND 2+ puzzles
            transitionRules.Add(new PhaseTransitionRule
            {
                fromPhase = StoryPhase.FirstAnomaly,
                toPhase = StoryPhase.DeepInvestigation,
                condition = () =>
                    EvidenceManager.Instance != null &&
                    PuzzleManager.Instance != null &&
                    EvidenceManager.Instance.TotalCollected >= 3 &&
                    PuzzleManager.Instance.GetCompletedCount() >= 2
            });

            // DeepInvestigation -> RealityBreakdown: horror > 60
            transitionRules.Add(new PhaseTransitionRule
            {
                fromPhase = StoryPhase.DeepInvestigation,
                toPhase = StoryPhase.RealityBreakdown,
                condition = () =>
                    HorrorManager.Instance != null &&
                    HorrorManager.Instance.GetHorrorLevel() > 60f
            });

            // RealityBreakdown -> FinalPreparation: 5+ documents AND 4+ evidence
            transitionRules.Add(new PhaseTransitionRule
            {
                fromPhase = StoryPhase.RealityBreakdown,
                toPhase = StoryPhase.FinalPreparation,
                condition = () =>
                    DocumentManager.Instance != null &&
                    EvidenceManager.Instance != null &&
                    DocumentManager.Instance.TotalCollected >= 5 &&
                    EvidenceManager.Instance.TotalCollected >= 4
            });

            // FinalPreparation -> FinalChase: horror > 80
            transitionRules.Add(new PhaseTransitionRule
            {
                fromPhase = StoryPhase.FinalPreparation,
                toPhase = StoryPhase.FinalChase,
                condition = () =>
                    HorrorManager.Instance != null &&
                    HorrorManager.Instance.GetHorrorLevel() > 80f
            });
        }

        private void CheckTransitionRules()
        {
            foreach (var rule in transitionRules)
            {
                if (rule.fromPhase == currentPhase && rule.condition != null && rule.condition())
                {
                    SetStoryPhase(rule.toPhase);
                    break; // Only one transition per frame
                }
            }
        }

        // ============================================
        // WORLD STATE INTEGRATION
        // ============================================

        private void ApplyPhaseWorldState(StoryPhase phase)
        {
            WorldStateManager worldManager = WorldStateManager.Instance;
            if (worldManager == null) return;

            switch (phase)
            {
                case StoryPhase.Introduction:
                case StoryPhase.EarlyInvestigation:
                    worldManager.SetWorldState(WorldState.Normal);
                    break;

                case StoryPhase.FirstAnomaly:
                    worldManager.SetWorldState(WorldState.Suspicious);
                    break;

                case StoryPhase.DeepInvestigation:
                    worldManager.SetWorldState(WorldState.Corrupted);
                    break;

                case StoryPhase.RealityBreakdown:
                case StoryPhase.FinalPreparation:
                case StoryPhase.FinalChase:
                case StoryPhase.Ending:
                    worldManager.SetWorldState(WorldState.BrokenReality);
                    break;
            }
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
                HorrorManager.Instance.OnHorrorStageChanged += OnHorrorStageChanged;
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
                HorrorManager.Instance.OnHorrorStageChanged -= OnHorrorStageChanged;
            }
        }

        private void OnPuzzleCompleted(string puzzleID)
        {
            Debug.Log($"[LevelFlowManager] Puzzle completed: {puzzleID}");
        }

        private void OnDocumentCollected(DocumentData document)
        {
            Debug.Log($"[LevelFlowManager] Document collected: {document.title}");
        }

        private void OnEvidenceCollected(EvidenceData evidence)
        {
            Debug.Log($"[LevelFlowManager] Evidence collected: {evidence.title}");
        }

        private void OnHorrorStageChanged(HorrorStage stage)
        {
            Debug.Log($"[LevelFlowManager] Horror stage changed: {stage}");
        }

        // ============================================
        // PHASE TRACKING
        // ============================================

        private void InitializePhaseTracking()
        {
            foreach (StoryPhase phase in Enum.GetValues(typeof(StoryPhase)))
            {
                phaseCompleted[phase] = false;
            }
        }

        // ============================================
        // SAVE/LOAD
        // ============================================

        public LevelFlowSaveData GetSaveData()
        {
            return new LevelFlowSaveData
            {
                currentPhase = currentPhase.ToString(),
                completedPhases = new List<string>()
            };
        }

        public void LoadSaveData(LevelFlowSaveData data)
        {
            if (data == null) return;

            if (Enum.TryParse(data.currentPhase, out StoryPhase phase))
            {
                SetStoryPhase(phase);
            }

            if (data.completedPhases != null)
            {
                foreach (string phaseName in data.completedPhases)
                {
                    if (Enum.TryParse(phaseName, out StoryPhase p))
                    {
                        phaseCompleted[p] = true;
                    }
                }
            }

            Debug.Log($"[LevelFlowManager] Loaded: Phase={currentPhase}");
        }

        // ============================================
        // DEBUG
        // ============================================

        public void DebugAdvancePhase()
        {
            AdvancePhase();
        }

        public void DebugPreviousPhase()
        {
            GoToPreviousPhase();
        }
    }

    [Serializable]
    public class PhaseTransitionRule
    {
        public StoryPhase fromPhase;
        public StoryPhase toPhase;
        public Func<bool> condition;
    }

    [Serializable]
    public class LevelFlowSaveData
    {
        public string currentPhase;
        public List<string> completedPhases;
    }
}
