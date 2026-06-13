using UnityEngine;
using System.Collections;

namespace EscapeCampus.Horror.Events
{
    public class DoorCloseEvent : HorrorEvent
    {
        [Header("Door Settings")]
        [SerializeField] private Transform doorTransform;
        [SerializeField] private float closeSpeed = 2f;
        [SerializeField] private float closedAngle = 90f;
        [SerializeField] private float openAngle = 0f;

        [Header("Audio")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip closeSound;

        private bool isOpen = true;
        private bool isAnimating;

        private void Awake()
        {
            if (string.IsNullOrEmpty(eventID))
            {
                eventID = "HORROR_DOOR_CLOSE";
                eventName = "Door Close";
                eventType = HorrorEventType.Environmental;
            }
        }

        public override bool CanExecute()
        {
            return base.CanExecute() && isOpen && !isAnimating;
        }

        public override bool Execute()
        {
            if (!CanExecute()) return false;

            StartCoroutine(CloseDoorCoroutine());
            return true;
        }

        public override void Cancel()
        {
            StopAllCoroutines();
            isAnimating = false;
        }

        private IEnumerator CloseDoorCoroutine()
        {
            isAnimating = true;

            if (audioSource != null && closeSound != null)
            {
                audioSource.PlayOneShot(closeSound);
            }

            Quaternion startRotation = doorTransform.localRotation;
            Quaternion targetRotation = Quaternion.Euler(0, closedAngle, 0);
            float elapsed = 0f;
            float duration = 1f / closeSpeed;

            while (elapsed < duration)
            {
                doorTransform.localRotation = Quaternion.Slerp(startRotation, targetRotation, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            doorTransform.localRotation = targetRotation;
            isOpen = false;
            isAnimating = false;

            Debug.Log("[DoorCloseEvent] Door closed mysteriously.");
        }
    }
}
