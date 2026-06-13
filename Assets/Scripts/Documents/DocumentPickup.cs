using UnityEngine;
using EscapeCampus.Interaction;

namespace EscapeCampus.Documents
{
    public class DocumentPickup : MonoBehaviour, IInteractable
    {
        [SerializeField] private DocumentData documentData;
        [SerializeField] private string interactionPrompt = "Pick Up Document";
        [SerializeField] private bool destroyAfterPickup = false;

        public string InteractionPrompt
        {
            get
            {
                if (documentData != null)
                {
                    return $"Pick Up: {documentData.title}";
                }
                return interactionPrompt;
            }
        }

        public DocumentData DocumentData => documentData;

        public void Interact()
        {
            if (documentData == null)
            {
                Debug.LogWarning($"[DocumentPickup] No DocumentData assigned to {gameObject.name}");
                return;
            }

            DocumentManager manager = DocumentManager.Instance;
            if (manager == null)
            {
                Debug.LogError("[DocumentManager] Instance not found!");
                return;
            }

            bool collected = manager.CollectDocument(documentData);

            if (collected)
            {
                DocumentViewer viewer = FindObjectOfType<DocumentViewer>();
                if (viewer != null)
                {
                    viewer.OpenDocument(documentData);
                }

                if (destroyAfterPickup)
                {
                    Destroy(gameObject);
                }
            }
        }

        public void SetDocumentData(DocumentData data)
        {
            documentData = data;
        }
    }
}
