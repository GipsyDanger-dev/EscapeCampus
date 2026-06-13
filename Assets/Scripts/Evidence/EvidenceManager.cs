using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace EscapeCampus.Evidence
{
    public class EvidenceManager : MonoBehaviour
    {
        public static EvidenceManager Instance { get; private set; }

        private HashSet<string> collectedEvidenceIDs = new HashSet<string>();
        private List<EvidenceData> collectedEvidence = new List<EvidenceData>();

        public event System.Action<EvidenceData> OnEvidenceCollected;
        public IReadOnlyList<EvidenceData> CollectedEvidence => collectedEvidence;
        public int TotalCollected => collectedEvidence.Count;

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

        public bool CollectEvidence(EvidenceData evidence)
        {
            if (evidence == null) return false;

            if (collectedEvidenceIDs.Contains(evidence.evidenceID))
            {
                Debug.Log($"[EvidenceManager] Evidence '{evidence.title}' already collected.");
                return false;
            }

            collectedEvidenceIDs.Add(evidence.evidenceID);
            collectedEvidence.Add(evidence);

            Debug.Log($"[EvidenceManager] Collected: '{evidence.title}' ({collectedEvidence.Count} total)");
            OnEvidenceCollected?.Invoke(evidence);

            return true;
        }

        public bool HasEvidence(string evidenceID)
        {
            return collectedEvidenceIDs.Contains(evidenceID);
        }

        public bool HasEvidence(EvidenceData evidence)
        {
            return evidence != null && collectedEvidenceIDs.Contains(evidence.evidenceID);
        }

        public EvidenceData GetEvidence(string evidenceID)
        {
            return collectedEvidence.FirstOrDefault(e => e.evidenceID == evidenceID);
        }

        public List<EvidenceData> GetCriticalEvidence()
        {
            return collectedEvidence.Where(e => e.isCritical).ToList();
        }
    }
}
