using UnityEngine;
using System.Collections;

namespace EscapeCampus.Horror.Semester14
{
    public class MissingFrameBlink : MonoBehaviour
    {
        private float visibleDuration = 0.1f;
        private float invisibleDuration = 0.5f;
        private Renderer objectRenderer;
        private bool isRunning;

        public void Initialize(float visible, float invisible)
        {
            visibleDuration = visible;
            invisibleDuration = invisible;
            objectRenderer = GetComponent<Renderer>();
            isRunning = true;
            StartCoroutine(BlinkCoroutine());
        }

        private IEnumerator BlinkCoroutine()
        {
            while (isRunning)
            {
                // Invisible
                if (objectRenderer != null)
                {
                    objectRenderer.enabled = false;
                }
                yield return new WaitForSeconds(invisibleDuration);

                // Visible (brief flash)
                if (objectRenderer != null)
                {
                    objectRenderer.enabled = true;
                }
                yield return new WaitForSeconds(visibleDuration);
            }
        }

        private void OnDestroy()
        {
            isRunning = false;
            StopAllCoroutines();
        }
    }
}
