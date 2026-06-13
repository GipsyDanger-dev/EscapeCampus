using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace EscapeCampus.Horror.Events
{
    public class DocumentTextShiftEvent : HorrorEvent
    {
        [Header("Text Shift Settings")]
        [SerializeField] private float shiftDuration = 2f;
        [SerializeField] private float glitchIntensity = 0.5f;

        [Header("Glitch Texts")]
        [SerializeField] private string[] glitchTexts = new string[]
        {
            "They're watching...",
            "Don't trust the documents...",
            "The truth is buried...",
            "You shouldn't be reading this...",
            "HELP ME"
        };

        private Text currentTextComponent;
        private string originalText;
        private bool isShifting;

        private void Awake()
        {
            if (string.IsNullOrEmpty(eventID))
            {
                eventID = "HORROR_TEXT_SHIFT";
                eventName = "Document Text Shift";
                eventType = HorrorEventType.UI;
            }

            cooldown = 60f; // Longer cooldown for this effect
        }

        public override bool CanExecute()
        {
            return base.CanExecute() && !isShifting;
        }

        public override bool Execute()
        {
            if (!CanExecute()) return false;

            // Find active document viewer
            Documents.DocumentViewer viewer = FindObjectOfType<Documents.DocumentViewer>();
            if (viewer == null || !viewer.IsOpen)
            {
                return false;
            }

            // Find text component in viewer
            Text[] texts = viewer.GetComponentsInChildren<Text>();
            foreach (Text text in texts)
            {
                if (text.gameObject.name == "ContentText")
                {
                    currentTextComponent = text;
                    break;
                }
            }

            if (currentTextComponent == null) return false;

            StartCoroutine(TextShiftCoroutine());
            return true;
        }

        public override void Cancel()
        {
            StopAllCoroutines();
            isShifting = false;

            if (currentTextComponent != null && originalText != null)
            {
                currentTextComponent.text = originalText;
            }
        }

        private IEnumerator TextShiftCoroutine()
        {
            isShifting = true;
            originalText = currentTextComponent.text;

            // Phase 1: Brief glitch
            string glitchText = glitchTexts[Random.Range(0, glitchTexts.Length)];
            currentTextComponent.text = glitchText;
            currentTextComponent.color = Color.red;

            yield return new WaitForSeconds(0.3f);

            // Phase 2: Restore partially
            currentTextComponent.text = originalText;
            currentTextComponent.color = new Color(0.8f, 0.8f, 0.8f);

            yield return new WaitForSeconds(0.5f);

            // Phase 3: Another brief glitch
            currentTextComponent.text = glitchTexts[Random.Range(0, glitchTexts.Length)];
            currentTextComponent.color = new Color(1f, 0.5f, 0.5f);

            yield return new WaitForSeconds(0.2f);

            // Phase 4: Restore completely
            currentTextComponent.text = originalText;
            currentTextComponent.color = Color.white;

            isShifting = false;
            Debug.Log("[DocumentTextShiftEvent] Text shifted momentarily.");
        }
    }
}
