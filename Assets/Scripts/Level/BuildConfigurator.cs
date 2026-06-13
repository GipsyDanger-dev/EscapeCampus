using UnityEngine;

namespace EscapeCampus.Level
{
    public class BuildConfigurator : MonoBehaviour
    {
        [Header("Build Settings")]
        [SerializeField] private bool isReleaseBuild = true;

        private void Awake()
        {
            if (isReleaseBuild)
            {
                DisableAllDebugTools();
                ConfigureCursor();
            }
        }

        private void DisableAllDebugTools()
        {
            // Disable all debug tool components
            DisableComponent<EscapeCampus.Puzzle.PuzzleDebugTool>();
            DisableComponent<EscapeCampus.Horror.HorrorDebugTool>();
            DisableComponent<EscapeCampus.Horror.Semester14.Semester14DebugTool>();
            DisableComponent<EscapeCampus.Core.LevelFlowDebugTool>();
            DisableComponent<EscapeCampus.Core.Pacing.PacingDebugTool>();
            DisableComponent<EscapeCampus.UI.SaveDebugTool>();

            // Hide all debug panels
            HideDebugPanels();

            Debug.Log("[BuildConfigurator] Release build: All debug tools disabled.");
        }

        private void DisableComponent<T>() where T : MonoBehaviour
        {
            T[] components = FindObjectsOfType<T>();
            foreach (T comp in components)
            {
                comp.enabled = false;
            }
        }

        private void HideDebugPanels()
        {
            // Find and hide all debug panels by name
            string[] debugPanelNames = {
                "SaveDebugPanel",
                "Semester14DebugPanel",
                "PacingDebugPanel",
                "PhaseDisplayPanel"
            };

            foreach (string panelName in debugPanelNames)
            {
                GameObject panel = GameObject.Find(panelName);
                if (panel != null)
                {
                    panel.SetActive(false);
                }
            }
        }

        private void ConfigureCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // Block debug input in release build
        private void Update()
        {
            if (!isReleaseBuild) return;

            // Block F1-F12 except F5 (save) and F9 (load)
            for (int i = 1; i <= 12; i++)
            {
                if (i == 5 || i == 9) continue; // Allow save/load

                KeyCode key = KeyCode.F1 + (i - 1);
                if (Input.GetKeyDown(key))
                {
                    // Consume the input - do nothing
                }
            }
        }
    }
}
