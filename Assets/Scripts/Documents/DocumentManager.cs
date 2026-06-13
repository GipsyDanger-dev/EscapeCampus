using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace EscapeCampus.Documents
{
    public class DocumentManager : MonoBehaviour
    {
        public static DocumentManager Instance { get; private set; }

        private HashSet<string> collectedDocumentIDs = new HashSet<string>();
        private List<DocumentData> collectedDocuments = new List<DocumentData>();

        public event System.Action<DocumentData> OnDocumentCollected;
        public IReadOnlyList<DocumentData> CollectedDocuments => collectedDocuments;
        public int TotalCollected => collectedDocuments.Count;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public bool CollectDocument(DocumentData document)
        {
            if (document == null) return false;

            if (collectedDocumentIDs.Contains(document.documentID))
            {
                Debug.Log($"[DocumentManager] Document '{document.title}' already collected.");
                return false;
            }

            collectedDocumentIDs.Add(document.documentID);
            collectedDocuments.Add(document);

            Debug.Log($"[DocumentManager] Collected: '{document.title}' ({collectedDocuments.Count} total)");
            OnDocumentCollected?.Invoke(document);

            return true;
        }

        public bool HasDocument(string documentID)
        {
            return collectedDocumentIDs.Contains(documentID);
        }

        public bool HasDocument(DocumentData document)
        {
            return document != null && collectedDocumentIDs.Contains(document.documentID);
        }

        public DocumentData GetDocument(string documentID)
        {
            return collectedDocuments.FirstOrDefault(d => d.documentID == documentID);
        }

        public List<DocumentData> GetDocumentsByCategory(DocumentCategory category)
        {
            return collectedDocuments.Where(d => d.category == category).ToList();
        }

        public List<DocumentData> GetCriticalDocuments()
        {
            return collectedDocuments.Where(d => d.isCritical).ToList();
        }
    }
}
