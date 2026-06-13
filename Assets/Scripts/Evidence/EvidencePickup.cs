using UnityEngine;
using EscapeCampus.Interaction;

namespace EscapeCampus.Evidence
{
    public class EvidencePickup : MonoBehaviour, IInteractable
    {
        [SerializeField] private EvidenceData evidenceData;
        [SerializeField] private string interactionPrompt = "Collect Evidence";
        [SerializeField] private bool destroyAfterPickup = false;

        public string InteractionPrompt
        {
            get
            {
                if (evidenceData != null)
                {
                    return $"Collect: {evidenceData.title}";
                }
                return interactionPrompt;
            }
        }

        public EvidenceData EvidenceData => evidenceData;

        public void Interact()
        {
            if (evidenceData == null)
            {
                Debug.LogWarning($"[EvidencePickup] No EvidenceData assigned to {gameObject.name}");
                return;
            }

            EvidenceManager manager = EvidenceManager.Instance;
            if (manager == null)
            {
                Debug.LogError("[EvidenceManager] Instance not found!");
                return;
            }

            bool collected = manager.CollectEvidence(evidenceData);

            if (collected && destroyAfterPickup)
            {
                Destroy(gameObject);
            }
        }

        public void SetEvidenceData(EvidenceData data)
        {
            evidenceData = data;
        }
    }
}
