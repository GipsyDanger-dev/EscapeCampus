using UnityEngine;
using System.Collections;
using EscapeCampus.Core;
using EscapeCampus.Core.Ending;
using EscapeCampus.Core.Pacing;
using EscapeCampus.Documents;
using EscapeCampus.Evidence;
using EscapeCampus.Horror;
using EscapeCampus.Horror.Semester14;
using EscapeCampus.Horror.SetPieces;
using EscapeCampus.Puzzle;
using EscapeCampus.UI;

namespace EscapeCampus.Level
{
    public class LevelConfigurator : MonoBehaviour
    {
        [Header("Document References")]
        [SerializeField] private DocumentData doc001;
        [SerializeField] private DocumentData doc002;
        [SerializeField] private DocumentData doc003;
        [SerializeField] private DocumentData doc004;

        [Header("Evidence References")]
        [SerializeField] private EvidenceData ev001;
        [SerializeField] private EvidenceData ev002;
        [SerializeField] private EvidenceData ev003;

        [Header("Horror Events")]
        [SerializeField] private HorrorEvent lightFlickerEvent;
        [SerializeField] private HorrorEvent whisperEvent;
        [SerializeField] private HorrorEvent textShiftEvent;
        [SerializeField] private HorrorEvent uiGlitchEvent;

        [Header("SetPieces")]
        [SerializeField] private SetPieceBase whisperCorridorSetPiece;

        private bool tutorialComplete;
        private bool phase2Started;
        private bool phase3Started;
        private bool phase4Started;
        private bool phase5Started;
        private bool phase6Started;
        private bool phase7Started;

        private void Start()
        {
            StartCoroutine(InitializeLevel());
        }

        private IEnumerator InitializeLevel()
        {
            yield return new WaitForSeconds(0.5f);

            SpawnPlayer();
            PlaceDocuments();
            PlaceEvidence();
            ConfigurePuzzles();
            ConfigureHorrorEvents();
            ConfigureSafeZones();
            WirePhaseTransitions();
            WireEndingSystem();
            ConfigureExperienceDirector();

            StartCoroutine(TutorialSequence());
        }

        // ============================================
        // PLAYER SPAWN
        // ============================================
        private void SpawnPlayer()
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = new Vector3(0, 1, 2);
                player.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        // ============================================
        // DOCUMENT PLACEMENT
        // ============================================
        private void PlaceDocuments()
        {
            // DOC_002 - Lobby Entrance (tutorial area)
            PlaceDocumentPickup(new Vector3(-3, 1, 1), doc002);

            // DOC_003 - Main Library (reading table)
            PlaceDocumentPickup(new Vector3(0, 1, -12), doc003);

            // DOC_004 - Main Library (librarian desk)
            PlaceDocumentPickup(new Vector3(8, 1, -8), doc004);

            // DOC_001 - Archive Room (research desk, critical)
            PlaceDocumentPickup(new Vector3(0, 1, -56), doc001);
        }

        private void PlaceDocumentPickup(Vector3 pos, DocumentData data)
        {
            if (data == null) return;

            GameObject pickup = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pickup.name = $"DocPickup_{data.documentID}";
            pickup.transform.position = pos;
            pickup.transform.localScale = new Vector3(0.3f, 0.4f, 0.2f);

            Renderer r = pickup.GetComponent<Renderer>();
            if (r != null)
            {
                Material mat = new Material(Shader.Find("Standard"));
                mat.color = new Color(1f, 0.9f, 0.3f); // Yellow for documents
                r.material = mat;
            }

            DocumentPickup dp = pickup.AddComponent<DocumentPickup>();
            dp.SetDocumentData(data);
        }

        // ============================================
        // EVIDENCE PLACEMENT
        // ============================================
        private void PlaceEvidence()
        {
            // EV_003 - Lobby Entrance
            PlaceEvidencePickup(new Vector3(3, 1, 1), ev003);

            // EV_001 - Main Library (near shelves)
            PlaceEvidencePickup(new Vector3(-7, 1, -14), ev001);

            // EV_002 - Archive Room (filing cabinet)
            PlaceEvidencePickup(new Vector3(-6, 1, -50), ev002);
        }

        private void PlaceEvidencePickup(Vector3 pos, EvidenceData data)
        {
            if (data == null) return;

            GameObject pickup = GameObject.CreatePrimitive(PrimitiveType.Cube);
            pickup.name = $"EvPickup_{data.evidenceID}";
            pickup.transform.position = pos;
            pickup.transform.localScale = new Vector3(0.3f, 0.4f, 0.2f);

            Renderer r = pickup.GetComponent<Renderer>();
            if (r != null)
            {
                Material mat = new Material(Shader.Find("Standard"));
                mat.color = new Color(0.3f, 1f, 0.5f); // Green for evidence
                r.material = mat;
            }

            EvidencePickup ep = pickup.AddComponent<EvidencePickup>();
            ep.SetEvidenceData(data);
        }

        // ============================================
        // PUZZLE CONFIGURATION
        // ============================================
        private void ConfigurePuzzles()
        {
            // PZ_LIBRARY_TIMELINE - Main Library
            // Already exists in scene from LibraryTimelinePuzzle
            // Configure requirements
            LibraryTimelinePuzzle timelinePuzzle = FindObjectOfType<LibraryTimelinePuzzle>();
            if (timelinePuzzle != null)
            {
                // Puzzle requires DOC_003 and DOC_004
                // Set via serialized fields in editor or configure here
            }

            // PZ_ARCHIVE_CORRELATION - Archive Room
            // Create second puzzle for archive
            CreateArchivePuzzle();
        }

        private void CreateArchivePuzzle()
        {
            // Create puzzle trigger in archive room
            GameObject puzzleTrigger = new GameObject("PuzzleTrigger_Archive");
            puzzleTrigger.transform.position = new Vector3(0, 1, -58);

            BoxCollider col = puzzleTrigger.AddComponent<BoxCollider>();
            col.size = new Vector3(3, 3, 3);
            col.isTrigger = true;

            // Add puzzle component (uses existing framework)
            PuzzleTrigger pt = puzzleTrigger.AddComponent<PuzzleTrigger>();
            // PuzzleBase will be configured via serialized fields
        }

        // ============================================
        // HORROR EVENT CONFIGURATION
        // ============================================
        private void ConfigureHorrorEvents()
        {
            // Disable all horror events initially
            if (lightFlickerEvent != null) lightFlickerEvent.SetEnabled(false);
            if (whisperEvent != null) whisperEvent.SetEnabled(false);
            if (textShiftEvent != null) textShiftEvent.SetEnabled(false);
            if (uiGlitchEvent != null) uiGlitchEvent.SetEnabled(false);

            // Configure HorrorManager initial state
            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.DebugSetLevel(0f);
            }
        }

        // ============================================
        // SAFE ZONE CONFIGURATION
        // ============================================
        private void ConfigureSafeZones()
        {
            // Safe zone is already configured in LevelLayoutBuilder
            // via SafeZone component on trigger
        }

        // ============================================
        // PHASE TRANSITIONS
        // ============================================
        private void WirePhaseTransitions()
        {
            // Subscribe to phase changes
            if (LevelFlowManager.Instance != null)
            {
                LevelFlowManager.Instance.OnPhaseEntered += OnPhaseEntered;
            }
        }

        private void OnPhaseEntered(StoryPhase phase)
        {
            switch (phase)
            {
                case StoryPhase.EarlyInvestigation:
                    OnPhase2Start();
                    break;
                case StoryPhase.FirstAnomaly:
                    OnPhase3Start();
                    break;
                case StoryPhase.DeepInvestigation:
                    OnPhase4Start();
                    break;
                case StoryPhase.RealityBreakdown:
                    OnPhase5Start();
                    break;
                case StoryPhase.FinalPreparation:
                    OnPhase6Start();
                    break;
                case StoryPhase.FinalChase:
                    OnPhase7Start();
                    break;
            }
        }

        // ============================================
        // PHASE 1: TUTORIAL (0-5 min)
        // ============================================
        private IEnumerator TutorialSequence()
        {
            // Show tutorial UI
            yield return new WaitForSeconds(2f);

            // Tutorial messages handled by existing UI system
            Debug.Log("[Level] Tutorial: WASD to move, Mouse to look, E to interact, J for journal");

            // Wait for 2 document interactions OR 90 seconds
            float timer = 0f;
            int docsCollected = 0;

            if (DocumentManager.Instance != null)
            {
                DocumentManager.Instance.OnDocumentCollected += (doc) =>
                {
                    docsCollected++;
                };
            }

            while (docsCollected < 2 && timer < 90f)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            // Transition to Phase 2
            if (LevelFlowManager.Instance != null)
            {
                LevelFlowManager.Instance.SetStoryPhase(StoryPhase.EarlyInvestigation);
            }
        }

        // ============================================
        // PHASE 2: EARLY INVESTIGATION (5-15 min)
        // ============================================
        private void OnPhase2Start()
        {
            if (phase2Started) return;
            phase2Started = true;

            Debug.Log("[Level] Phase 2: Early Investigation");

            // Enable limited horror events
            if (lightFlickerEvent != null) lightFlickerEvent.SetEnabled(true);
            if (whisperEvent != null) whisperEvent.SetEnabled(true);

            // Configure horror rules
            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.DebugSetLevel(15f);
            }

            // Subscribe to puzzle completion for phase transition
            if (PuzzleManager.Instance != null)
            {
                PuzzleManager.Instance.OnPuzzleCompleted += OnPuzzleCompletedPhase2;
            }
        }

        private void OnPuzzleCompletedPhase2(string puzzleID)
        {
            // PZ_LIBRARY_TIMELINE solved → FirstAnomaly
            if (puzzleID == "PZ_LIBRARY_TIMELINE")
            {
                if (LevelFlowManager.Instance != null)
                {
                    LevelFlowManager.Instance.SetStoryPhase(StoryPhase.FirstAnomaly);
                }

                if (PuzzleManager.Instance != null)
                {
                    PuzzleManager.Instance.OnPuzzleCompleted -= OnPuzzleCompletedPhase2;
                }
            }
        }

        // ============================================
        // PHASE 3: FIRST HORROR SETPIECE (15-20 min)
        // ============================================
        private void OnPhase3Start()
        {
            if (phase3Started) return;
            phase3Started = true;

            Debug.Log("[Level] Phase 3: First Horror Setpiece");

            // Setpiece will be triggered by trigger volume in corridor
            // Configure horror level
            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.DebugSetLevel(35f);
            }
        }

        // ============================================
        // PHASE 4: DEEP INVESTIGATION (20-35 min)
        // ============================================
        private void OnPhase4Start()
        {
            if (phase4Started) return;
            phase4Started = true;

            Debug.Log("[Level] Phase 4: Deep Investigation");

            // Enable text shift and UI glitch
            if (textShiftEvent != null) textShiftEvent.SetEnabled(true);
            if (uiGlitchEvent != null) uiGlitchEvent.SetEnabled(true);

            // Enable S14 peripheral observations
            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.DebugSetLevel(45f);
            }

            // Subscribe to evidence collection for phase transition
            if (EvidenceManager.Instance != null)
            {
                EvidenceManager.Instance.OnEvidenceCollected += OnEvidenceCollectedPhase4;
            }

            // Subscribe to puzzle completion
            if (PuzzleManager.Instance != null)
            {
                PuzzleManager.Instance.OnPuzzleCompleted += OnPuzzleCompletedPhase4;
            }
        }

        private void OnEvidenceCollectedPhase4(EvidenceData evidence)
        {
            CheckPhase4Completion();
        }

        private void OnPuzzleCompletedPhase4(string puzzleID)
        {
            CheckPhase4Completion();
        }

        private void CheckPhase4Completion()
        {
            if (EvidenceManager.Instance == null || PuzzleManager.Instance == null) return;

            bool evidenceComplete = EvidenceManager.Instance.TotalCollected >= 3;
            bool puzzleComplete = PuzzleManager.Instance.GetCompletedCount() >= 2;

            if (evidenceComplete && puzzleComplete)
            {
                if (LevelFlowManager.Instance != null)
                {
                    LevelFlowManager.Instance.SetStoryPhase(StoryPhase.RealityBreakdown);
                }

                if (EvidenceManager.Instance != null)
                {
                    EvidenceManager.Instance.OnEvidenceCollected -= OnEvidenceCollectedPhase4;
                }
                if (PuzzleManager.Instance != null)
                {
                    PuzzleManager.Instance.OnPuzzleCompleted -= OnPuzzleCompletedPhase4;
                }
            }
        }

        // ============================================
        // PHASE 5: ESCALATION (35-45 min)
        // ============================================
        private void OnPhase5Start()
        {
            if (phase5Started) return;
            phase5Started = true;

            Debug.Log("[Level] Phase 5: Escalation");

            // World state changes
            if (WorldStateManager.Instance != null)
            {
                WorldStateManager.Instance.SetWorldState(WorldState.BrokenReality);
            }

            // Horror escalation
            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.DebugSetLevel(65f);
            }

            // S14 observation frequency doubled
            if (Semester14Observer.Instance != null)
            {
                // Observer will naturally increase with higher horror level
            }

            // Disable horror cooldowns (but keep caps)
            // Handled by ExperienceDirector

            // After horror level reaches threshold → FinalPreparation
            StartCoroutine(WaitForEscalationPeak());
        }

        private IEnumerator WaitForEscalationPeak()
        {
            while (HorrorManager.Instance != null && HorrorManager.Instance.GetHorrorLevel() < 75f)
            {
                yield return null;
            }

            if (LevelFlowManager.Instance != null)
            {
                LevelFlowManager.Instance.SetStoryPhase(StoryPhase.FinalPreparation);
            }
        }

        // ============================================
        // PHASE 6: SAFE ZONE (45-50 min)
        // ============================================
        private void OnPhase6Start()
        {
            if (phase6Started) return;
            phase6Started = true;

            Debug.Log("[Level] Phase 6: Safe Zone");

            // Disable all horror
            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.DebugSetLevel(30f);
            }

            if (lightFlickerEvent != null) lightFlickerEvent.SetEnabled(false);
            if (whisperEvent != null) whisperEvent.SetEnabled(false);
            if (textShiftEvent != null) textShiftEvent.SetEnabled(false);
            if (uiGlitchEvent != null) uiGlitchEvent.SetEnabled(false);

            // Reveal truth fragments
            if (TruthRevealManager.Instance != null)
            {
                TruthRevealManager.Instance.EvaluateTruthFragments();
            }

            // Unlock Graduation Hall door
            UnlockGraduationDoor();

            // After player enters Graduation Hall → FinalChase
            StartCoroutine(WaitForGraduationEntry());
        }

        private void UnlockGraduationDoor()
        {
            DoorController[] doors = FindObjectsOfType<DoorController>();
            foreach (DoorController door in doors)
            {
                if (door.gameObject.name == "GraduationDoor")
                {
                    door.SetLocked(false);
                    break;
                }
            }
        }

        private IEnumerator WaitForGraduationEntry()
        {
            // Wait for player to reach graduation hall area
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null) yield break;

            while (player.transform.position.z > -80f)
            {
                yield return null;
            }

            if (LevelFlowManager.Instance != null)
            {
                LevelFlowManager.Instance.SetStoryPhase(StoryPhase.FinalChase);
            }
        }

        // ============================================
        // PHASE 7: FINAL SEQUENCE (50-60 min)
        // ============================================
        private void OnPhase7Start()
        {
            if (phase7Started) return;
            phase7Started = true;

            Debug.Log("[Level] Phase 7: Final Sequence");

            // World state locked to BrokenReality
            if (WorldStateManager.Instance != null)
            {
                WorldStateManager.Instance.SetWorldState(WorldState.BrokenReality);
            }

            // Horror level locked 80-100
            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.DebugSetLevel(85f);
            }

            // S14 always active
            if (Semester14Observer.Instance != null)
            {
                Semester14Observer.Instance.DebugForceSpawn();
            }

            // ExperienceDirector locks to Horror beat
            if (ExperienceDirector.Instance != null)
            {
                ExperienceDirector.Instance.TriggerNarrativeBeat(NarrativeBeat.Horror);
            }

            // Enable all horror events at high frequency
            if (lightFlickerEvent != null) lightFlickerEvent.SetEnabled(true);
            if (whisperEvent != null) whisperEvent.SetEnabled(true);
            if (textShiftEvent != null) textShiftEvent.SetEnabled(true);
            if (uiGlitchEvent != null) uiGlitchEvent.SetEnabled(true);

            // Final setpiece: collapse effect
            StartCoroutine(FinalCollapseSequence());
        }

        private IEnumerator FinalCollapseSequence()
        {
            // Wait for player to reach ritual area
            GameObject player = GameObject.FindWithTag("Player");
            if (player == null) yield break;

            while (Vector3.Distance(player.transform.position, new Vector3(0, 1, -90)) > 5f)
            {
                yield return null;
            }

            // Trigger final decision
            if (EndingManager.Instance != null)
            {
                EndingType possibleEnding = EndingManager.Instance.EvaluateEndingCondition();

                if (possibleEnding == EndingType.TrueEnding)
                {
                    // Auto-trigger true ending if conditions met
                    EndingManager.Instance.TriggerEnding(EndingType.TrueEnding);
                }
                else
                {
                    // Show final decision UI
                    EndingManager.Instance.SetEndingPhase(EndingPhase.FinalDecision);
                }
            }
        }

        // ============================================
        // ENDING SYSTEM
        // ============================================
        private void WireEndingSystem()
        {
            if (EndingManager.Instance != null)
            {
                EndingManager.OnEndingTriggered += OnEndingTriggered;
            }
        }

        private void OnEndingTriggered(EndingType type)
        {
            Debug.Log($"[Level] Ending triggered: {type}");

            // Lock all player control
            var playerController = FindObjectOfType<EscapeCampus.Player.FirstPersonController>();
            if (playerController != null) playerController.enabled = false;

            // Show ending UI
            EndingUI endingUI = FindObjectOfType<EndingUI>();
            if (endingUI != null)
            {
                // EndingUI handles display
            }
        }

        // ============================================
        // EXPERIENCE DIRECTOR
        // ============================================
        private void ConfigureExperienceDirector()
        {
            if (ExperienceDirector.Instance != null)
            {
                // Start with calm pacing
                ExperienceDirector.Instance.TriggerNarrativeBeat(NarrativeBeat.Exploration);
            }
        }

        private void OnDestroy()
        {
            if (LevelFlowManager.Instance != null)
            {
                LevelFlowManager.Instance.OnPhaseEntered -= OnPhaseEntered;
            }

            if (EndingManager.Instance != null)
            {
                EndingManager.OnEndingTriggered -= OnEndingTriggered;
            }
        }
    }
}
