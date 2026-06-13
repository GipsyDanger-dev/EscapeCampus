using UnityEngine;
using System;
using System.Collections.Generic;

namespace EscapeCampus.Core
{
    public enum WorldState
    {
        Normal,
        Suspicious,
        Corrupted,
        BrokenReality
    }

    public class WorldStateManager : MonoBehaviour
    {
        public static WorldStateManager Instance { get; private set; }

        [Header("Current State")]
        [SerializeField] private WorldState currentWorldState = WorldState.Normal;

        [Header("Lighting")]
        [SerializeField] private Color normalAmbient = new Color(0.5f, 0.5f, 0.6f);
        [SerializeField] private Color suspiciousAmbient = new Color(0.4f, 0.4f, 0.55f);
        [SerializeField] private Color corruptedAmbient = new Color(0.3f, 0.25f, 0.35f);
        [SerializeField] private Color brokenAmbient = new Color(0.15f, 0.1f, 0.2f);

        [Header("Fog")]
        [SerializeField] private bool useFog = true;
        [SerializeField] private Color normalFog = new Color(0.5f, 0.5f, 0.6f, 1f);
        [SerializeField] private Color suspiciousFog = new Color(0.4f, 0.35f, 0.5f, 1f);
        [SerializeField] private Color corruptedFog = new Color(0.25f, 0.2f, 0.35f, 1f);
        [SerializeField] private Color brokenFog = new Color(0.1f, 0.05f, 0.15f, 1f);

        [Header("Fog Density")]
        [SerializeField] private float normalFogDensity = 0.01f;
        [SerializeField] private float suspiciousFogDensity = 0.02f;
        [SerializeField] private float corruptedFogDensity = 0.04f;
        [SerializeField] private float brokenFogDensity = 0.08f;

        // Runtime changeable objects
        private List<WorldObject> managedObjects = new List<WorldObject>();
        private Dictionary<string, bool> doorStates = new Dictionary<string, bool>();

        // Events
        public event Action<WorldState, WorldState> OnWorldStateChanged;
        public event Action<string, bool> OnDoorStateChanged;
        public event Action<string> OnAnomalyTriggered;

        public WorldState CurrentState => currentWorldState;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            ApplyWorldState(currentWorldState);
        }

        // ============================================
        // PUBLIC API
        // ============================================

        public void SetWorldState(WorldState newState)
        {
            if (newState == currentWorldState) return;

            WorldState oldState = currentWorldState;
            currentWorldState = newState;

            ApplyWorldState(newState);

            Debug.Log($"[WorldStateManager] State changed: {oldState} -> {newState}");
            OnWorldStateChanged?.Invoke(oldState, newState);
        }

        public void ChangeEnvironmentState(string stateName)
        {
            Debug.Log($"[WorldStateManager] Environment state changed: {stateName}");
            OnAnomalyTriggered?.Invoke(stateName);
        }

        public void EnableAnomalyMode()
        {
            SetWorldState(WorldState.Corrupted);
        }

        public void DisableNormalMode()
        {
            SetWorldState(WorldState.Normal);
        }

        // ============================================
        // DOOR MANAGEMENT
        // ============================================

        public void SetDoorState(string doorID, bool isOpen)
        {
            doorStates[doorID] = isOpen;
            OnDoorStateChanged?.Invoke(doorID, isOpen);
            Debug.Log($"[WorldStateManager] Door {doorID} {(isOpen ? "opened" : "closed")}");
        }

        public bool GetDoorState(string doorID)
        {
            return doorStates.ContainsKey(doorID) && doorStates[doorID];
        }

        public void LockDoor(string doorID)
        {
            SetDoorState(doorID, false);
        }

        public void UnlockDoor(string doorID)
        {
            SetDoorState(doorID, true);
        }

        // ============================================
        // OBJECT MANAGEMENT
        // ============================================

        public void RegisterWorldObject(WorldObject obj)
        {
            if (obj != null && !managedObjects.Contains(obj))
            {
                managedObjects.Add(obj);
            }
        }

        public void UnregisterWorldObject(WorldObject obj)
        {
            managedObjects.Remove(obj);
        }

        public void RepositionObject(string objectID, Vector3 newPosition)
        {
            foreach (var obj in managedObjects)
            {
                if (obj.ObjectID == objectID)
                {
                    obj.SetPosition(newPosition);
                    Debug.Log($"[WorldStateManager] Repositioned {objectID} to {newPosition}");
                    return;
                }
            }
        }

        // ============================================
        // STATE APPLICATION
        // ============================================

        private void ApplyWorldState(WorldState state)
        {
            switch (state)
            {
                case WorldState.Normal:
                    ApplyNormalState();
                    break;
                case WorldState.Suspicious:
                    ApplySuspiciousState();
                    break;
                case WorldState.Corrupted:
                    ApplyCorruptedState();
                    break;
                case WorldState.BrokenReality:
                    ApplyBrokenRealityState();
                    break;
            }
        }

        private void ApplyNormalState()
        {
            RenderSettings.ambientLight = normalAmbient;

            if (useFog)
            {
                RenderSettings.fog = true;
                RenderSettings.fogColor = normalFog;
                RenderSettings.fogDensity = normalFogDensity;
            }
        }

        private void ApplySuspiciousState()
        {
            RenderSettings.ambientLight = suspiciousAmbient;

            if (useFog)
            {
                RenderSettings.fog = true;
                RenderSettings.fogColor = suspiciousFog;
                RenderSettings.fogDensity = suspiciousFogDensity;
            }
        }

        private void ApplyCorruptedState()
        {
            RenderSettings.ambientLight = corruptedAmbient;

            if (useFog)
            {
                RenderSettings.fog = true;
                RenderSettings.fogColor = corruptedFog;
                RenderSettings.fogDensity = corruptedFogDensity;
            }
        }

        private void ApplyBrokenRealityState()
        {
            RenderSettings.ambientLight = brokenAmbient;

            if (useFog)
            {
                RenderSettings.fog = true;
                RenderSettings.fogColor = brokenFog;
                RenderSettings.fogDensity = brokenFogDensity;
            }
        }

        // ============================================
        // SAVE/LOAD
        // ============================================

        public WorldStateSaveData GetSaveData()
        {
            return new WorldStateSaveData
            {
                worldState = currentWorldState.ToString(),
                doorStates = new Dictionary<string, bool>(doorStates)
            };
        }

        public void LoadSaveData(WorldStateSaveData data)
        {
            if (data == null) return;

            if (Enum.TryParse(data.worldState, out WorldState state))
            {
                SetWorldState(state);
            }

            if (data.doorStates != null)
            {
                doorStates = new Dictionary<string, bool>(data.doorStates);
            }

            Debug.Log($"[WorldStateManager] Loaded: State={currentWorldState}");
        }
    }

    [Serializable]
    public class WorldObject : MonoBehaviour
    {
        [SerializeField] private string objectID;
        private Vector3 originalPosition;

        public string ObjectID => objectID;

        private void Awake()
        {
            originalPosition = transform.position;
        }

        private void Start()
        {
            WorldStateManager.Instance?.RegisterWorldObject(this);
        }

        private void OnDestroy()
        {
            WorldStateManager.Instance?.UnregisterWorldObject(this);
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void ResetPosition()
        {
            transform.position = originalPosition;
        }
    }

    [Serializable]
    public class WorldStateSaveData
    {
        public string worldState;
        public Dictionary<string, bool> doorStates;
    }
}
