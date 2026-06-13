using UnityEngine;
using System.Collections;
using EscapeCampus.Core;
using EscapeCampus.Horror;

namespace EscapeCampus.Horror.SetPieces
{
    public abstract class SetPieceBase : MonoBehaviour
    {
        [Header("SetPiece Identity")]
        [SerializeField] protected string setPieceID;
        [SerializeField] protected string setPieceName;
        [SerializeField] protected SetPieceType setPieceType;

        [Header("Trigger Conditions")]
        [SerializeField] protected StoryPhase requiredPhase = StoryPhase.Introduction;
        [SerializeField] protected float requiredHorrorLevel = 0f;
        [SerializeField] protected string requiredPuzzleID;
        [SerializeField] protected bool useTriggerVolume = true;

        [Header("Settings")]
        [SerializeField] protected bool oneTimeOnly = true;
        [SerializeField] protected float timeout = 60f;

        protected bool isExecuting;
        protected float executionStartTime;

        public string SetPieceID => setPieceID;
        public string SetPieceName => setPieceName;
        public SetPieceType SetPieceType => setPieceType;
        public bool IsExecuting => isExecuting;

        protected virtual void Start()
        {
            if (SetPieceManager.Instance != null)
            {
                SetPieceManager.Instance.RegisterSetPiece(this);
            }
        }

        protected virtual void OnDestroy()
        {
            if (SetPieceManager.Instance != null)
            {
                SetPieceManager.Instance.UnregisterSetPiece(this);
            }
        }

        // ============================================
        // CONDITIONS
        // ============================================

        public virtual bool CanTrigger()
        {
            if (oneTimeOnly && SetPieceManager.Instance != null &&
                SetPieceManager.Instance.IsSetPieceCompleted(setPieceID))
            {
                return false;
            }

            if (LevelFlowManager.Instance != null)
            {
                if (LevelFlowManager.Instance.CurrentPhase < requiredPhase)
                {
                    return false;
                }
            }

            if (requiredHorrorLevel > 0 && HorrorManager.Instance != null)
            {
                if (HorrorManager.Instance.GetHorrorLevel() < requiredHorrorLevel)
                {
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(requiredPuzzleID) && Puzzle.PuzzleManager.Instance != null)
            {
                if (!Puzzle.PuzzleManager.Instance.IsPuzzleCompleted(requiredPuzzleID))
                {
                    return false;
                }
            }

            return true;
        }

        // ============================================
        // EXECUTION
        // ============================================

        public virtual void OnTrigger()
        {
            if (isExecuting) return;

            isExecuting = true;
            executionStartTime = Time.time;

            SetPieceManager.Instance?.SetState(setPieceID, SetPieceState.Active);

            try
            {
                OnSetPieceStart();
                StartCoroutine(ExecuteSetPiece());
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[SetPiece] {setPieceID} failed: {e.Message}");
                FailSafeRecovery();
            }
        }

        public virtual void OnCancel()
        {
            StopAllCoroutines();
            isExecuting = false;
            OnSetPieceEnd();
        }

        protected abstract void OnSetPieceStart();
        protected abstract IEnumerator ExecuteSetPiece();
        protected abstract void OnSetPieceEnd();

        // ============================================
        // PLAYER CONTROL
        // ============================================

        protected void LockPlayerControl()
        {
            var playerController = FindObjectOfType<Player.FirstPersonController>();
            if (playerController != null)
            {
                playerController.enabled = false;
            }

            var interactionSystem = FindObjectOfType<Interaction.InteractionSystem>();
            if (interactionSystem != null)
            {
                interactionSystem.enabled = false;
            }
        }

        protected void UnlockPlayerControl()
        {
            var playerController = FindObjectOfType<Player.FirstPersonController>();
            if (playerController != null)
            {
                playerController.enabled = true;
            }

            var interactionSystem = FindObjectOfType<Interaction.InteractionSystem>();
            if (interactionSystem != null)
            {
                interactionSystem.enabled = true;
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        // ============================================
        // CAMERA
        // ============================================

        protected ScriptedCameraController GetCameraController()
        {
            ScriptedCameraController controller = FindObjectOfType<ScriptedCameraController>();
            if (controller == null)
            {
                Camera mainCam = Camera.main;
                if (mainCam != null)
                {
                    controller = mainCam.gameObject.AddComponent<ScriptedCameraController>();
                }
            }
            return controller;
        }

        // ============================================
        // COMPLETION
        // ============================================

        protected void Complete()
        {
            isExecuting = false;
            OnSetPieceEnd();
            SetPieceManager.Instance?.CompleteSetPiece(setPieceID);
        }

        // ============================================
        // FAIL SAFE
        // ============================================

        protected void FailSafeRecovery()
        {
            Debug.LogWarning($"[SetPiece] FailSafe recovery for: {setPieceID}");
            isExecuting = false;
            UnlockPlayerControl();

            ScriptedCameraController cam = GetCameraController();
            if (cam != null)
            {
                cam.ReleaseControl();
            }

            SetPieceManager.Instance?.FailSafe(setPieceID);
        }

        protected void CheckTimeout()
        {
            if (Time.time - executionStartTime > timeout)
            {
                Debug.LogWarning($"[SetPiece] {setPieceID} timed out after {timeout}s");
                FailSafeRecovery();
            }
        }
    }
}
