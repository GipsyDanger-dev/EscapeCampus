using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using EscapeCampus.Documents;
using EscapeCampus.Evidence;
using EscapeCampus.Horror;
using EscapeCampus.Horror.Semester14;
using EscapeCampus.Puzzle;

namespace EscapeCampus.Core.Ending
{
    public class TruthRevealManager : MonoBehaviour
    {
        public static TruthRevealManager Instance { get; private set; }

        [Header("Truth Fragments")]
        [SerializeField] private List<TruthFragment> truthFragments = new List<TruthFragment>();

        private Dictionary<string, bool> revealedFragments = new Dictionary<string, bool>();

        public event Action<string> OnTruthFragmentRevealed;
        public event Action<string> OnFullTruthRevealed;

        public int TotalFragments => truthFragments.Count;
        public int RevealedCount
        {
            get
            {
                int count = 0;
                foreach (var kvp in revealedFragments)
                {
                    if (kvp.Value) count++;
                }
                return count;
            }
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeTruthFragments();
        }

        // ============================================
        // PUBLIC API
        // ============================================

        public string GenerateFinalNarrativeSummary()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("=== THE TRUTH OF CAMPUS 14 ===");
            sb.AppendLine();

            // Gather data
            EndingEvaluationData data = GatherData();

            // Document findings
            sb.AppendLine("<b>DOCUMENTS COLLECTED:</b>");
            sb.AppendLine($"  Total: {data.totalDocuments}");
            sb.AppendLine($"  Critical: {data.criticalDocuments}");
            sb.AppendLine();

            // Evidence findings
            sb.AppendLine("<b>EVIDENCE GATHERED:</b>");
            sb.AppendLine($"  Total: {data.totalEvidence}");
            sb.AppendLine($"  Critical: {data.criticalEvidence}");
            sb.AppendLine();

            // Puzzle results
            sb.AppendLine("<b>MYSTERIES SOLVED:</b>");
            sb.AppendLine($"  Puzzles: {data.puzzlesSolved}");
            sb.AppendLine();

            // Horror experience
            sb.AppendLine("<b>WHAT YOU EXPERIENCED:</b>");
            sb.AppendLine($"  Peak Horror Level: {data.horrorLevelPeak:F0}");
            sb.AppendLine($"  Times Observed: {data.observations}");
            sb.AppendLine();

            // Truth fragments
            sb.AppendLine("<b>TRUTH REVEALED:</b>");
            foreach (var fragment in truthFragments)
            {
                if (revealedFragments.ContainsKey(fragment.fragmentID) && revealedFragments[fragment.fragmentID])
                {
                    sb.AppendLine($"  ✓ {fragment.title}");
                    sb.AppendLine($"    {fragment.revealText}");
                }
                else
                {
                    sb.AppendLine($"  ✗ {fragment.title} [NOT REVEALED]");
                }
            }
            sb.AppendLine();

            // Final narrative
            sb.AppendLine("<b>THE STORY:</b>");
            sb.AppendLine(GetNarrativeForEnding(EvaluateBestEnding()));

            return sb.ToString();
        }

        public string GetNarrativeForEnding(EndingType type)
        {
            switch (type)
            {
                case EndingType.GoodEnding:
                    return @"You found the truth buried beneath Campus 14. The ritual that binds this place,
the loop that traps students like Raka — you ended it. The campus shudders,
walls cracking, as decades of suppressed reality break free.

Semester 14 was never your enemy. It was the campus itself, trying to show you
what had been hidden. Every flicker, every whisper, every impossible sighting
was a message: 'See the truth.'

You saw it. And you acted.

The loop ends. Raka is free. But the campus... the campus remembers.";

                case EndingType.BadEnding:
                    return @"You escaped. The gate opened, and you ran. Behind you, the campus sealed itself
shut, as if nothing had happened. As if you were never there.

But you were. And the evidence you gathered will haunt you. The documents
you read, the anomalies you witnessed — they'll follow you.

Because the loop didn't end. It just... paused. Waiting for the next student.
The next investigator. The next victim.

Somewhere on Campus 14, Raka wakes up again. And the cycle begins anew.";

                case EndingType.SecretEnding:
                    return @"You found the third path. Not destruction, not escape — absorption.

The system that runs Campus 14 isn't evil. It's a machine. A process.
And processes can be... modified.

You didn't destroy the ritual. You didn't escape the loop. You rewrote it.

Semester 14 acknowledges you now. Not as prey. Not as observer.
As architect.

The campus is yours. The loop is yours. And the next student who arrives...
well, you'll be watching.";

                case EndingType.TrueEnding:
                    return @"The full truth, at last.

Semester 14 was Raka. Has always been Raka. The entity you feared,
the presence that watched from the shadows — it was a student,
trapped between loops, trying desperately to communicate.

Every anomaly was a message. Every environmental shift was a plea.
The 'monster' was a victim, reaching across the boundaries of reality
to say: 'Help me.'

And you listened. You gathered every document, solved every mystery,
faced every fear. Not to escape. Not to destroy. But to understand.

The ritual doesn't need to be destroyed. It needs to be completed.
With Raka on the other side.

The loop ends. Not with violence. Not with escape.
With understanding.

Raka is free. And so are you.";

                default:
                    return "The story remains untold...";
            }
        }

        public void RevealFragment(string fragmentID)
        {
            if (revealedFragments.ContainsKey(fragmentID) && revealedFragments[fragmentID])
            {
                return; // Already revealed
            }

            revealedFragments[fragmentID] = true;
            OnTruthFragmentRevealed?.Invoke(fragmentID);

            Debug.Log($"[TruthReveal] Fragment revealed: {fragmentID}");

            // Check if all fragments revealed
            if (RevealedCount >= TotalFragments)
            {
                OnFullTruthRevealed?.Invoke("FULL_TRUTH");
                Debug.Log("[TruthReveal] All fragments revealed — FULL TRUTH available.");
            }
        }

        public bool IsFragmentRevealed(string fragmentID)
        {
            return revealedFragments.ContainsKey(fragmentID) && revealedFragments[fragmentID];
        }

        // ============================================
        // TRUTH FRAGMENT EVALUATION
        // ============================================

        private void InitializeTruthFragments()
        {
            truthFragments = new List<TruthFragment>
            {
                new TruthFragment
                {
                    fragmentID = "TRUTH_CAMPUS_HISTORY",
                    title = "The Campus History",
                    revealText = "Campus 14 was built on a site of a former research facility.",
                    condition = (data) => data.totalDocuments >= 2
                },
                new TruthFragment
                {
                    fragmentID = "TRUTH_RITUAL_EXISTS",
                    title = "The Ritual",
                    revealText = "A ritual was performed decades ago. It never ended.",
                    condition = (data) => data.criticalDocuments >= 1
                },
                new TruthFragment
                {
                    fragmentID = "TRUTH_LOOP_MECHANISM",
                    title = "The Loop",
                    revealText = "The campus exists in a temporal loop. Students repeat the same semester.",
                    condition = (data) => data.puzzlesSolved >= 2
                },
                new TruthFragment
                {
                    fragmentID = "TRUTH_SEMESTER14_IDENTITY",
                    title = "Semester 14's Identity",
                    revealText = "Semester 14 is Raka — a student trapped between loops.",
                    condition = (data) => data.observations >= 5 && data.totalEvidence >= 3
                },
                new TruthFragment
                {
                    fragmentID = "TRUTH_RAKA_ORIGIN",
                    title = "Raka's Story",
                    revealText = "Raka was the first to discover the ritual. The loop consumed him.",
                    condition = (data) => data.totalDocuments >= 4 && data.totalEvidence >= 4
                },
                new TruthFragment
                {
                    fragmentID = "TRUTH_THE_CHOICE",
                    title = "The Final Choice",
                    revealText = "The ritual can be destroyed, continued, or... completed.",
                    condition = (data) => data.currentPhase >= StoryPhase.FinalPreparation
                }
            };

            foreach (var fragment in truthFragments)
            {
                revealedFragments[fragment.fragmentID] = false;
            }
        }

        public void EvaluateTruthFragments()
        {
            EndingEvaluationData data = GatherData();

            foreach (var fragment in truthFragments)
            {
                if (!revealedFragments[fragment.fragmentID] && fragment.condition(data))
                {
                    RevealFragment(fragment.fragmentID);
                }
            }
        }

        // ============================================
        // HELPERS
        // ============================================

        private EndingType EvaluateBestEnding()
        {
            if (EndingManager.Instance != null)
            {
                return EndingManager.Instance.EvaluateEndingCondition();
            }
            return EndingType.BadEnding;
        }

        private EndingEvaluationData GatherData()
        {
            EndingEvaluationData data = new EndingEvaluationData();

            if (DocumentManager.Instance != null)
            {
                data.totalDocuments = DocumentManager.Instance.TotalCollected;
                data.criticalDocuments = DocumentManager.Instance.GetCriticalDocuments().Count;
            }

            if (EvidenceManager.Instance != null)
            {
                data.totalEvidence = EvidenceManager.Instance.TotalCollected;
                data.criticalEvidence = EvidenceManager.Instance.GetCriticalEvidence().Count;
            }

            if (PuzzleManager.Instance != null)
            {
                data.puzzlesSolved = PuzzleManager.Instance.GetCompletedCount();
            }

            if (HorrorManager.Instance != null)
            {
                data.horrorLevelPeak = HorrorManager.Instance.GetHorrorLevel();
            }

            if (Semester14Observer.Instance != null)
            {
                data.observations = Semester14Observer.Instance.TotalObservations;
            }

            if (LevelFlowManager.Instance != null)
            {
                data.currentPhase = LevelFlowManager.Instance.CurrentPhase;
            }

            return data;
        }

        // ============================================
        // SAVE/LOAD
        // ============================================

        public TruthRevealSaveData GetSaveData()
        {
            List<string> revealed = new List<string>();
            foreach (var kvp in revealedFragments)
            {
                if (kvp.Value) revealed.Add(kvp.Key);
            }

            return new TruthRevealSaveData
            {
                revealedFragmentIDs = revealed
            };
        }

        public void LoadSaveData(TruthRevealSaveData data)
        {
            if (data == null || data.revealedFragmentIDs == null) return;

            foreach (string id in data.revealedFragmentIDs)
            {
                if (revealedFragments.ContainsKey(id))
                {
                    revealedFragments[id] = true;
                }
            }

            Debug.Log($"[TruthReveal] Loaded {data.revealedFragmentIDs.Count} revealed fragments.");
        }
    }

    [Serializable]
    public class TruthFragment
    {
        public string fragmentID;
        public string title;
        public string revealText;
        public Func<EndingEvaluationData, bool> condition;
    }

    [Serializable]
    public class TruthRevealSaveData
    {
        public List<string> revealedFragmentIDs;
    }
}
