using UnityEngine;

namespace EscapeCampus.Interaction
{
    public class InteractableObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private string interactionPrompt = "Interact";
        [SerializeField] private bool destroyOnInteract = false;

        public string InteractionPrompt => interactionPrompt;

        public void Interact()
        {
            Debug.Log($"[Interaction] {gameObject.name} interacted!");

            if (destroyOnInteract)
            {
                Destroy(gameObject);
            }
        }
    }
}
