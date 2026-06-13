using UnityEngine;

namespace EscapeCampus.Horror.Events
{
    public class WhisperAudioEvent : HorrorEvent
    {
        [Header("Whisper Settings")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip[] whisperClips;
        [SerializeField] [Range(0f, 1f)] private float volume = 0.3f;
        [SerializeField] private float spatialBlend = 1f; // 3D audio

        private void Awake()
        {
            if (string.IsNullOrEmpty(eventID))
            {
                eventID = "HORROR_WHISPER";
                eventName = "Whisper";
                eventType = HorrorEventType.Audio;
            }

            if (audioSource == null)
            {
                audioSource = GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    audioSource = gameObject.AddComponent<AudioSource>();
                }
            }

            audioSource.spatialBlend = spatialBlend;
            audioSource.playOnAwake = false;
        }

        public override bool CanExecute()
        {
            return base.CanExecute() && audioSource != null && !audioSource.isPlaying;
        }

        public override bool Execute()
        {
            if (!CanExecute()) return false;

            if (whisperClips == null || whisperClips.Length == 0)
            {
                Debug.LogWarning($"[WhisperAudioEvent] No whisper clips assigned to {eventID}");
                return false;
            }

            AudioClip clip = whisperClips[Random.Range(0, whisperClips.Length)];
            audioSource.volume = volume;
            audioSource.clip = clip;
            audioSource.Play();

            Debug.Log($"[WhisperAudioEvent] Playing whisper: {clip.name}");
            return true;
        }

        public override void Cancel()
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
