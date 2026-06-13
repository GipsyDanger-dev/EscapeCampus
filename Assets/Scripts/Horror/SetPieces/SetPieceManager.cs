using UnityEngine;
using System;
using System.Collections.Generic;

namespace EscapeCampus.Horror.SetPieces
{
    public class SetPieceManager : MonoBehaviour
    {
        public static SetPieceManager Instance { get; private set; }

        private Dictionary<string, SetPieceBase> registeredSetPieces = new Dictionary<string, SetPieceBase>();
        private Dictionary<string, SetPieceState> setPieceStates = new Dictionary<string, SetPieceState>();
        private SetPieceBase activeSetPiece;

        // Events
        public event Action<string> OnSetPieceTriggered;
        public event Action<string> OnSetPieceCompleted;
        public event Action<string, SetPieceState> OnSetPieceStateChanged;

        public SetPieceBase ActiveSetPiece => activeSetPiece;

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

        // ============================================
        // PUBLIC API
        // ============================================

        public void RegisterSetPiece(SetPieceBase setPiece)
        {
            if (setPiece == null) return;

            string id = setPiece.SetPieceID;
            if (!registeredSetPieces.ContainsKey(id))
            {
                registeredSetPieces[id] = setPiece;
                setPieceStates[id] = SetPieceState.Idle;
                Debug.Log($"[SetPieceManager] Registered: {id}");
            }
        }

        public void UnregisterSetPiece(SetPieceBase setPiece)
        {
            if (setPiece == null) return;
            registeredSetPieces.Remove(setPiece.SetPieceID);
        }

        public bool TriggerSetPiece(string setPieceID)
        {
            if (!registeredSetPieces.ContainsKey(setPieceID))
            {
                Debug.LogWarning($"[SetPieceManager] SetPiece not found: {setPieceID}");
                return false;
            }

            // Already active or completed
            SetPieceState currentState = GetSetPieceState(setPieceID);
            if (currentState != SetPieceState.Idle)
            {
                Debug.Log($"[SetPieceManager] {setPieceID} is not idle (state: {currentState})");
                return false;
            }

            // Another setpiece is active
            if (activeSetPiece != null)
            {
                Debug.Log($"[SetPieceManager] Another setpiece is active: {activeSetPiece.SetPieceID}");
                return false;
            }

            SetPieceBase setPiece = registeredSetPieces[setPieceID];

            try
            {
                activeSetPiece = setPiece;
                SetState(setPieceID, SetPieceState.Triggering);
                setPiece.OnTrigger();
                OnSetPieceTriggered?.Invoke(setPieceID);
                Debug.Log($"[SetPieceManager] Triggered: {setPieceID}");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SetPieceManager] Failed to trigger {setPieceID}: {e.Message}");
                FailSafe(setPieceID);
                return false;
            }
        }

        public void CompleteSetPiece(string setPieceID)
        {
            if (!registeredSetPieces.ContainsKey(setPieceID)) return;

            SetState(setPieceID, SetPieceState.Completed);

            if (activeSetPiece != null && activeSetPiece.SetPieceID == setPieceID)
            {
                activeSetPiece = null;
            }

            OnSetPieceCompleted?.Invoke(setPieceID);
            Debug.Log($"[SetPieceManager] Completed: {setPieceID}");
        }

        public SetPieceState GetSetPieceState(string setPieceID)
        {
            if (setPieceStates.ContainsKey(setPieceID))
            {
                return setPieceStates[setPieceID];
            }
            return SetPieceState.Idle;
        }

        public bool IsSetPieceCompleted(string setPieceID)
        {
            return GetSetPieceState(setPieceID) == SetPieceState.Completed;
        }

        public bool IsAnySetPieceActive()
        {
            return activeSetPiece != null;
        }

        // ============================================
        // STATE MANAGEMENT
        // ============================================

        public void SetState(string setPieceID, SetPieceState newState)
        {
            if (setPieceStates.ContainsKey(setPieceID))
            {
                setPieceStates[setPieceID] = newState;
                OnSetPieceStateChanged?.Invoke(setPieceID, newState);
            }
        }

        // ============================================
        // FAIL SAFE
        // ============================================

        public void FailSafe(string setPieceID)
        {
            Debug.LogWarning($"[SetPieceManager] FailSafe triggered for: {setPieceID}");

            // Restore player control
            RestorePlayerControl();

            // Mark as completed to prevent re-trigger
            if (setPieceStates.ContainsKey(setPieceID))
            {
                setPieceStates[setPieceID] = SetPieceState.Completed;
            }

            // Clear active
            if (activeSetPiece != null && activeSetPiece.SetPieceID == setPieceID)
            {
                activeSetPiece = null;
            }
        }

        public void ForceCompleteActive()
        {
            if (activeSetPiece != null)
            {
                string id = activeSetPiece.SetPieceID;
                activeSetPiece.OnCancel();
                FailSafe(id);
            }
        }

        private void RestorePlayerControl()
        {
            // Re-enable player controller
            var playerController = FindObjectOfType<Player.FirstPersonController>();
            if (playerController != null)
            {
                playerController.enabled = true;
            }

            // Re-enable interaction
            var interactionSystem = FindObjectOfType<Interaction.InteractionSystem>();
            if (interactionSystem != null)
            {
                interactionSystem.enabled = true;
            }

            // Restore cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Restore camera
            ScriptedCameraController cameraController = FindObjectOfType<ScriptedCameraController>();
            if (cameraController != null)
            {
                cameraController.ReleaseControl();
            }
        }

        // ============================================
        // SAVE/LOAD
        // ============================================

        public SetPieceSaveData GetSaveData()
        {
            List<SetPieceSaveEntry> entries = new List<SetPieceSaveEntry>();
            foreach (var kvp in setPieceStates)
            {
                entries.Add(new SetPieceSaveEntry(kvp.Key, kvp.Value.ToString()));
            }
            return new SetPieceSaveData { entries = entries };
        }

        public void LoadSaveData(SetPieceSaveData data)
        {
            if (data == null || data.entries == null) return;

            foreach (var entry in data.entries)
            {
                if (Enum.TryParse(entry.state, out SetPieceState state))
                {
                    setPieceStates[entry.setPieceID] = state;
                }
            }

            Debug.Log($"[SetPieceManager] Loaded {data.entries.Count} setpiece states.");
        }
    }

    [Serializable]
    public class SetPieceSaveData
    {
        public List<SetPieceSaveEntry> entries = new List<SetPieceSaveEntry>();
    }

    [Serializable]
    public class SetPieceSaveEntry
    {
        public string setPieceID;
        public string state;

        public SetPieceSaveEntry() { }

        public SetPieceSaveEntry(string id, string state)
        {
            this.setPieceID = id;
            this.state = state;
        }
    }
}
