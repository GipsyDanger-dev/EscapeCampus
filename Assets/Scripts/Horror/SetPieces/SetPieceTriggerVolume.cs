using UnityEngine;

namespace EscapeCampus.Horror.SetPieces
{
    [RequireComponent(typeof(Collider))]
    public class SetPieceTriggerVolume : MonoBehaviour
    {
        [SerializeField] private string setPieceID;
        [SerializeField] private bool disableAfterTrigger = true;

        private bool hasTriggered;

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
            if (hasTriggered && disableAfterTrigger) return;

            if (!other.CompareTag("Player")) return;

            if (SetPieceManager.Instance == null) return;

            SetPieceBase setPiece = FindSetPiece();
            if (setPiece == null) return;

            if (!setPiece.CanTrigger()) return;

            bool success = SetPieceManager.Instance.TriggerSetPiece(setPieceID);
            if (success)
            {
                hasTriggered = true;
                if (disableAfterTrigger)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        private SetPieceBase FindSetPiece()
        {
            SetPieceBase[] setPieces = FindObjectsOfType<SetPieceBase>();
            foreach (var sp in setPieces)
            {
                if (sp.SetPieceID == setPieceID)
                {
                    return sp;
                }
            }
            return null;
        }
    }
}
