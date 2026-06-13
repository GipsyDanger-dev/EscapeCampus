using UnityEngine;

namespace EscapeCampus.Horror
{
    public enum HorrorEventType
    {
        Audio,
        Visual,
        Environmental,
        UI
    }

    public abstract class HorrorEvent : MonoBehaviour
    {
        [Header("Event Identity")]
        [SerializeField] protected string eventID;
        [SerializeField] protected string eventName;
        [SerializeField] protected HorrorEventType eventType;

        [Header("Settings")]
        [SerializeField] protected float minHorrorLevel = 0f;
        [SerializeField] protected float cooldown = 30f;
        [SerializeField] protected bool isOneTime = false;
        [SerializeField] protected bool isEnabled = true;

        public string EventID => eventID;
        public string EventName => eventName;
        public HorrorEventType EventType => eventType;
        public float MinHorrorLevel => minHorrorLevel;
        public float Cooldown => cooldown;
        public bool IsOneTime => isOneTime;
        public bool IsEnabled => isEnabled;

        public virtual bool CanExecute()
        {
            if (!isEnabled) return false;
            if (HorrorManager.Instance == null) return false;

            float currentLevel = HorrorManager.Instance.GetHorrorLevel();
            if (currentLevel < minHorrorLevel) return false;

            return true;
        }

        public abstract bool Execute();

        public virtual void Cancel()
        {
            // Override for cleanup
        }

        public void SetEnabled(bool enabled)
        {
            isEnabled = enabled;
        }
    }
}
