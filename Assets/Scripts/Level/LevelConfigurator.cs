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
        [Header("Timeout Fallbacks (seconds)")]
        [SerializeField] private float phase1Timeout = 120f;
        [SerializeField] private float phase2Timeout = 600f;
        [SerializeField] private float phase4Timeout = 900f;
        [SerializeField] private float phase5Timeout = 600f;

        private int documentsInteracted;
        private bool phase1Complete;
        private bool phase2Complete;
        private bool phase3Complete;
        private bool phase4Complete;
        private bool phase5Complete;
        private bool phase6Complete;
        private bool phase7Complete;

        private void Start()
        {
            StartCoroutine(InitializeLevel());
        }

        private IEnumerator InitializeLevel()
        {
            yield return new WaitForSeconds(1f);

            SpawnPlayer();
            yield return new WaitForSeconds(0.5f);

            WireDocumentTracking();
            WireEvidenceTracking();
            WirePuzzleTracking();
            WirePhaseTransitions();
            ConfigureInitialHorror();
            ConfigureSafeZone();
            WireEndingSystem();
            ConfigurePacing();

            StartCoroutine(Phase1_Intro());
        }

        // ============================================
        // PLAYER SPAWN
        // ============================================
        private void SpawnPlayer()
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                CharacterController cc = player.GetComponent<CharacterController>();
                if (cc != null) cc.enabled = false;
                player.transform.position = new Vector3(0, 1, 2);
                player.transform.rotation = Quaternion.Euler(0, 180, 0);
                if (cc != null) cc.enabled = true;
            }
        }

        // ============================================
        // WIRING
        // ============================================
        private void WireDocumentTracking()
        {
            if (DocumentManager.Instance != null)
            {
                DocumentManager.Instance.OnDocumentCollected += OnDocumentCollected;
            }
        }

        private void WireEvidenceTracking()
        {
            if (EvidenceManager.Instance != null)
            {
                EvidenceManager.Instance.OnEvidenceCollected += OnEvidenceCollected;
            }
        }

        private void WirePuzzleTracking()
        {
            if (PuzzleManager.Instance != null)
            {
                PuzzleManager.Instance.OnPuzzleCompleted += OnPuzzleCompleted;
            }
        }

        private void WirePhaseTransitions()
        {
            if (LevelFlowManager.Instance != null)
            {
                LevelFlowManager.Instance.OnPhaseEntered += OnPhaseEntered;
            }
        }

        private void ConfigureInitialHorror()
        {
            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.DebugSetLevel(0f);
            }
        }

        private void ConfigureSafeZone()
        {
            // Safe zone is already configured via SafeZone component in LevelLayoutBuilder
        }

        private void WireEndingSystem()
        {
            // Ending is wired via EndingManager events
        }

        private void ConfigurePacing()
        {
            if (ExperienceDirector.Instance != null)
            {
                ExperienceDirector.Instance.TriggerNarrativeBeat(NarrativeBeat.Exploration);
            }
        }

        // ============================================
        // EVENT HANDLERS
        // ============================================
        private void OnDocumentCollected(DocumentData doc)
        {
            documentsInteracted++;
            Debug.Log($"[Level] Document collected: {doc.title} (Total: {documentsInteracted})");
        }

        private void OnEvidenceCollected(EvidenceData ev)
        {
            Debug.Log($"[Level] Evidence collected: {ev.title}");
        }

        private void OnPuzzleCompleted(string puzzleID)
        {
            Debug.Log($"[Level] Puzzle completed: {puzzleID}");
        }

        private void OnPhaseEntered(StoryPhase phase)
        {
            Debug.Log($"[Level] Phase entered: {phase}");
        }

        // ============================================
        // PHASE 1: INTRO (0-5 min)
        // ============================================
        private IEnumerator Phase1_Intro()
        {
            Debug.Log("[Level] === PHASE 1: INTRO ===");

            // Tutorial messages
            yield return new WaitForSeconds(3f);
            Debug.Log("[Level] Tutorial: WASD to move, Mouse to look, E to interact, J for journal");

            // Wait for 2 document interactions OR timeout
            float timer = 0f;
            while (documentsInteracted < 2 && timer < phase1Timeout)
            {
                timer += Time.deltaTime;
                yield return null;
            }

            if (!phase1Complete)
            {
                phase1Complete = true;
                TransitionToPhase(StoryPhase.EarlyInvestigation);
            }
        }

        // ============================================
        // PHASE 2: EARLY INVESTIGATION (5-15 min)
        // ============================================
        private IEnumerator Phase2_EarlyInvestigation()
        {
            Debug.Log("[Level] === PHASE 2: EARLY INVESTIGATION ===");

            // Enable limited horror
            EnableHorrorEvents(false, false, false, false);

            // Set horror level
            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.DebugSetLevel(15f);
            }

            // Wait for puzzle completion OR timeout
            float timer = 0f;
            bool puzzleSolved = false;

            if (PuzzleManager.Instance != null)
            {
                puzzleSolved = PuzzleManager.Instance.GetCompletedCount() >= 1;
            }

            while (!puzzleSolved && timer < phase2Timeout)
            {
                timer += Time.deltaTime;
                if (PuzzleManager.Instance != null)
                {
                    puzzleSolved = PuzzleManager.Instance.GetCompletedCount() >= 1;
                }
                yield return null;
            }

            if (!phase2Complete)
            {
                phase2Complete = true;
                TransitionToPhase(StoryPhase.FirstAnomaly);
            }
        }

        // ============================================
        // PHASE 3: FIRST SETPIECE (15-20 min)
        // ============================================
        private IEnumerator Phase3_FirstSetpiece()
        {
            Debug.Log("[Level] === PHASE 3: FIRST SETPIECE ===");

            // Set horror level
            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.DebugSetLevel(35f);
            }

            // Setpiece will be triggered by trigger volume in corridor
            // Wait for setpiece completion OR timeout
            float timer = 0f;
            bool setpieceDone = false;

            while (!setpieceDone && timer < 300f)
            {
                timer += Time.deltaTime;
                if (SetPieceManager.Instance != null)
                {
                    setpieceDone = SetPieceManager.Instance.IsSetPieceCompleted("SP_LIBRARY_WHISPER_CORRIDOR");
                }
                yield return null;
            }

            if (!phase3Complete)
            {
                phase3Complete = true;
                TransitionToPhase(StoryPhase.DeepInvestigation);
            }
        }

        // ============================================
        // PHASE 4: DEEP INVESTIGATION (20-35 min)
        // ============================================
        private IEnumerator Phase4_DeepInvestigation()
        {
            Debug.Log("[Level] === PHASE 4: DEEP INVESTIGATION ===");

            // Enable text corruption and UI glitch
            EnableHorrorEvents(false, false, true, true);

            // Set horror level
            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.DebugSetLevel(45f);
            }

            // Wait for 3+ evidence AND 2+ puzzles OR timeout
            float timer = 0f;
            bool conditionsMet = false;

            while (!conditionsMet && timer < phase4Timeout)
            {
                timer += Time.deltaTime;

                int evidenceCount = EvidenceManager.Instance?.TotalCollected ?? 0;
                int puzzleCount = PuzzleManager.Instance?.GetCompletedCount() ?? 0;

                conditionsMet = evidenceCount >= 3 && puzzleCount >= 2;

                yield return null;
            }

            if (!phase4Complete)
            {
                phase4Complete = true;
                TransitionToPhase(StoryPhase.RealityBreakdown);
            }
        }

        // ============================================
        // PHASE 5: ESCALATION (35-45 min)
        // ============================================
        private IEnumerator Phase5_Escalation()
        {
            Debug.Log("[Level] === PHASE 5: ESCALATION ===");

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

            // Enable all horror
            EnableHorrorEvents(true, true, true, true);

            // Wait for horror peak OR timeout
            float timer = 0f;
            while (timer < phase5Timeout)
            {
                timer += Time.deltaTime;

                if (HorrorManager.Instance != null)
                {
                    float level = HorrorManager.Instance.GetHorrorLevel();
                    if (level >= 75f) break;
                }

                yield return null;
            }

            if (!phase5Complete)
            {
                phase5Complete = true;
                TransitionToPhase(StoryPhase.FinalPreparation);
            }
        }

        // ============================================
        // PHASE 6: SAFE ZONE (45-50 min)
        // ============================================
        private IEnumerator Phase6_SafeZone()
        {
            Debug.Log("[Level] === PHASE 6: SAFE ZONE ===");

            // Disable all horror
            EnableHorrorEvents(false, false, false, false);

            // Reduce horror level
            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.DebugSetLevel(30f);
            }

            // Reveal truth fragments
            if (TruthRevealManager.Instance != null)
            {
                TruthRevealManager.Instance.EvaluateTruthFragments();
            }

            // Unlock graduation door
            UnlockGraduationDoor();

            // Wait for player to enter graduation hall
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                while (player.transform.position.z > -80f)
                {
                    yield return null;
                }
            }
            else
            {
                yield return new WaitForSeconds(300f);
            }

            if (!phase6Complete)
            {
                phase6Complete = true;
                TransitionToPhase(StoryPhase.FinalChase);
            }
        }

        // ============================================
        // PHASE 7: FINAL SEQUENCE (50-60 min)
        // ============================================
        private IEnumerator Phase7_FinalSequence()
        {
            Debug.Log("[Level] === PHASE 7: FINAL SEQUENCE ===");

            // Lock world state
            if (WorldStateManager.Instance != null)
            {
                WorldStateManager.Instance.SetWorldState(WorldState.BrokenReality);
            }

            // Lock horror level
            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.DebugSetLevel(85f);
            }

            // S14 always active
            if (Semester14Observer.Instance != null)
            {
                Semester14Observer.Instance.DebugForceSpawn();
            }

            // Lock pacing to horror
            if (ExperienceDirector.Instance != null)
            {
                ExperienceDirector.Instance.TriggerNarrativeBeat(NarrativeBeat.Horror);
            }

            // Enable all horror
            EnableHorrorEvents(true, true, true, true);

            // Wait for player to reach ritual area
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                while (Vector3.Distance(player.transform.position, new Vector3(0, 1, -90)) > 5f)
                {
                    yield return null;
                }
            }
            else
            {
                yield return new WaitForSeconds(300f);
            }

            // Trigger ending
            TriggerEnding();
        }

        // ============================================
        // PHASE TRANSITIONS
        // ============================================
        private void TransitionToPhase(StoryPhase phase)
        {
            if (LevelFlowManager.Instance != null)
            {
                LevelFlowManager.Instance.SetStoryPhase(phase);
            }

            // Start corresponding coroutine
            switch (phase)
            {
                case StoryPhase.EarlyInvestigation:
                    StartCoroutine(Phase2_EarlyInvestigation());
                    break;
                case StoryPhase.FirstAnomaly:
                    StartCoroutine(Phase3_FirstSetpiece());
                    break;
                case StoryPhase.DeepInvestigation:
                    StartCoroutine(Phase4_DeepInvestigation());
                    break;
                case StoryPhase.RealityBreakdown:
                    StartCoroutine(Phase5_Escalation());
                    break;
                case StoryPhase.FinalPreparation:
                    StartCoroutine(Phase6_SafeZone());
                    break;
                case StoryPhase.FinalChase:
                    StartCoroutine(Phase7_FinalSequence());
                    break;
            }
        }

        // ============================================
        // HELPERS
        // ============================================
        private void EnableHorrorEvents(bool flicker, bool whisper, bool textShift, bool uiGlitch)
        {
            // Find and enable/disable horror events by type
            HorrorEvent[] events = FindObjectsOfType<HorrorEvent>();
            foreach (HorrorEvent evt in events)
            {
                if (evt is EscapeCampus.Horror.Events.LightFlickerEvent)
                    evt.SetEnabled(flicker);
                else if (evt is EscapeCampus.Horror.Events.WhisperAudioEvent)
                    evt.SetEnabled(whisper);
                else if (evt is EscapeCampus.Horror.Events.DocumentTextShiftEvent)
                    evt.SetEnabled(textShift);
                else if (evt is EscapeCampus.Horror.Events.UIGlitchEvent)
                    evt.SetEnabled(uiGlitch);
            }
        }

        private void UnlockGraduationDoor()
        {
            DoorController[] doors = FindObjectsOfType<DoorController>();
            foreach (DoorController door in doors)
            {
                if (door.gameObject.name.Contains("Graduation"))
                {
                    door.SetLocked(false);
                    break;
                }
            }
        }

        private void TriggerEnding()
        {
            if (EndingManager.Instance == null) return;

            // Check for true ending
            EndingType possibleEnding = EndingManager.Instance.EvaluateEndingCondition();

            if (possibleEnding == EndingType.TrueEnding)
            {
                EndingManager.Instance.TriggerEnding(EndingType.TrueEnding);
            }
            else
            {
                // Show decision UI
                EndingManager.Instance.SetEndingPhase(EndingPhase.FinalDecision);
            }
        }

        // ============================================
        // CLEANUP
        // ============================================
        private void OnDestroy()
        {
            if (DocumentManager.Instance != null)
                DocumentManager.Instance.OnDocumentCollected -= OnDocumentCollected;
            if (EvidenceManager.Instance != null)
                EvidenceManager.Instance.OnEvidenceCollected -= OnEvidenceCollected;
            if (PuzzleManager.Instance != null)
                PuzzleManager.Instance.OnPuzzleCompleted -= OnPuzzleCompleted;
            if (LevelFlowManager.Instance != null)
                LevelFlowManager.Instance.OnPhaseEntered -= OnPhaseEntered;
        }
    }
}
