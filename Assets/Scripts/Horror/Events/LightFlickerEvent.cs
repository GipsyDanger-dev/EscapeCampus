using UnityEngine;
using System.Collections;

namespace EscapeCampus.Horror.Events
{
    public class LightFlickerEvent : HorrorEvent
    {
        [Header("Light Flicker Settings")]
        [SerializeField] private Light targetLight;
        [SerializeField] private float flickerDuration = 3f;
        [SerializeField] private float flickerSpeed = 0.1f;
        [SerializeField] private float minIntensity = 0.1f;
        [SerializeField] private float maxIntensity = 2f;

        private float originalIntensity;
        private bool isFlickering;

        private void Awake()
        {
            if (string.IsNullOrEmpty(eventID))
            {
                eventID = "HORROR_LIGHT_FLICKER";
                eventName = "Light Flicker";
                eventType = HorrorEventType.Environmental;
            }

            if (targetLight == null)
            {
                targetLight = GetComponent<Light>();
            }

            if (targetLight != null)
            {
                originalIntensity = targetLight.intensity;
            }
        }

        public override bool CanExecute()
        {
            return base.CanExecute() && targetLight != null && !isFlickering;
        }

        public override bool Execute()
        {
            if (!CanExecute()) return false;

            StartCoroutine(FlickerCoroutine());
            return true;
        }

        public override void Cancel()
        {
            StopAllCoroutines();
            isFlickering = false;

            if (targetLight != null)
            {
                targetLight.intensity = originalIntensity;
            }
        }

        private IEnumerator FlickerCoroutine()
        {
            isFlickering = true;
            float elapsed = 0f;

            while (elapsed < flickerDuration)
            {
                float randomIntensity = Random.Range(minIntensity, maxIntensity);
                targetLight.intensity = randomIntensity;

                yield return new WaitForSeconds(flickerSpeed);
                elapsed += flickerSpeed;
            }

            targetLight.intensity = originalIntensity;
            isFlickering = false;
        }
    }
}
