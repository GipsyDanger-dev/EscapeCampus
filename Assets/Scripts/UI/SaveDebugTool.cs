using UnityEngine;
using UnityEngine.UI;
using EscapeCampus.Documents;
using EscapeCampus.Evidence;
using EscapeCampus.Save;

namespace EscapeCampus.UI
{
    public class SaveDebugTool : MonoBehaviour
    {
        [SerializeField] private KeyCode toggleKey = KeyCode.F1;
        [SerializeField] private GameObject debugPanel;
        [SerializeField] private Text debugText;

        private bool isVisible;
        private float updateInterval = 0.5f;
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
                lastUpdate = UpdateDebugInfo();
            }
        }

        private float UpdateDebugInfo()
        {
            if (debugText == null) return Time.time;

            string info = "<b>=== SAVE DEBUG TOOL ===</b>\n\n";

            // Save file info
            info += "<b>Save Files:</b>\n";
            info += $"  Manual: {(SaveManager.Instance?.HasSave(false) == true ? "EXISTS" : "NONE")}\n";
            info += $"  Autosave: {(SaveManager.Instance?.HasSave(true) == true ? "EXISTS" : "NONE")}\n";
            info += $"  Path: {SaveManager.Instance?.GetSavePath()}\n\n";

            // Play time
            info += "<b>Play Time:</b>\n";
            info += $"  {SaveManager.Instance?.GetPlayTimeFormatted()}\n\n";

            // Document count
            info += "<b>Documents:</b>\n";
            int docCount = DocumentManager.Instance?.TotalCollected ?? 0;
            info += $"  Collected: {docCount}\n";
            if (DocumentManager.Instance != null)
            {
                foreach (var doc in DocumentManager.Instance.CollectedDocuments)
                {
                    string critical = doc.isCritical ? " [CRITICAL]" : "";
                    info += $"    - {doc.title}{critical}\n";
                }
            }
            info += "\n";

            // Evidence count
            info += "<b>Evidence:</b>\n";
            int evCount = EvidenceManager.Instance?.TotalCollected ?? 0;
            info += $"  Collected: {evCount}\n";
            if (EvidenceManager.Instance != null)
            {
                foreach (var ev in EvidenceManager.Instance.CollectedEvidence)
                {
                    string critical = ev.isCritical ? " [CRITICAL]" : "";
                    info += $"    - {ev.title}{critical}\n";
                }
            }
            info += "\n";

            // Player position
            info += "<b>Player:</b>\n";
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                Vector3 pos = player.transform.position;
                info += $"  Position: ({pos.x:F1}, {pos.y:F1}, {pos.z:F1})\n";
            }
            else
            {
                info += "  Position: N/A (no player found)\n";
            }

            // Controls
            info += "\n<b>Controls:</b>\n";
            info += "  F5 = Save | F9 = Load | F1 = Toggle Debug\n";

            debugText.text = info;
            return Time.time;
        }
    }
}
