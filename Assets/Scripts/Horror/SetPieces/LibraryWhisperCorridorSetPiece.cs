using UnityEngine;
using System.Collections;
using EscapeCampus.Core;
using EscapeCampus.Horror.Semester14;

namespace EscapeCampus.Horror.SetPieces
{
    public class LibraryWhisperCorridorSetPiece : SetPieceBase
    {
        [Header("Corridor Settings")]
        [SerializeField] private Transform corridorStart;
        [SerializeField] private Transform corridorEnd;
        [SerializeField] private Transform lookBackTarget;
        [SerializeField] private Transform doorTransform;

        [Header("Lights")]
        [SerializeField] private Light[] corridorLights;

        [Header("Audio")]
        [SerializeField] private AudioSource whisperSource;
        [SerializeField] private AudioClip whisperClip;

        private void Awake()
        {
            if (string.IsNullOrEmpty(setPieceID))
            {
                setPieceID = "SP_LIBRARY_WHISPER_CORRIDOR";
                setPieceName = "Library Whisper Corridor";
                setPieceType = SetPieceType.CorridorEvent;
            }
        }

        protected override void OnSetPieceStart()
        {
            LockPlayerControl();

            ScriptedCameraController cam = GetCameraController();
            if (cam != null)
            {
                cam.TakeControl();
            }
        }

        protected override IEnumerator ExecuteSetPiece()
        {
            // Phase 1: Lights turn off behind player (3 seconds)
            yield return StartCoroutine(TurnOffLightsBehind());

            // Phase 2: Whisper increases (2 seconds)
            yield return StartCoroutine(IncreaseWhisper());

            // Phase 3: Door closes (1 second)
            yield return StartCoroutine(CloseDoor());

            // Phase 4: Semester 14 appears for 1 frame behind glass
            yield return StartCoroutine(FlashSemester14());

            // Phase 5: Camera forces look back (2 seconds)
            yield return StartCoroutine(ForceLookBack());

            // Phase 6: Nothing is there - pause (1.5 seconds)
            yield return new WaitForSeconds(1.5f);

            // Phase 7: Restore control
            Complete();
        }

        protected override void OnSetPieceEnd()
        {
            UnlockPlayerControl();

            ScriptedCameraController cam = GetCameraController();
            if (cam != null)
            {
                cam.ReleaseControl();
            }

            // Restore whisper volume
            if (whisperSource != null)
            {
                whisperSource.volume = 0f;
            }
        }

        // ============================================
        // PHASE COROUTINES
        // ============================================

        private IEnumerator TurnOffLightsBehind()
        {
            if (corridorLights == null || corridorLights.Length == 0) yield break;

            Transform player = GameObject.FindWithTag("Player")?.transform;
            if (player == null) yield break;

            float duration = 3f;
            float elapsed = 0f;

            // Sort lights by distance from player (farthest first)
            System.Array.Sort(corridorLights, (a, b) =>
            {
                float distA = Vector3.Distance(player.position, a.transform.position);
                float distB = Vector3.Distance(player.position, b.transform.position);
                return distB.CompareTo(distA);
            });

            int lightsOff = 0;
            float interval = duration / corridorLights.Length;

            while (elapsed < duration)
            {
                int targetOff = Mathf.FloorToInt(elapsed / interval);
                targetOff = Mathf.Min(targetOff, corridorLights.Length);

                while (lightsOff < targetOff)
                {
                    if (corridorLights[lightsOff] != null)
                    {
                        StartCoroutine(FadeLight(corridorLights[lightsOff], 0f, 0.5f));
                    }
                    lightsOff++;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            // Ensure all off
            foreach (Light light in corridorLights)
            {
                if (light != null) light.intensity = 0f;
            }
        }

        private IEnumerator FadeLight(Light light, float targetIntensity, float duration)
        {
            float startIntensity = light.intensity;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                light.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            light.intensity = targetIntensity;
        }

        private IEnumerator IncreaseWhisper()
        {
            if (whisperSource == null || whisperClip == null) yield break;

            whisperSource.clip = whisperClip;
            whisperSource.volume = 0f;
            whisperSource.Play();

            float duration = 2f;
            float elapsed = 0f;
            float maxVolume = 0.7f;

            while (elapsed < duration)
            {
                whisperSource.volume = Mathf.Lerp(0f, maxVolume, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            whisperSource.volume = maxVolume;
        }

        private IEnumerator CloseDoor()
        {
            if (doorTransform == null) yield break;

            Quaternion startRot = doorTransform.localRotation;
            Quaternion targetRot = Quaternion.Euler(0, 90, 0);
            float duration = 0.8f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                doorTransform.localRotation = Quaternion.Slerp(startRot, targetRot, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            doorTransform.localRotation = targetRot;

            // Play door slam sound if available
            AudioSource doorAudio = doorTransform.GetComponent<AudioSource>();
            if (doorAudio != null)
            {
                doorAudio.Play();
            }
        }

        private IEnumerator FlashSemester14()
        {
            // Spawn S14 behind glass for 1 frame
            Vector3 spawnPos = corridorStart != null ?
                corridorStart.position + Vector3.up * 1.5f :
                transform.position + transform.forward * 5f;

            GameObject s14 = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            s14.name = "S14_Flash";
            s14.transform.position = spawnPos;

            Renderer renderer = s14.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material mat = new Material(Shader.Find("Standard"));
                mat.color = new Color(0.02f, 0.02f, 0.05f, 0.8f);
                renderer.material = mat;
            }

            // Remove collider
            Collider col = s14.GetComponent<Collider>();
            if (col != null) Destroy(col);

            // Visible for ~0.1 seconds (few frames)
            yield return new WaitForSeconds(0.1f);

            // Destroy
            Destroy(s14);
        }

        private IEnumerator ForceLookBack()
        {
            ScriptedCameraController cam = GetCameraController();
            if (cam == null) yield break;

            Vector3 lookTarget = lookBackTarget != null ?
                lookBackTarget.position :
                transform.position - transform.forward * 10f;

            cam.ForceLookAt(lookTarget, 1.5f);

            yield return new WaitForSeconds(2f);

            // Micro head shake while looking
            cam.MicroHeadShake(0.3f, 0.5f);

            yield return new WaitForSeconds(0.5f);
        }
    }
}
