using UnityEngine;
using EscapeCampus.Interaction;

namespace EscapeCampus.Core
{
    public class DoorController : MonoBehaviour, IInteractable
    {
        [Header("Door Settings")]
        [SerializeField] private string doorID;
        [SerializeField] private bool isOpen = false;
        [SerializeField] private bool isLocked = false;
        [SerializeField] private float openAngle = 90f;
        [SerializeField] private float closeAngle = 0f;
        [SerializeField] private float speed = 2f;

        [Header("Interaction")]
        [SerializeField] private string openPrompt = "Open Door";
        [SerializeField] private string closePrompt = "Close Door";
        [SerializeField] private string lockedPrompt = "Door is Locked";

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip openSound;
        [SerializeField] private AudioClip closeSound;
        [SerializeField] private AudioClip lockedSound;

        private bool isAnimating;
        private Quaternion targetRotation;

        public string DoorID => doorID;
        public bool IsOpen => isOpen;
        public bool IsLocked => isLocked;

        public string InteractionPrompt
        {
            get
            {
                if (isLocked) return lockedPrompt;
                return isOpen ? closePrompt : openPrompt;
            }
        }

        private void Awake()
        {
            targetRotation = isOpen ? Quaternion.Euler(0, openAngle, 0) : Quaternion.Euler(0, closeAngle, 0);
            transform.localRotation = targetRotation;

            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
            }
        }

        private void Start()
        {
            // Register with WorldStateManager
            if (WorldStateManager.Instance != null)
            {
                WorldStateManager.Instance.SetDoorState(doorID, isOpen);
                WorldStateManager.Instance.OnDoorStateChanged += OnDoorStateChanged;
            }
        }

        private void OnDestroy()
        {
            if (WorldStateManager.Instance != null)
            {
                WorldStateManager.Instance.OnDoorStateChanged -= OnDoorStateChanged;
            }
        }

        public void Interact()
        {
            if (isLocked)
            {
                PlaySound(lockedSound);
                Debug.Log($"[DoorController] {doorID} is locked.");
                return;
            }

            ToggleDoor();
        }

        public void ToggleDoor()
        {
            if (isAnimating) return;

            isOpen = !isOpen;
            targetRotation = isOpen ? Quaternion.Euler(0, openAngle, 0) : Quaternion.Euler(0, closeAngle, 0);

            PlaySound(isOpen ? openSound : closeSound);

            WorldStateManager.Instance?.SetDoorState(doorID, isOpen);

            StopAllCoroutines();
            StartCoroutine(AnimateDoor());
        }

        public void SetOpen(bool open)
        {
            if (isAnimating) return;

            isOpen = open;
            targetRotation = isOpen ? Quaternion.Euler(0, openAngle, 0) : Quaternion.Euler(0, closeAngle, 0);

            StopAllCoroutines();
            StartCoroutine(AnimateDoor());
        }

        public void SetLocked(bool locked)
        {
            isLocked = locked;
        }

        private System.Collections.IEnumerator AnimateDoor()
        {
            isAnimating = true;
            Quaternion startRotation = transform.localRotation;

            float elapsed = 0f;
            float duration = 1f / speed;

            while (elapsed < duration)
            {
                transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.localRotation = targetRotation;
            isAnimating = false;
        }

        private void OnDoorStateChanged(string doorID, bool isOpen)
        {
            if (doorID == this.doorID)
            {
                SetOpen(isOpen);
            }
        }

        private void PlaySound(AudioClip clip)
        {
            if (audioSource != null && clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }
    }
}
