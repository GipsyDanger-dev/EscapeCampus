using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using EscapeCampus.Core;
using EscapeCampus.Horror;

namespace EscapeCampus.Horror.Semester14
{
    public class Semester14Observer : MonoBehaviour
    {
        public static Semester14Observer Instance { get; private set; }

        [Header("Spawn Settings")]
        [SerializeField] private float minHorrorLevel = 40f;
        [SerializeField] private StoryPhase minimumPhase = StoryPhase.FirstAnomaly;
        [SerializeField] private float minCooldown = 60f;
        [SerializeField] private float maxCooldown = 120f;
        [SerializeField] private float spawnChance = 0.3f;

        [Header("Observation Settings")]
        [SerializeField] private float observationDuration = 5f;
        [SerializeField] private float despawnFadeTime = 1f;

        [Header("References")]
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Camera playerCamera;

        // State
        private bool isObserving;
        private ObservationType currentObservationType;
        private float lastObservationTime;
        private float nextCooldownTime;
        private GameObject activeObservationObject;

        // Data tracking
        private int totalObservations;
        private Dictionary<ObservationType, int> observationTypeFrequency = new Dictionary<ObservationType, int>();
        private float lastObservationTimestamp;

        // Observation points
        private List<ObservationPoint> registeredPoints = new List<ObservationPoint>();

        // Events
        public event Action<ObservationType> OnObservationStarted;
        public event Action<ObservationType> OnObservationEnded;
        public event Action OnEnvironmentResponse;

        public bool IsObserving => isObserving;
        public int TotalObservations => totalObservations;
        public float LastObservationTime => lastObservationTimestamp;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeFrequencyMap();
        }

        private void Start()
        {
            if (playerTransform == null)
            {
                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    playerTransform = player.transform;
                }
            }

            if (playerCamera == null)
            {
                playerCamera = Camera.main;
            }

            SubscribeToEvents();
            nextCooldownTime = UnityEngine.Random.Range(minCooldown, maxCooldown);
        }

        private void OnDestroy()
        {
            UnsubscribeFromEvents();
        }

        private void Update()
        {
            if (!CanSpawn()) return;

            if (Time.time - lastObservationTime >= nextCooldownTime)
            {
                TrySpawnObservation();
            }
        }

        // ============================================
        // PUBLIC API
        // ============================================

        public void RegisterObservationPoint(ObservationPoint point)
        {
            if (point != null && !registeredPoints.Contains(point))
            {
                registeredPoints.Add(point);
            }
        }

        public void UnregisterObservationPoint(ObservationPoint point)
        {
            registeredPoints.Remove(point);
        }

        public bool TrySpawnObservation()
        {
            if (!CanSpawn()) return false;

            // Random chance check
            if (UnityEngine.Random.value > spawnChance)
            {
                ResetCooldown();
                return false;
            }

            // Select observation type
            ObservationType type = SelectObservationType();

            // Find spawn position
            Vector3 spawnPos = GetSpawnPosition(type);
            if (spawnPos == Vector3.zero) return false;

            // Spawn
            SpawnObservation(type, spawnPos);
            return true;
        }

        public void DespawnObservation()
        {
            if (!isObserving) return;

            isObserving = false;
            OnObservationEnded?.Invoke(currentObservationType);

            if (activeObservationObject != null)
            {
                StartCoroutine(FadeAndDestroy(activeObservationObject));
                activeObservationObject = null;
            }

            ResetCooldown();
            Debug.Log($"[Semester14] Observation ended: {currentObservationType}");
        }

        public bool GetActiveObservationState()
        {
            return isObserving;
        }

        public ObservationType GetCurrentObservationType()
        {
            return currentObservationType;
        }

        public float GetDistanceToPlayer()
        {
            if (activeObservationObject == null || playerTransform == null) return -1f;
            return Vector3.Distance(playerTransform.position, activeObservationObject.transform.position);
        }

        // ============================================
        // SPAWN LOGIC
        // ============================================

        private bool CanSpawn()
        {
            // Already observing
            if (isObserving) return false;

            // Horror level check
            if (HorrorManager.Instance == null) return false;
            if (HorrorManager.Instance.GetHorrorLevel() < minHorrorLevel) return false;

            // Story phase check
            if (LevelFlowManager.Instance == null) return false;
            if (LevelFlowManager.Instance.CurrentPhase < minimumPhase) return false;

            return true;
        }

        private void SpawnObservation(ObservationType type, Vector3 position)
        {
            isObserving = true;
            currentObservationType = type;
            lastObservationTime = Time.time;
            lastObservationTimestamp = Time.time;
            totalObservations++;

            // Track frequency
            if (observationTypeFrequency.ContainsKey(type))
            {
                observationTypeFrequency[type]++;
            }

            // Create observation object
            activeObservationObject = CreateObservationObject(type, position);

            // Trigger environment response
            TriggerEnvironmentResponse();

            // Auto despawn after duration
            StartCoroutine(AutoDespawnCoroutine());

            OnObservationStarted?.Invoke(type);
            Debug.Log($"[Semester14] Observation started: {type} at {position}");
        }

        private ObservationType SelectObservationType()
        {
            // Weighted selection based on horror level
            float level = HorrorManager.Instance?.GetHorrorLevel() ?? 0f;

            if (level < 50f)
            {
                // Lower levels: mostly static
                return UnityEngine.Random.value > 0.7f ? ObservationType.Peripheral : ObservationType.Static;
            }
            else if (level < 70f)
            {
                // Medium levels: add mirror
                float r = UnityEngine.Random.value;
                if (r < 0.3f) return ObservationType.Static;
                if (r < 0.6f) return ObservationType.Peripheral;
                return ObservationType.Mirror;
            }
            else
            {
                // High levels: all types including missing frame
                float r = UnityEngine.Random.value;
                if (r < 0.2f) return ObservationType.Static;
                if (r < 0.4f) return ObservationType.Peripheral;
                if (r < 0.7f) return ObservationType.Mirror;
                return ObservationType.MissingFrame;
            }
        }

        private Vector3 GetSpawnPosition(ObservationType type)
        {
            if (playerTransform == null) return Vector3.zero;

            switch (type)
            {
                case ObservationType.Static:
                    return GetStaticSpawnPosition();
                case ObservationType.Peripheral:
                    return GetPeripheralSpawnPosition();
                case ObservationType.Mirror:
                    return GetMirrorSpawnPosition();
                case ObservationType.MissingFrame:
                    return GetStaticSpawnPosition(); // Same as static but with blink
                default:
                    return Vector3.zero;
            }
        }

        private Vector3 GetStaticSpawnPosition()
        {
            // Spawn in front of player, far away
            Vector3 forward = playerTransform.forward;
            Vector3 right = playerTransform.right;

            // Random angle in front hemisphere
            float angle = UnityEngine.Random.Range(-45f, 45f);
            Vector3 direction = Quaternion.Euler(0, angle, 0) * forward;

            float distance = UnityEngine.Random.Range(15f, 30f);
            return playerTransform.position + direction * distance;
        }

        private Vector3 GetPeripheralSpawnPosition()
        {
            // Spawn at edge of camera view
            Vector3 right = playerTransform.right;
            float side = UnityEngine.Random.value > 0.5f ? 1f : -1f;
            float distance = UnityEngine.Random.Range(8f, 15f);

            return playerTransform.position + (right * side * distance) + (playerTransform.forward * 2f);
        }

        private Vector3 GetMirrorSpawnPosition()
        {
            // Spawn behind player
            return playerTransform.position - playerTransform.forward * 5f;
        }

        private GameObject CreateObservationObject(ObservationType type, Vector3 position)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            obj.name = $"Semester14_Observation_{type}";
            obj.transform.position = position;

            // Make it dark, barely visible
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material mat = new Material(Shader.Find("Standard"));
                mat.color = new Color(0.05f, 0.05f, 0.08f);
                mat.SetFloat("_Mode", 3); // Transparent
                mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mat.SetInt("_ZWrite", 0);
                mat.DisableKeyword("_ALPHATEST_ON");
                mat.EnableKeyword("_ALPHABLEND_ON");
                mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                mat.renderQueue = 3000;
                mat.color = new Color(0.05f, 0.05f, 0.08f, 0.6f);
                renderer.material = mat;
            }

            // Remove collider - no interaction
            Collider col = obj.GetComponent<Collider>();
            if (col != null)
            {
                Destroy(col);
            }

            // Apply observation type behavior
            switch (type)
            {
                case ObservationType.Peripheral:
                    ApplyPeripheralBehavior(obj);
                    break;
                case ObservationType.Mirror:
                    ApplyMirrorBehavior(obj);
                    break;
                case ObservationType.MissingFrame:
                    ApplyMissingFrameBehavior(obj);
                    break;
            }

            // Face player
            if (playerTransform != null)
            {
                obj.transform.LookAt(new Vector3(playerTransform.position.x, obj.transform.position.y, playerTransform.position.z));
            }

            return obj;
        }

        private void ApplyPeripheralBehavior(GameObject obj)
        {
            // Only visible when not directly looked at
            PeripheralVisibility vis = obj.AddComponent<PeripheralVisibility>();
            vis.Initialize(playerCamera);
        }

        private void ApplyMirrorBehavior(GameObject obj)
        {
            // Very transparent, only visible in specific conditions
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color c = renderer.material.color;
                c.a = 0.3f;
                renderer.material.color = c;
            }
        }

        private void ApplyMissingFrameBehavior(GameObject obj)
        {
            // Blink effect
            MissingFrameBlink blink = obj.AddComponent<MissingFrameBlink>();
            blink.Initialize(0.1f, 0.5f); // 0.1s visible, 0.5s invisible
        }

        // ============================================
        // ENVIRONMENT RESPONSE
        // ============================================

        private void TriggerEnvironmentResponse()
        {
            OnEnvironmentResponse?.Invoke();

            // Lights desync
            TriggerLightDesync();

            // Audio drop
            TriggerAudioDrop();

            // UI flicker
            TriggerUIFlicker();

            // Random door state change
            TriggerDoorChange();
        }

        private void TriggerLightDesync()
        {
            Light[] lights = FindObjectsOfType<Light>();
            foreach (Light light in lights)
            {
                if (UnityEngine.Random.value > 0.5f)
                {
                    StartCoroutine(LightFlickerCoroutine(light));
                }
            }
        }

        private IEnumerator LightFlickerCoroutine(Light light)
        {
            float originalIntensity = light.intensity;
            float elapsed = 0f;

            while (elapsed < 0.5f)
            {
                light.intensity = originalIntensity * UnityEngine.Random.Range(0.3f, 1.2f);
                yield return new WaitForSeconds(0.05f);
                elapsed += 0.05f;
            }

            light.intensity = originalIntensity;
        }

        private void TriggerAudioDrop()
        {
            // Brief audio silence
            AudioSource[] sources = FindObjectsOfType<AudioSource>();
            StartCoroutine(AudioDropCoroutine(sources));
        }

        private IEnumerator AudioDropCoroutine(AudioSource[] sources)
        {
            float[] originalVolumes = new float[sources.Length];
            for (int i = 0; i < sources.Length; i++)
            {
                if (sources[i] != null)
                {
                    originalVolumes[i] = sources[i].volume;
                    sources[i].volume = 0f;
                }
            }

            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < sources.Length; i++)
            {
                if (sources[i] != null)
                {
                    sources[i].volume = originalVolumes[i];
                }
            }
        }

        private void TriggerUIFlicker()
        {
            // Handled by UIGlitchEvent
            if (HorrorManager.Instance != null)
            {
                HorrorEvent[] events = FindObjectsOfType<Horror.Events.UIGlitchEvent>();
                foreach (var evt in events)
                {
                    if (evt.CanExecute())
                    {
                        HorrorManager.Instance.TriggerHorrorEvent(evt);
                        break;
                    }
                }
            }
        }

        private void TriggerDoorChange()
        {
            if (WorldStateManager.Instance == null) return;

            DoorController[] doors = FindObjectsOfType<DoorController>();
            if (doors.Length > 0)
            {
                DoorController randomDoor = doors[UnityEngine.Random.Range(0, doors.Length)];
                randomDoor.ToggleDoor();
            }
        }

        // ============================================
        // HELPERS
        // ============================================

        private void ResetCooldown()
        {
            nextCooldownTime = UnityEngine.Random.Range(minCooldown, maxCooldown);
            lastObservationTime = Time.time;
        }

        private IEnumerator AutoDespawnCoroutine()
        {
            yield return new WaitForSeconds(observationDuration);
            DespawnObservation();
        }

        private IEnumerator FadeAndDestroy(GameObject obj)
        {
            if (obj == null) yield break;

            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer == null)
            {
                Destroy(obj);
                yield break;
            }

            float elapsed = 0f;
            Color startColor = renderer.material.color;

            while (elapsed < despawnFadeTime)
            {
                float alpha = Mathf.Lerp(startColor.a, 0f, elapsed / despawnFadeTime);
                renderer.material.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                elapsed += Time.deltaTime;
                yield return null;
            }

            Destroy(obj);
        }

        private void InitializeFrequencyMap()
        {
            foreach (ObservationType type in Enum.GetValues(typeof(ObservationType)))
            {
                observationTypeFrequency[type] = 0;
            }
        }

        // ============================================
        // EVENT SUBSCRIPTIONS
        // ============================================

        private void SubscribeToEvents()
        {
            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.OnHorrorStageChanged += OnHorrorStageChanged;
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.OnHorrorStageChanged -= OnHorrorStageChanged;
            }
        }

        private void OnHorrorStageChanged(HorrorStage stage)
        {
            // Increase spawn chance at higher stages
            if (stage >= HorrorStage.Paranoia)
            {
                spawnChance = 0.5f;
            }
            else if (stage >= HorrorStage.Disturbance)
            {
                spawnChance = 0.35f;
            }
        }

        // ============================================
        // SAVE/LOAD
        // ============================================

        public ObservationSaveData GetSaveData()
        {
            return new ObservationSaveData
            {
                totalObservations = totalObservations,
                lastObservationTime = lastObservationTimestamp,
                typeFrequency = new Dictionary<ObservationType, int>(observationTypeFrequency)
            };
        }

        public void LoadSaveData(ObservationSaveData data)
        {
            if (data == null) return;

            totalObservations = data.totalObservations;
            lastObservationTimestamp = data.lastObservationTime;

            if (data.typeFrequency != null)
            {
                observationTypeFrequency = new Dictionary<ObservationType, int>(data.typeFrequency);
            }

            Debug.Log($"[Semester14] Loaded: Total observations={totalObservations}");
        }

        // ============================================
        // DEBUG
        // ============================================

        public void DebugForceSpawn()
        {
            ObservationType type = SelectObservationType();
            Vector3 pos = GetSpawnPosition(type);
            if (pos != Vector3.zero)
            {
                SpawnObservation(type, pos);
                Debug.Log($"[Semester14] Debug: Forced spawn {type}");
            }
        }

        public void DebugClearAll()
        {
            StopAllCoroutines();
            DespawnObservation();

            if (activeObservationObject != null)
            {
                Destroy(activeObservationObject);
                activeObservationObject = null;
            }

            isObserving = false;
            Debug.Log("[Semester14] Debug: Cleared all observations.");
        }
    }

    [Serializable]
    public class ObservationSaveData
    {
        public int totalObservations;
        public float lastObservationTime;
        public Dictionary<ObservationType, int> typeFrequency;
    }

    [Serializable]
    public class ObservationPoint
    {
        public string pointID;
        public Vector3 position;
        public ObservationType preferredType;
        public float weight = 1f;
    }
}
