using UnityEngine;

namespace EscapeCampus.Horror
{
    public class HorrorDebugTool : MonoBehaviour
    {
        [SerializeField] private KeyCode increaseLevelKey = KeyCode.F4;
        [SerializeField] private KeyCode decreaseLevelKey = KeyCode.F5;
        [SerializeField] private KeyCode resetHorrorKey = KeyCode.F6;
        [SerializeField] private float levelStep = 10f;

        private void Update()
        {
            if (Input.GetKeyDown(increaseLevelKey))
            {
                IncreaseLevel();
            }

            if (Input.GetKeyDown(decreaseLevelKey))
            {
                DecreaseLevel();
            }

            if (Input.GetKeyDown(resetHorrorKey))
            {
                ResetHorror();
            }
        }

        private void IncreaseLevel()
        {
            if (HorrorManager.Instance != null)
            {
                float newLevel = HorrorManager.Instance.GetHorrorLevel() + levelStep;
                HorrorManager.Instance.DebugSetLevel(newLevel);
                Debug.Log($"[HorrorDebug] Level increased to {newLevel:F1} ({HorrorManager.Instance.GetHorrorStage()})");
            }
        }

        private void DecreaseLevel()
        {
            if (HorrorManager.Instance != null)
            {
                float newLevel = HorrorManager.Instance.GetHorrorLevel() - levelStep;
                HorrorManager.Instance.DebugSetLevel(newLevel);
                Debug.Log($"[HorrorDebug] Level decreased to {newLevel:F1} ({HorrorManager.Instance.GetHorrorStage()})");
            }
        }

        private void ResetHorror()
        {
            if (HorrorManager.Instance != null)
            {
                HorrorManager.Instance.DebugResetAll();
                Debug.Log("[HorrorDebug] Horror system reset.");
            }
        }
    }
}
