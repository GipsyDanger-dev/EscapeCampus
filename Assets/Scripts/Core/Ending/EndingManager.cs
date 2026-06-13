using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using EscapeCampus.Documents;
using EscapeCampus.Evidence;
using EscapeCampus.Horror;
using EscapeCampus.Horror.Semester14;
using EscapeCampus.Horror.SetPieces;
using EscapeCampus.Puzzle;

namespace EscapeCampus.Core.Ending
{
    public class EndingManager : MonoBehaviour
    {
        public static EndingManager Instance { get; private set; }

        [Header("Ending Conditions")]
        [SerializeField] private EndingConditions goodEndingConditions;
        [SerializeField] private EndingConditions badEndingConditions;
        [SerializeField] private EndingConditions secretEndingConditions;
        [SerializeField] private EndingConditions trueEndingConditions;

        [Header("State")]
        [SerializeField] private EndingPhase currentPhase = EndingPhase.NotStarted;
        [SerializeField] private EndingType achievedEnding = EndingType.None;
        [SerializeField] private FinalDecision playerDecision = FinalDecision.None;
        [SerializeField] private bool endingTriggered;

        [Header("References")]
        [SerializeField] private Transform graduationHallSpawn;

        // Peak tracking
        private float horrorLevelPeak;
        private int totalObservations;

        // Events
        public event Action<EndingPhase> OnEndingPhaseChanged;
        public event Action<EndingType> OnEndingTriggered;
        public event Action<FinalDecision> OnFinalDecisionMade;

        public EndingPhase CurrentPhase => currentPhase;
        public EndingType AchievedEnding => achievedEnding;
        public FinalDecision PlayerDecision => playerDecision;
        public bool HasEndingTriggered => endingTriggered;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeDefaultConditions();
        }

        private void Start()
        {
            SubscribeToEvents();
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        // ============================================
        // PUBLIC API
        // ============================================

        public EndingType EvaluateEndingCondition()
        {
            EndingEvaluationData data = GatherEvaluationData();

            // Check in priority order: True > Good > Secret > Bad
            if (trueEndingConditions != null && trueEndingConditions.Evaluate(data))
            {
                return EndingType.TrueEnding;
            }

            if (goodEndingConditions != null && goodEndingConditions.Evaluate(data))
            {
                return EndingType.GoodEnding;
            }

            if (secretEndingConditions != null && secretEndingConditions.Evaluate(data))
            {
                return EndingType.SecretEnding;
            }

            if (badEndingConditions != null && badEndingConditions.Evaluate(data))
            {
                return EndingType.BadEnding;
            }

            return EndingType.None;
        }

        public void TriggerEnding(EndingType type)
        {
            if (endingTriggered) return;

            achievedEnding = type;
            endingTriggered = true;

            Debug.Log($"[EndingManager] Ending triggered: {EndingTypeExtensions.GetEndingName(type)}");
            OnEndingTriggered?.Invoke(type);

            StartCoroutine(PlayEndingSequence(type));
        }

        public void MakeFinalDecision(FinalDecision decision)
        {
            if (currentPhase != EndingPhase.FinalDecision) return;

            playerDecision = decision;
            Debug.Log($"[EndingManager] Final decision: {decision}");
            OnFinalDecisionMade?.Invoke(decision);

            // Determine ending based on decision
            EndingType endingType = GetEndingFromDecision(decision);
            TriggerEnding(endingType);
        }

        public void SetEndingPhase(EndingPhase phase)
        {
            if (phase == currentPhase) return;

            currentPhase = phase;
            Debug.Log($"[EndingManager] Phase: {phase}");
            OnEndingPhaseChanged?.Invoke(phase);
        }

        // ============================================
        // ENDING SEQUENCE
        // ============================================

        private IEnumerator PlayEndingSequence(EndingType type)
        {
            // Phase 1: World Breakdown
            SetEndingPhase(EndingPhase.WorldBreakdown);
            WorldStateManager.Instance?.SetWorldState(WorldState.BrokenReality);
            yield return new WaitForSeconds(2f);

            // Phase 2: Constant S14 Presence
            SetEndingPhase(EndingPhase.ConstantPresence);
            if (Semester14Observer.Instance != null)
            {
                Semester14Observer.Instance.DebugForceSpawn();
            }
            yield return new WaitForSeconds(3f);

            // Phase 3: Graduation Hall
            SetEndingPhase(EndingPhase.GraduationHall);
            yield return StartCoroutine(MoveToGraduationHall());
            yield return new WaitForSeconds(2f);

            // Phase 4: Final Decision (if not already decided)
            if (playerDecision == FinalDecision.None)
            {
                SetEndingPhase(EndingPhase.FinalDecision);
                ShowFinalDecisionUI();
            }
            else
            {
                // Already decided, play ending
                SetEndingPhase(EndingPhase.EndingSequence);
                yield return StartCoroutine(PlayEndingCinematic(type));
            }
        }

        private IEnumerator MoveToGraduationHall()
        {
            if (graduationHallSpawn == null) yield break;

            GameObject player = GameObject.FindWithTag("Player");
            if (player == null) yield break;

            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            player.transform.position = graduationHallSpawn.position;
            player.transform.rotation = graduationHallSpawn.rotation;

            if (cc != null) cc.enabled = true;

            yield return null;
        }

        private IEnumerator PlayEndingCinematic(EndingType type)
        {
            // Lock player
            var playerController = FindObjectOfType<Player.FirstPersonController>();
            if (playerController != null) playerController.enabled = false;

            // Show ending text
            yield return StartCoroutine(ShowEndingText(type));

            // Credits
            SetEndingPhase(EndingPhase.Credits);
            yield return StartCoroutine(ShowCredits());

            // Restore (for now, eventually load credits scene)
            if (playerController != null) playerController.enabled = true;
        }

        private IEnumerator ShowEndingText(EndingType type)
        {
            // Ending text display handled by EndingUI
            Debug.Log($"[EndingManager] Playing ending: {EndingTypeExtensions.GetEndingName(type)}");
            Debug.Log($"[EndingManager] {EndingTypeExtensions.GetEndingDescription(type)}");
            yield return new WaitForSeconds(5f);
        }

        private IEnumerator ShowCredits()
        {
            Debug.Log("[EndingManager] Credits rolling...");
            yield return new WaitForSeconds(3f);
        }

        // ============================================
        // FINAL DECISION
        // ============================================

        private void ShowFinalDecisionUI()
        {
            // Handled by EndingUI component
            Debug.Log("[EndingManager] Showing final decision UI...");
        }

        private EndingType GetEndingFromDecision(FinalDecision decision)
        {
            switch (decision)
            {
                case FinalDecision.DestroyRitual:
                    return EndingType.GoodEnding;
                case FinalDecision.ContinueLoop:
                    return EndingType.BadEnding;
                case FinalDecision.EscapeWithoutTruth:
                    return EndingType.SecretEnding;
                default:
                    return EndingType.BadEnding;
            }
        }

        // ============================================
        // DATA GATHERING
        // ============================================

        private EndingEvaluationData GatherEvaluationData()
        {
            EndingEvaluationData data = new EndingEvaluationData();

            // Documents
            if (DocumentManager.Instance != null)
            {
                data.totalDocuments = DocumentManager.Instance.TotalCollected;
                data.criticalDocuments = DocumentManager.Instance.GetCriticalDocuments().Count;
            }

            // Evidence
            if (EvidenceManager.Instance != null)
            {
                data.totalEvidence = EvidenceManager.Instance.TotalCollected;
                data.criticalEvidence = EvidenceManager.Instance.GetCriticalEvidence().Count;
            }

            // Puzzles
            if (PuzzleManager.Instance != null)
            {
                data.puzzlesSolved = PuzzleManager.Instance.GetCompletedCount();
            }

            // Horror
            if (HorrorManager.Instance != null)
            {
                data.horrorLevelPeak = horrorLevelPeak;
            }

            // SetPieces
            if (SetPieceManager.Instance != null)
            {
                // Count completed setpieces
                int completed = 0;
                string[] knownSetPieces = { "SP_LIBRARY_WHISPER_CORRIDOR" };
                foreach (string sp in knownSetPieces)
                {
                    if (SetPieceManager.Instance.IsSetPieceCompleted(sp))
                    {
                        completed++;
                    }
                }
                data.setPiecesCompleted = completed;
            }

            // Observations
            if (Semester14Observer.Instance != null)
            {
                data.observations = Semester14Observer.Instance.TotalObservations;
            }

            // Phase
            if (LevelFlowManager.Instance != null)
            {
                data.currentPhase = LevelFlowManager.Instance.CurrentPhase;
            }

            return data;
        }

        // ============================================
        // DEFAULT CONDITIONS
        // ============================================

        private void InitializeDefaultConditions()
        {
            // Good Ending: Destroy ritual
            goodEndingConditions = new EndingConditions
            {
                minDocumentsCollected = 4,
                minCriticalDocuments = 1,
                minEvidenceCollected = 3,
                minCriticalEvidence = 1,
                minPuzzlesSolved = 2,
                minHorrorLevelPeak = 50f,
                minSetPiecesCompleted = 1,
                minObservations = 3,
                requiredPhase = StoryPhase.FinalPreparation
            };

            // Bad Ending: Escape but loop continues
            badEndingConditions = new EndingConditions
            {
                minDocumentsCollected = 2,
                minCriticalDocuments = 0,
                minEvidenceCollected = 1,
                minCriticalEvidence = 0,
                minPuzzlesSolved = 1,
                minHorrorLevelPeak = 30f,
                minSetPiecesCompleted = 0,
                minObservations = 1,
                requiredPhase = StoryPhase.DeepInvestigation
            };

            // Secret Ending: Join system
            secretEndingConditions = new EndingConditions
            {
                minDocumentsCollected = 5,
                minCriticalDocuments = 1,
                minEvidenceCollected = 4,
                minCriticalEvidence = 1,
                minPuzzlesSolved = 3,
                minHorrorLevelPeak = 40f,
                maxHorrorLevelPeak = 70f,
                minSetPiecesCompleted = 1,
                minObservations = 5,
                requiredPhase = StoryPhase.RealityBreakdown
            };

            // True Ending: Full truth
            trueEndingConditions = new EndingConditions
            {
                minDocumentsCollected = 5,
                minCriticalDocuments = 1,
                minEvidenceCollected = 4,
                minCriticalEvidence = 1,
                minPuzzlesSolved = 3,
                minHorrorLevelPeak = 70f,
                minSetPiecesCompleted = 2,
                minObservations = 7,
                requiredPhase = StoryPhase.FinalPreparation
            };
        }

        // ============================================
        // EVENT TRACKING
        // ============================================

        private void SubscribeToEvents()
        {
            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.OnHorrorLevelChanged += OnHorrorLevelChanged;
            }

            if (Semester14Observer.Instance != null)
            {
                Semester14Observer.Instance.OnObservationStarted += OnObservationStarted;
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
                HorrorManager.Instance.OnHorrorLevelChanged -= OnHorrorLevelChanged;
            }

            if (Semester14Observer.Instance != null)
            {
                Semester14Observer.Instance.OnObservationStarted -= OnObservationStarted;
            }

            if (LevelFlowManager.Instance != null)
            {
                LevelFlowManager.Instance.OnPhaseEntered -= OnPhaseEntered;
            }
        }

        private void OnHorrorLevelChanged(float level)
        {
            if (level > horrorLevelPeak)
            {
                horrorLevelPeak = level;
            }
        }

        private void OnObservationStarted(Horror.Semester14.ObservationType type)
        {
            totalObservations++;
        }

        private void OnPhaseEntered(StoryPhase phase)
        {
            if (phase == StoryPhase.FinalChase && !endingTriggered)
            {
                // Auto-evaluate ending when reaching FinalChase
                EndingType possibleEnding = EvaluateEndingCondition();
                if (possibleEnding != EndingType.None)
                {
                    SetEndingPhase(EndingPhase.FinalSetPiece);
                }
            }
        }

        // ============================================
        // SAVE/LOAD
        // ============================================

        public EndingSaveData GetSaveData()
        {
            return new EndingSaveData
            {
                achievedEnding = achievedEnding.ToString(),
                playerDecision = playerDecision.ToString(),
                endingTriggered = endingTriggered,
                horrorLevelPeak = horrorLevelPeak,
                totalObservations = totalObservations
            };
        }

        public void LoadSaveData(EndingSaveData data)
        {
            if (data == null) return;

            if (Enum.TryParse(data.achievedEnding, out EndingType ending))
            {
                achievedEnding = ending;
            }

            if (Enum.TryParse(data.playerDecision, out FinalDecision decision))
            {
                playerDecision = decision;
            }

            endingTriggered = data.endingTriggered;
            horrorLevelPeak = data.horrorLevelPeak;
            totalObservations = data.totalObservations;

            Debug.Log($"[EndingManager] Loaded: Ending={achievedEnding}, Decision={playerDecision}");
        }
    }

    [Serializable]
    public class EndingSaveData
    {
        public string achievedEnding;
        public string playerDecision;
        public bool endingTriggered;
        public float horrorLevelPeak;
        public int totalObservations;
    }
}
