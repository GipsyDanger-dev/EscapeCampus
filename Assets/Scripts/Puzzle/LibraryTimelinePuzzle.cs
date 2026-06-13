using UnityEngine;
using System.Collections.Generic;
using EscapeCampus.Documents;
using EscapeCampus.UI;

namespace EscapeCampus.Puzzle
{
    public class LibraryTimelinePuzzle : PuzzleBase
    {
        [Header("Timeline Puzzle")]
        [SerializeField] private DocumentData firstDocument;
        [SerializeField] private DocumentData secondDocument;

        [Header("UI References")]
        [SerializeField] private GameObject puzzleUIPanel;
        [SerializeField] private UnityEngine.UI.Text questionText;
        [SerializeField] private UnityEngine.UI.Text feedbackText;
        [SerializeField] private UnityEngine.UI.Button optionAButton;
        [SerializeField] private UnityEngine.UI.Button optionBButton;

        private bool puzzleActive;
        private int correctOrder; // 0 = first doc happened first, 1 = second doc happened first

        protected override void Start()
        {
            base.Start();

            if (puzzleUIPanel != null)
            {
                puzzleUIPanel.SetActive(false);
            }

            // Set correct answer based on document timestamps
            // Assume DOC_002 (Student Complaint) happened after DOC_004 (Maintenance Request)
            // So correct order: Maintenance Request -> Student Complaint
            correctOrder = 0;
        }

        protected override void OnPuzzleStarted()
        {
            base.OnPuzzleStarted();
            ShowPuzzleUI();
        }

        protected override void OnPuzzleCompleted()
        {
            base.OnPuzzleCompleted();
            HidePuzzleUI();
            Debug.Log("[LibraryTimeline] Puzzle completed! The timeline is correct.");
        }

        protected override void OnRequirementsNotMet()
        {
            base.OnRequirementsNotMet();
            Debug.Log("[LibraryTimeline] You need to read the relevant documents first.");
        }

        public override void OnStateChanged(PuzzleState newState)
        {
            base.OnStateChanged(newState);

            if (newState == PuzzleState.Solved && puzzleUIPanel != null)
            {
                puzzleUIPanel.SetActive(false);
            }
        }

        private void ShowPuzzleUI()
        {
            if (puzzleUIPanel == null)
            {
                // Create UI programmatically if not assigned
                CreatePuzzleUI();
            }

            puzzleUIPanel.SetActive(true);
            puzzleActive = true;

            if (questionText != null)
            {
                questionText.text = "Based on the documents you've read,\n" +
                                   "which event happened FIRST?\n\n" +
                                   "A) Maintenance Request (MT-2024-0389)\n" +
                                   "B) Student Complaint Letter";
            }

            if (feedbackText != null)
            {
                feedbackText.text = "";
            }

            // Set up button listeners
            if (optionAButton != null)
            {
                optionAButton.onClick.RemoveAllListeners();
                optionAButton.onClick.AddListener(() => SubmitAnswer(0));
            }

            if (optionBButton != null)
            {
                optionBButton.onClick.RemoveAllListeners();
                optionBButton.onClick.AddListener(() => SubmitAnswer(1));
            }

            SetPlayerMovement(false);
        }

        private void HidePuzzleUI()
        {
            if (puzzleUIPanel != null)
            {
                puzzleUIPanel.SetActive(false);
            }

            puzzleActive = false;
            SetPlayerMovement(true);
        }

        private void SubmitAnswer(int answer)
        {
            if (!puzzleActive) return;

            if (answer == correctOrder)
            {
                if (feedbackText != null)
                {
                    feedbackText.text = "CORRECT! The maintenance request was filed before the student complaint.";
                    feedbackText.color = Color.green;
                }

                // Delay completion to show feedback
                StartCoroutine(CompleteAfterDelay(1.5f));
            }
            else
            {
                if (feedbackText != null)
                {
                    feedbackText.text = "INCORRECT. Think about the dates in the documents...";
                    feedbackText.color = Color.red;
                }
            }
        }

        private System.Collections.IEnumerator CompleteAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            CompletePuzzle();
        }

        private void SetPlayerMovement(bool enabled)
        {
            var playerController = FindObjectOfType<EscapeCampus.Player.FirstPersonController>();
            if (playerController != null)
            {
                playerController.enabled = enabled;
            }

            var interactionSystem = FindObjectOfType<EscapeCampus.Interaction.InteractionSystem>();
            if (interactionSystem != null)
            {
                interactionSystem.enabled = enabled;
            }

            if (enabled)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        private void CreatePuzzleUI()
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null) return;

            // Panel
            puzzleUIPanel = new GameObject("LibraryTimelinePuzzleUI");
            puzzleUIPanel.transform.SetParent(canvas.transform, false);

            UnityEngine.UI.Image bg = puzzleUIPanel.AddComponent<UnityEngine.UI.Image>();
            bg.color = new Color(0, 0, 0, 0.9f);

            RectTransform panelRect = puzzleUIPanel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.2f, 0.15f);
            panelRect.anchorMax = new Vector2(0.8f, 0.85f);
            panelRect.sizeDelta = Vector2.zero;

            // Question text
            GameObject questionObj = new GameObject("QuestionText");
            questionObj.transform.SetParent(puzzleUIPanel.transform, false);

            questionText = questionObj.AddComponent<UnityEngine.UI.Text>();
            questionText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            questionText.fontSize = 24;
            questionText.color = Color.white;
            questionText.alignment = TextAnchor.MiddleCenter;

            RectTransform questionRect = questionObj.GetComponent<RectTransform>();
            questionRect.anchorMin = new Vector2(0.05f, 0.6f);
            questionRect.anchorMax = new Vector2(0.95f, 0.95f);
            questionRect.sizeDelta = Vector2.zero;

            // Feedback text
            GameObject feedbackObj = new GameObject("FeedbackText");
            feedbackObj.transform.SetParent(puzzleUIPanel.transform, false);

            feedbackText = feedbackObj.AddComponent<UnityEngine.UI.Text>();
            feedbackText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            feedbackText.fontSize = 20;
            feedbackText.color = Color.white;
            feedbackText.alignment = TextAnchor.MiddleCenter;

            RectTransform feedbackRect = feedbackObj.GetComponent<RectTransform>();
            feedbackRect.anchorMin = new Vector2(0.05f, 0.45f);
            feedbackRect.anchorMax = new Vector2(0.95f, 0.6f);
            feedbackRect.sizeDelta = Vector2.zero;

            // Button container
            GameObject buttonContainer = new GameObject("ButtonContainer");
            buttonContainer.transform.SetParent(puzzleUIPanel.transform, false);

            RectTransform containerRect = buttonContainer.AddComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(0.1f, 0.1f);
            containerRect.anchorMax = new Vector2(0.9f, 0.4f);
            containerRect.sizeDelta = Vector2.zero;

            // Option A button
            optionAButton = CreatePuzzleButton(buttonContainer.transform, "OptionA",
                "A) Maintenance Request First", new Vector2(0, 0.5f));

            // Option B button
            optionBButton = CreatePuzzleButton(buttonContainer.transform, "OptionB",
                "B) Student Complaint First", new Vector2(0, 0));

            puzzleUIPanel.SetActive(false);
        }

        private UnityEngine.UI.Button CreatePuzzleButton(Transform parent, string name, string text, Vector2 position)
        {
            GameObject btnObj = new GameObject(name);
            btnObj.transform.SetParent(parent, false);

            UnityEngine.UI.Image btnImage = btnObj.AddComponent<UnityEngine.UI.Image>();
            btnImage.color = new Color(0.3f, 0.3f, 0.5f);

            UnityEngine.UI.Button btn = btnObj.AddComponent<UnityEngine.UI.Button>();

            RectTransform btnRect = btnObj.GetComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(0.1f, position.y);
            btnRect.anchorMax = new Vector2(0.9f, position.y + 0.4f);
            btnRect.sizeDelta = Vector2.zero;

            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(btnObj.transform, false);

            UnityEngine.UI.Text btnText = textObj.AddComponent<UnityEngine.UI.Text>();
            btnText.text = text;
            btnText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            btnText.fontSize = 20;
            btnText.color = Color.white;
            btnText.alignment = TextAnchor.MiddleCenter;

            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;

            return btn;
        }
    }
}
