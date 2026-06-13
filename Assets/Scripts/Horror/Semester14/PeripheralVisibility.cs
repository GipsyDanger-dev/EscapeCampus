using UnityEngine;

namespace EscapeCampus.Horror.Semester14
{
    public class PeripheralVisibility : MonoBehaviour
    {
        private Camera playerCamera;
        private Renderer objectRenderer;
        private bool isVisibleToCamera;

        public void Initialize(Camera camera)
        {
            playerCamera = camera;
            objectRenderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            if (playerCamera == null || objectRenderer == null) return;

            // Check if object is in camera view
            Vector3 viewportPos = playerCamera.WorldToViewportPoint(transform.position);
            bool isInView = viewportPos.z > 0 &&
                           viewportPos.x > 0 && viewportPos.x < 1 &&
                           viewportPos.y > 0 && viewportPos.y < 1;

            // Check if player is looking directly at it
            bool isLookingDirectly = false;
            if (isInView)
            {
                Vector3 dirToObj = (transform.position - playerCamera.transform.position).normalized;
                float dot = Vector3.Dot(playerCamera.transform.forward, dirToObj);
                isLookingDirectly = dot > 0.8f; // Looking within ~36 degree cone
            }

            // Peripheral: visible when in view but NOT looked at directly
            if (isInView && !isLookingDirectly)
            {
                if (!isVisibleToCamera)
                {
                    isVisibleToCamera = true;
                    SetVisibility(true);
                }
            }
            else
            {
                if (isVisibleToCamera)
                {
                    isVisibleToCamera = false;
                    SetVisibility(false);
                }
            }
        }

        private void SetVisibility(bool visible)
        {
            if (objectRenderer != null)
            {
                objectRenderer.enabled = visible;
            }
        }
    }
}
