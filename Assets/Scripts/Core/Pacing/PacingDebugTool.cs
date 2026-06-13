using UnityEngine;
using UnityEngine.UI;

namespace EscapeCampus.Core.Pacing
{
    public class PacingDebugTool : MonoBehaviour
    {
        [SerializeField] private KeyCode toggleKey = KeyCode.F12;

        [Header("UI")]
        [SerializeField] private GameObject debugPanel;
        [SerializeField] private Text debugText;

        private bool isVisible;
        private float updateInterval = 0.2f;
        private float lastUpdate;

        private void Awake()
        {
            if (debugPanel != null)
            {
                debugPanel.SetActive(false);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                isVisible = !isVisible;
                if (debugPanel != null)
                {
                    debugPanel.SetActive(isVisible);
                }
            }

            if (isVisible && Time.time - lastUpdate > updateInterval)
            {
                UpdateDebugDisplay();
                lastUpdate = Time.time;
            }
        }

        private void UpdateDebugDisplay()
        {
            if (debugText == null || ExperienceDirector.Instance == null) return;

            var director = ExperienceDirector.Instance;

            string info = "<b>=== EXPERIENCE DIRECTOR ===</b>\n\n";

            // Tension
            info += $"<b>Tension:</b> {director.TensionLevel:F1}/100\n";
            info += $"<b>Tension State:</b> {TensionLevelExtensions.GetStateName(director.CurrentTensionState)}\n";

            // Tension bar
            info += GetTensionBar(director.TensionLevel);
            info += "\n\n";

            // Beat
            info += $"<b>Current Beat:</b> {TensionLevelExtensions.GetBeatName(director.CurrentBeat)}\n";
            info += $"<b>Next Expected:</b> {TensionLevelExtensions.GetBeatName(director.NextExpectedBeat)}\n\n";

            // Safe zone
            info += $"<b>Safe Zone:</b> {(director.IsInSafeZone ? "YES" : "NO")}\n\n";

            // Horror state
            if (Horror.HorrorManager.Instance != null)
            {
                info += $"<b>Horror Level:</b> {Horror.HorrorManager.Instance.GetHorrorLevel():F1}\n";
                info += $"<b>Horror Stage:</b> {Horror.HorrorManager.Instance.GetHorrorStage()}\n\n";
            }

            // Story phase
            if (LevelFlowManager.Instance != null)
            {
                info += $"<b>Story Phase:</b> {LevelFlowManager.Instance.CurrentPhase}\n\n";
            }

            // Controls
            info += "<b>Controls:</b>\n";
            info += "  F12 = Toggle This Panel\n";

            debugText.text = info;
        }

        private string GetTensionBar(float level)
        {
            int barLength = 20;
            int filled = Mathf.RoundToInt(level / 100f * barLength);

            string bar = "[";
            for (int i = 0; i < barLength; i++)
            {
                if (i < filled)
                {
                    if (level > 80f) bar += "█"; // Red zone
                    else if (level > 60f) bar += "▓"; // Fear zone
                    else if (level > 40f) bar += "▒"; // Unease zone
                    else if (level > 20f) bar += "░"; // Suspense zone
                    else bar += "·"; // Calm zone
                }
                else
                {
                    bar += " ";
                }
            }
            bar += "]";

            return bar;
        }
    }
}
