using UnityEngine;
using UnityEngine.UI;

namespace EscapeCampus.Core
{
    public class LevelFlowDebugTool : MonoBehaviour
    {
        [SerializeField] private KeyCode nextPhaseKey = KeyCode.F7;
        [SerializeField] private KeyCode prevPhaseKey = KeyCode.F8;

        [Header("UI References")]
        [SerializeField] private GameObject phaseDisplayPanel;
        [SerializeField] private Text phaseText;

        private void Update()
        {
            if (Input.GetKeyDown(nextPhaseKey))
            {
                NextPhase();
            }

            if (Input.GetKeyDown(prevPhaseKey))
            {
                PreviousPhase();
            }
        }

        private void Start()
        {
            if (LevelFlowManager.Instance != null)
            {
                LevelFlowManager.Instance.OnPhaseChanged += OnPhaseChanged;
                UpdatePhaseDisplay();
            }
        }

        private void OnDestroy()
        {
            if (LevelFlowManager.Instance != null)
            {
                LevelFlowManager.Instance.OnPhaseChanged -= OnPhaseChanged;
            }
        }

        private void NextPhase()
        {
            if (LevelFlowManager.Instance != null)
            {
                LevelFlowManager.Instance.DebugAdvancePhase();
            }
        }

        private void PreviousPhase()
        {
            if (LevelFlowManager.Instance != null)
            {
                LevelFlowManager.Instance.DebugPreviousPhase();
            }
        }

        private void OnPhaseChanged(StoryPhase oldPhase, StoryPhase newPhase)
        {
            UpdatePhaseDisplay();
            Debug.Log($"[LevelFlowDebug] Phase: {oldPhase} -> {newPhase}");
        }

        private void UpdatePhaseDisplay()
        {
            if (phaseText != null && LevelFlowManager.Instance != null)
            {
                StoryPhase phase = LevelFlowManager.Instance.CurrentPhase;
                phaseText.text = $"Phase: {StoryPhaseExtensions.GetPhaseName(phase)}";
            }
        }
    }
}
