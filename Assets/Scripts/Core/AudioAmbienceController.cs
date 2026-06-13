using UnityEngine;
using System.Collections.Generic;

namespace EscapeCampus.Core
{
    public class AudioAmbienceController : MonoBehaviour
    {
        [Header("Audio Source")]
        [SerializeField] private AudioSource ambienceSource;

        [Header("Ambience Clips")]
        [SerializeField] private AudioClip normalAmbience;
        [SerializeField] private AudioClip suspiciousAmbience;
        [SerializeField] private AudioClip corruptedAmbience;
        [SerializeField] private AudioClip brokenAmbience;

        [Header("Settings")]
        [SerializeField] private float crossfadeDuration = 2f;
        [SerializeField] [Range(0f, 1f)] private float maxVolume = 0.5f;

        private WorldState currentState;

        private void Awake()
        {
            if (ambienceSource == null)
            {
                ambienceSource = GetComponent<AudioSource>();
                if (ambienceSource == null)
                {
                    ambienceSource = gameObject.AddComponent<AudioSource>();
                }
            }

            ambienceSource.loop = true;
            ambienceSource.playOnAwake = false;
            ambienceSource.volume = 0f;
        }

        private void Start()
        {
            if (WorldStateManager.Instance != null)
            {
                WorldStateManager.Instance.OnWorldStateChanged += OnWorldStateChanged;
                currentState = WorldStateManager.Instance.CurrentState;
                ApplyAmbience(currentState);
            }
        }

        private void OnDestroy()
        {
            if (WorldStateManager.Instance != null)
            {
                WorldStateManager.Instance.OnWorldStateChanged -= OnWorldStateChanged;
            }
        }

        private void OnWorldStateChanged(WorldState oldState, WorldState newState)
        {
            currentState = newState;
            StopAllCoroutines();
            StartCoroutine(CrossfadeAmbience(newState));
        }

        private void ApplyAmbience(WorldState state)
        {
            AudioClip clip = GetClipForState(state);
            if (clip != null)
            {
                ambienceSource.clip = clip;
                ambienceSource.volume = maxVolume;
                ambienceSource.Play();
            }
        }

        private System.Collections.IEnumerator CrossfadeAmbience(WorldState newState)
        {
            float startVolume = ambienceSource.volume;

            // Fade out
            float elapsed = 0f;
            while (elapsed < crossfadeDuration / 2f)
            {
                ambienceSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / (crossfadeDuration / 2f));
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Switch clip
            AudioClip newClip = GetClipForState(newState);
            if (newClip != null)
            {
                ambienceSource.clip = newClip;
                ambienceSource.Play();
            }

            // Fade in
            elapsed = 0f;
            while (elapsed < crossfadeDuration / 2f)
            {
                ambienceSource.volume = Mathf.Lerp(0f, maxVolume, elapsed / (crossfadeDuration / 2f));
                elapsed += Time.deltaTime;
                yield return null;
            }

            ambienceSource.volume = maxVolume;
        }

        private AudioClip GetClipForState(WorldState state)
        {
            switch (state)
            {
                case WorldState.Normal: return normalAmbience;
                case WorldState.Suspicious: return suspiciousAmbience;
                case WorldState.Corrupted: return corruptedAmbience;
                case WorldState.BrokenReality: return brokenAmbience;
                default: return normalAmbience;
            }
        }

        public void SetVolume(float volume)
        {
            maxVolume = Mathf.Clamp01(volume);
            ambienceSource.volume = maxVolume;
        }
    }
}
