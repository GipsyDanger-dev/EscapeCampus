using UnityEngine;
using System.Collections;

namespace EscapeCampus.Horror.SetPieces
{
    public class ScriptedCameraController : MonoBehaviour
    {
        private Camera controlledCamera;
        private bool isControlled;
        private Coroutine activeCoroutine;

        private float originalFOV;
        private Quaternion originalRotation;

        private void Awake()
        {
            controlledCamera = GetComponent<Camera>();
            if (controlledCamera == null)
            {
                controlledCamera = Camera.main;
            }
        }

        // ============================================
        // PUBLIC API
        // ============================================

        public void TakeControl()
        {
            if (isControlled) return;

            isControlled = true;
            originalFOV = controlledCamera.fieldOfView;
            originalRotation = controlledCamera.transform.localRotation;
        }

        public void ReleaseControl()
        {
            if (!isControlled) return;

            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
                activeCoroutine = null;
            }

            controlledCamera.fieldOfView = originalFOV;
            controlledCamera.transform.localRotation = originalRotation;
            isControlled = false;
        }

        // ============================================
        // CAMERA COMMANDS
        // ============================================

        public void ForceLookAt(Vector3 target, float duration)
        {
            if (!isControlled) TakeControl();
            activeCoroutine = StartCoroutine(ForceLookAtCoroutine(target, duration));
        }

        public void SlowDrag(Vector3 direction, float speed, float duration)
        {
            if (!isControlled) TakeControl();
            activeCoroutine = StartCoroutine(SlowDragCoroutine(direction, speed, duration));
        }

        public void MicroHeadShake(float intensity, float duration)
        {
            if (!isControlled) TakeControl();
            activeCoroutine = StartCoroutine(MicroHeadShakeCoroutine(intensity, duration));
        }

        public void SetFOV(float targetFOV, float duration)
        {
            if (!isControlled) TakeControl();
            activeCoroutine = StartCoroutine(SetFOVCoroutine(targetFOV, duration));
        }

        public void SmoothRotation(Quaternion target, float duration)
        {
            if (!isControlled) TakeControl();
            activeCoroutine = StartCoroutine(SmoothRotationCoroutine(target, duration));
        }

        // ============================================
        // COROUTINES
        // ============================================

        private IEnumerator ForceLookAtCoroutine(Vector3 target, float duration)
        {
            float elapsed = 0f;
            Quaternion startRot = controlledCamera.transform.rotation;

            while (elapsed < duration)
            {
                Vector3 direction = (target - controlledCamera.transform.position).normalized;
                Quaternion targetRot = Quaternion.LookRotation(direction);

                controlledCamera.transform.rotation = Quaternion.Slerp(startRot, targetRot, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            controlledCamera.transform.rotation = Quaternion.LookRotation((target - controlledCamera.transform.position).normalized);
        }

        private IEnumerator SlowDragCoroutine(Vector3 direction, float speed, float duration)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                controlledCamera.transform.Rotate(direction * speed * Time.deltaTime);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        private IEnumerator MicroHeadShakeCoroutine(float intensity, float duration)
        {
            float elapsed = 0f;
            Quaternion baseRotation = controlledCamera.transform.localRotation;

            while (elapsed < duration)
            {
                float x = Random.Range(-intensity, intensity) * 0.1f;
                float y = Random.Range(-intensity, intensity) * 0.1f;
                float z = Random.Range(-intensity * 0.5f, intensity * 0.5f) * 0.1f;

                controlledCamera.transform.localRotation = baseRotation * Quaternion.Euler(x, y, z);

                elapsed += Time.deltaTime;
                yield return null;
            }

            controlledCamera.transform.localRotation = baseRotation;
        }

        private IEnumerator SetFOVCoroutine(float targetFOV, float duration)
        {
            float startFOV = controlledCamera.fieldOfView;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                controlledCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            controlledCamera.fieldOfView = targetFOV;
        }

        private IEnumerator SmoothRotationCoroutine(Quaternion target, float duration)
        {
            Quaternion start = controlledCamera.transform.localRotation;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                controlledCamera.transform.localRotation = Quaternion.Slerp(start, target, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            controlledCamera.transform.localRotation = target;
        }

        public bool IsControlled => isControlled;
    }
}
