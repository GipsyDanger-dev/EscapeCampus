using UnityEngine;
using UnityEngine.UI;
using EscapeCampus.Interaction;

namespace EscapeCampus.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private InteractionSystem interactionSystem;
        [SerializeField] private GameObject crosshairPanel;
        [SerializeField] private GameObject interactionPromptPanel;
        [SerializeField] private Text interactionPromptText;

        private void Start()
        {
            if (interactionSystem == null)
            {
                interactionSystem = FindObjectOfType<InteractionSystem>();
            }

            if (interactionSystem != null)
            {
                interactionSystem.OnInteractableFound += ShowInteractionPrompt;
                interactionSystem.OnInteractableLost += HideInteractionPrompt;
            }

            HideInteractionPrompt();
        }

        private void OnDestroy()
        {
            if (interactionSystem != null)
            {
                interactionSystem.OnInteractableFound -= ShowInteractionPrompt;
                interactionSystem.OnInteractableLost -= HideInteractionPrompt;
            }
        }

        private void ShowInteractionPrompt(IInteractable interactable)
        {
            if (interactionPromptPanel != null)
            {
                interactionPromptPanel.SetActive(true);
            }

            if (interactionPromptText != null)
            {
                interactionPromptText.text = $"[E] {interactable.InteractionPrompt}";
            }
        }

        private void HideInteractionPrompt()
        {
            if (interactionPromptPanel != null)
            {
                interactionPromptPanel.SetActive(false);
            }
        }
    }
}
