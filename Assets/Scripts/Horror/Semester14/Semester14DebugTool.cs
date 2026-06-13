using UnityEngine;
using UnityEngine.UI;

namespace EscapeCampus.Horror.Semester14
{
    public class Semester14DebugTool : MonoBehaviour
    {
        [SerializeField] private KeyCode forceSpawnKey = KeyCode.F9;
        [SerializeField] private KeyCode clearAllKey = KeyCode.F10;

        [Header("UI")]
        [SerializeField] private GameObject debugPanel;
        [SerializeField] private Text debugText;

        private float updateInterval = 0.3f;
        private float lastUpdate;

        private void Update()
        {
            if (Input.GetKeyDown(forceSpawnKey))
            {
                ForceSpawn();
            }

            if (Input.GetKeyDown(clearAllKey))
            {
                ClearAll();
            }

            if (debugPanel != null && debugPanel.activeSelf)
            {
                if (Time.time - lastUpdate > updateInterval)
                {
                    UpdateDebugDisplay();
                    lastUpdate = Time.time;
                }
            }

            // Toggle debug panel with F11
            if (Input.GetKeyDown(KeyCode.F11))
            {
                if (debugPanel != null)
                {
                    debugPanel.SetActive(!debugPanel.activeSelf);
                }
            }
        }

        private void ForceSpawn()
        {
            if (Semester14Observer.Instance != null)
            {
                Semester14Observer.Instance.DebugForceSpawn();
            }
        }

        private void ClearAll()
        {
            if (Semester14Observer.Instance != null)
            {
                Semester14Observer.Instance.DebugClearAll();
            }
        }

        private void UpdateDebugDisplay()
        {
            if (debugText == null || Semester14Observer.Instance == null) return;

            var observer = Semester14Observer.Instance;

            string info = "<b>=== SEMESTER 14 OBSERVER ===</b>\n\n";

            info += $"<b>State:</b> {(observer.IsObserving ? "OBSERVING" : "Idle")}\n";

            if (observer.IsObserving)
            {
                info += $"<b>Type:</b> {observer.GetCurrentObservationType()}\n";
                info += $"<b>Distance:</b> {observer.GetDistanceToPlayer():F1}m\n";
            }

            info += $"\n<b>Total Observations:</b> {observer.TotalObservations}\n";
            info += $"<b>Last Trigger:</b> {Time.time - observer.LastObservationTime:F0}s ago\n";

            info += "\n<b>Controls:</b>\n";
            info += "  F9 = Force Spawn\n";
            info += "  F10 = Clear All\n";
            info += "  F11 = Toggle This Panel\n";

            debugText.text = info;
        }
    }
}
