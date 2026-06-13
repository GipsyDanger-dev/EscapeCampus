using UnityEngine;

namespace EscapeCampus.Interaction
{
    public class InteractionSystem : MonoBehaviour
    {
        [SerializeField] private float interactionDistance = 3f;
        [SerializeField] private LayerMask interactionLayer = ~0;
        [SerializeField] private KeyCode interactKey = KeyCode.E;
        [SerializeField] private Camera playerCamera;

        private IInteractable currentInteractable;
        private bool isInteracting;

        public IInteractable CurrentInteractable => currentInteractable;
        public bool HasInteractable => currentInteractable != null;

        public event System.Action<IInteractable> OnInteractableFound;
        public event System.Action OnInteractableLost;

        private void Start()
        {
            if (playerCamera == null)
            {
                playerCamera = GetComponentInChildren<Camera>();
            }

            if (playerCamera == null)
            {
                Debug.LogError("[InteractionSystem] No camera found! Assign playerCamera in inspector.");
                enabled = false;
                return;
            }
        }

        private void Update()
        {
            CheckForInteractable();
            HandleInteractionInput();
        }

        private void CheckForInteractable()
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, interactionLayer))
            {
                IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    if (currentInteractable != interactable)
                    {
                        currentInteractable = interactable;
                        OnInteractableFound?.Invoke(currentInteractable);
                    }
                    return;
                }
            }

            if (currentInteractable != null)
            {
                currentInteractable = null;
                OnInteractableLost?.Invoke();
            }
        }

        private void HandleInteractionInput()
        {
            if (currentInteractable != null && Input.GetKeyDown(interactKey))
            {
                currentInteractable.Interact();
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (playerCamera != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * interactionDistance);
            }
        }
    }
}
