using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace EscapeCampus.Horror.Events
{
    public class UIGlitchEvent : HorrorEvent
    {
        [Header("UI Glitch Settings")]
        [SerializeField] private float glitchDuration = 1.5f;
        [SerializeField] private float flickerSpeed = 0.05f;
        [SerializeField] private Canvas targetCanvas;

        [Header("Glitch Colors")]
        [SerializeField] private Color glitchColor1 = new Color(1f, 0f, 0f, 0.3f);
        [SerializeField] private Color glitchColor2 = new Color(0f, 1f, 0f, 0.3f);

        private Image glitchOverlay;
        private bool isGlitching;

        private void Awake()
        {
            if (string.IsNullOrEmpty(eventID))
            {
                eventID = "HORROR_UI_GLITCH";
                eventName = "UI Glitch";
                eventType = HorrorEventType.UI;
            }
        }

        public override bool CanExecute()
        {
            return base.CanExecute() && !isGlitching;
        }

        public override bool Execute()
        {
            if (!CanExecute()) return false;

            if (targetCanvas == null)
            {
                targetCanvas = FindObjectOfType<Canvas>();
            }

            if (targetCanvas == null) return false;

            CreateGlitchOverlay();
            StartCoroutine(GlitchCoroutine());
            return true;
        }

        public override void Cancel()
        {
            StopAllCoroutines();
            isGlitching = false;

            if (glitchOverlay != null)
            {
                Destroy(glitchOverlay.gameObject);
            }
        }

        private void CreateGlitchOverlay()
        {
            GameObject overlayObj = new GameObject("GlitchOverlay");
            overlayObj.transform.SetParent(targetCanvas.transform, false);

            glitchOverlay = overlayObj.AddComponent<Image>();
            glitchOverlay.color = Color.clear;
            glitchOverlay.raycastTarget = false;

            RectTransform rect = overlayObj.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;
        }

        private IEnumerator GlitchCoroutine()
        {
            isGlitching = true;
            float elapsed = 0f;

            while (elapsed < glitchDuration)
            {
                // Random glitch flashes
                if (Random.value > 0.5f)
                {
                    glitchOverlay.color = Random.value > 0.5f ? glitchColor1 : glitchColor2;
                }
                else
                {
                    glitchOverlay.color = Color.clear;
                }

                yield return new WaitForSeconds(flickerSpeed);
                elapsed += flickerSpeed;
            }

            glitchOverlay.color = Color.clear;

            yield return new WaitForSeconds(0.1f);

            if (glitchOverlay != null)
            {
                Destroy(glitchOverlay.gameObject);
            }

            isGlitching = false;
            Debug.Log("[UIGlitchEvent] UI glitch completed.");
        }
    }
}
