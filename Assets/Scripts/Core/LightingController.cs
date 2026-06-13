using UnityEngine;
using System.Collections;

namespace EscapeCampus.Core
{
    public class LightingController : MonoBehaviour
    {
        [Header("Target Light")]
        [SerializeField] private Light targetLight;

        [Header("State Colors")]
        [SerializeField] private Color normalColor = new Color(1f, 0.95f, 0.9f);
        [SerializeField] private Color suspiciousColor = new Color(0.9f, 0.85f, 0.95f);
        [SerializeField] private Color corruptedColor = new Color(0.7f, 0.5f, 0.7f);
        [SerializeField] private Color brokenColor = new Color(0.4f, 0.2f, 0.5f);

        [Header("State Intensities")]
        [SerializeField] private float normalIntensity = 1f;
        [SerializeField] private float suspiciousIntensity = 0.8f;
        [SerializeField] private float corruptedIntensity = 0.5f;
        [SerializeField] private float brokenIntensity = 0.3f;

        [Header("Transition")]
        [SerializeField] private float transitionSpeed = 2f;

        private Color targetColor;
        private float targetIntensity;

        private void Awake()
        {
            if (targetLight == null)
            {
                targetLight = GetComponent<Light>();
            }
        }

        private void Start()
        {
            if (WorldStateManager.Instance != null)
            {
                WorldStateManager.Instance.OnWorldStateChanged += OnWorldStateChanged;
                ApplyState(WorldStateManager.Instance.CurrentState);
            }
        }

        private void OnDestroy()
        {
            if (WorldStateManager.Instance != null)
            {
                WorldStateManager.Instance.OnWorldStateChanged -= OnWorldStateChanged;
            }
        }

        private void Update()
        {
            if (targetLight != null)
            {
                targetLight.color = Color.Lerp(targetLight.color, targetColor, transitionSpeed * Time.deltaTime);
                targetLight.intensity = Mathf.Lerp(targetLight.intensity, targetIntensity, transitionSpeed * Time.deltaTime);
            }
        }

        private void OnWorldStateChanged(WorldState oldState, WorldState newState)
        {
            ApplyState(newState);
        }

        private void ApplyState(WorldState state)
        {
            switch (state)
            {
                case WorldState.Normal:
                    targetColor = normalColor;
                    targetIntensity = normalIntensity;
                    break;
                case WorldState.Suspicious:
                    targetColor = suspiciousColor;
                    targetIntensity = suspiciousIntensity;
                    break;
                case WorldState.Corrupted:
                    targetColor = corruptedColor;
                    targetIntensity = corruptedIntensity;
                    break;
                case WorldState.BrokenReality:
                    targetColor = brokenColor;
                    targetIntensity = brokenIntensity;
                    break;
            }
        }

        public void SetColor(Color color)
        {
            targetColor = color;
        }

        public void SetIntensity(float intensity)
        {
            targetIntensity = intensity;
        }
    }
}
