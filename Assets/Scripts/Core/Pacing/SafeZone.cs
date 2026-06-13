using UnityEngine;

namespace EscapeCampus.Core.Pacing
{
    [RequireComponent(typeof(Collider))]
    public class SafeZone : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private string zoneName = "Safe Room";
        [SerializeField] private float tensionDecayMultiplier = 2f;
        [SerializeField] private bool disableHorrorEvents = true;
        [SerializeField] private bool disableSetPieces = true;

        private void Awake()
        {
            Collider col = GetComponent<Collider>();
            if (col != null)
            {
                col.isTrigger = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            if (ExperienceDirector.Instance != null)
            {
                ExperienceDirector.Instance.SetSafeZone(true);
            }

            Debug.Log($"[SafeZone] Player entered: {zoneName}");
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            if (ExperienceDirector.Instance != null)
            {
                ExperienceDirector.Instance.SetSafeZone(false);
            }

            Debug.Log($"[SafeZone] Player left: {zoneName}");
        }
    }
}
