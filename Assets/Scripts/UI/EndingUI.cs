using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using EscapeCampus.Core.Ending;

namespace EscapeCampus.UI
{
    public class EndingUI : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject endingPanel;
        [SerializeField] private GameObject decisionPanel;
        [SerializeField] private GameObject narrativePanel;

        [Header("Decision UI")]
        [SerializeField] private Text decisionTitle;
        [SerializeField] private Text decisionDescription;
        [SerializeField] private Button destroyRitualButton;
        [SerializeField] private Button continueLoopButton;
        [SerializeField] private Button escapeButton;

        [Header("Ending Display")]
        [SerializeField] private Text endingTitle;
        [SerializeField] private Text endingDescription;
        [SerializeField] private Text narrativeText;

        [Header("Truth Summary")]
        [SerializeField] private Text truthSummaryText;

        private void Start()
        {
            if (EndingManager.Instance != null)
            {
                EndingManager.Instance.OnEndingPhaseChanged += OnEndingPhaseChanged;
                EndingManager.Instance.OnEndingTriggered += OnEndingTriggered;
            }

            HideAllPanels();
        }

        private void OnDestroy()
        {
            if (EndingManager.Instance != null)
            {
                EndingManager.Instance.OnEndingPhaseChanged -= OnEndingPhaseChanged;
                EndingManager.Instance.OnEndingTriggered -= OnEndingTriggered;
            }
        }

        // ============================================
        // EVENT HANDLERS
        // ============================================

        private void OnEndingPhaseChanged(EndingPhase phase)
        {
            switch (phase)
            {
                case EndingPhase.FinalDecision:
                    ShowFinalDecision();
                    break;
                case EndingPhase.EndingSequence:
                    ShowEndingSequence();
                    break;
                case EndingPhase.Credits:
                    ShowCredits();
                    break;
            }
        }

        private void OnEndingTriggered(EndingType type)
        {
            ShowEndingResult(type);
        }

        // ============================================
        // FINAL DECISION
        // ============================================

        private void ShowFinalDecision()
        {
            HideAllPanels();

            if (decisionPanel != null)
            {
                decisionPanel.SetActive(true);
            }

            if (decisionTitle != null)
            {
                decisionTitle.text = "THE FINAL CHOICE";
            }

            if (decisionDescription != null)
            {
                decisionDescription.text = "Before you lies the ritual circle. The source of everything.\n\nWhat will you do?";
            }

            // Setup buttons
            if (destroyRitualButton != null)
            {
                destroyRitualButton.onClick.RemoveAllListeners();
                destroyRitualButton.onClick.AddListener(() => MakeDecision(FinalDecision.DestroyRitual));
                SetButtonText(destroyRitualButton, "DESTROY THE RITUAL",
                    EndingTypeExtensions.GetDecisionDescription(FinalDecision.DestroyRitual));
            }

            if (continueLoopButton != null)
            {
                continueLoopButton.onClick.RemoveAllListeners();
                continueLoopButton.onClick.AddListener(() => MakeDecision(FinalDecision.ContinueLoop));
                SetButtonText(continueLoopButton, "CONTINUE THE LOOP",
                    EndingTypeExtensions.GetDecisionDescription(FinalDecision.ContinueLoop));
            }

            if (escapeButton != null)
            {
                escapeButton.onClick.RemoveAllListeners();
                escapeButton.onClick.AddListener(() => MakeDecision(FinalDecision.EscapeWithoutTruth));
                SetButtonText(escapeButton, "ESCAPE WITHOUT TRUTH",
                    EndingTypeExtensions.GetDecisionDescription(FinalDecision.EscapeWithoutTruth));
            }

            // Show truth summary
            ShowTruthSummary();

            // Unlock cursor for UI interaction
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        private void MakeDecision(FinalDecision decision)
        {
            if (EndingManager.Instance != null)
            {
                EndingManager.Instance.MakeFinalDecision(decision);
            }
        }

        private void SetButtonText(Button button, string title, string description)
        {
            Text[] texts = button.GetComponentsInChildren<Text>();
            if (texts.Length >= 2)
            {
                texts[0].text = title;
                texts[1].text = description;
            }
            else if (texts.Length == 1)
            {
                texts[0].text = $"{title}\n<size=14>{description}</size>";
            }
        }

        // ============================================
        // ENDING DISPLAY
        // ============================================

        private void ShowEndingResult(EndingType type)
        {
            HideAllPanels();

            if (endingPanel != null)
            {
                endingPanel.SetActive(true);
            }

            if (endingTitle != null)
            {
                endingTitle.text = EndingTypeExtensions.GetEndingName(type);
            }

            if (endingDescription != null)
            {
                endingDescription.text = EndingTypeExtensions.GetEndingDescription(type);
            }

            // Show narrative
            if (narrativeText != null && TruthRevealManager.Instance != null)
            {
                narrativeText.text = TruthRevealManager.Instance.GetNarrativeForEnding(type);
            }

            // Lock cursor during ending
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void ShowEndingSequence()
        {
            // Transition to ending display
            StartCoroutine(FadeToEnding());
        }

        private IEnumerator FadeToEnding()
        {
            // Brief blackout
            yield return new WaitForSeconds(1f);
            // Ending panel will be shown by OnEndingTriggered
        }

        private void ShowCredits()
        {
            Debug.Log("[EndingUI] Credits would play here.");
        }

        // ============================================
        // TRUTH SUMMARY
        // ============================================

        private void ShowTruthSummary()
        {
            if (truthSummaryText == null) return;
            if (TruthRevealManager.Instance == null) return;

            truthSummaryText.text = TruthRevealManager.Instance.GenerateFinalNarrativeSummary();
        }

        // ============================================
        // HELPERS
        // ============================================

        private void HideAllPanels()
        {
            if (endingPanel != null) endingPanel.SetActive(false);
            if (decisionPanel != null) decisionPanel.SetActive(false);
            if (narrativePanel != null) narrativePanel.SetActive(false);
        }
    }
}
