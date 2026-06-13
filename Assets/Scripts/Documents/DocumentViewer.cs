using UnityEngine;
using UnityEngine.UI;

namespace EscapeCampus.Documents
{
    public class DocumentViewer : MonoBehaviour
    {
        [SerializeField] private GameObject viewerPanel;
        [SerializeField] private Text titleText;
        [SerializeField] private Text contentText;
        [SerializeField] private Text categoryText;
        [SerializeField] private ScrollRect scrollRect;

        private bool isOpen;
        private DocumentData currentDocument;

        public bool IsOpen => isOpen;

        public event System.Action OnDocumentOpened;
        public event System.Action OnDocumentClosed;

        private void Awake()
        {
            if (viewerPanel != null)
            {
                viewerPanel.SetActive(false);
            }
        }

        private void Update()
        {
            if (isOpen && Input.GetKeyDown(KeyCode.Escape))
            {
                CloseDocument();
            }
        }

        public void OpenDocument(DocumentData document)
        {
            if (document == null) return;

            currentDocument = document;
            isOpen = true;

            if (viewerPanel != null)
            {
                viewerPanel.SetActive(true);
            }

            if (titleText != null)
            {
                titleText.text = document.title;
            }

            if (contentText != null)
            {
                contentText.text = document.content;
            }

            if (categoryText != null)
            {
                string criticalTag = document.isCritical ? " [CRITICAL]" : "";
                categoryText.text = $"{document.category}{criticalTag}";
            }

            if (scrollRect != null)
            {
                scrollRect.verticalNormalizedPosition = 1f;
            }

            SetPlayerMovement(false);
            OnDocumentOpened?.Invoke();

            Debug.Log($"[DocumentViewer] Opened: {document.title}");
        }

        public void CloseDocument()
        {
            if (!isOpen) return;

            isOpen = false;
            currentDocument = null;

            if (viewerPanel != null)
            {
                viewerPanel.SetActive(false);
            }

            SetPlayerMovement(true);
            OnDocumentClosed?.Invoke();

            Debug.Log("[DocumentViewer] Closed document.");
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
    }
}
