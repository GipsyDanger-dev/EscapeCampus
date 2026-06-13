using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace EscapeCampus.UI
{
    public class SaveUI : MonoBehaviour
    {
        [SerializeField] private KeyCode saveKey = KeyCode.F5;
        [SerializeField] private KeyCode loadKey = KeyCode.F9;

        [Header("References")]
        [SerializeField] private GameObject messagePanel;
        [SerializeField] private Text messageText;

        private Coroutine messageCoroutine;

        private void Start()
        {
            if (Save.SaveManager.Instance != null)
            {
                Save.SaveManager.Instance.OnGameSaved += ShowSaveMessage;
                Save.SaveManager.Instance.OnGameLoaded += ShowLoadMessage;
            }

            if (messagePanel != null)
            {
                messagePanel.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            if (Save.SaveManager.Instance != null)
            {
                Save.SaveManager.Instance.OnGameSaved -= ShowSaveMessage;
                Save.SaveManager.Instance.OnGameLoaded -= ShowLoadMessage;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(saveKey))
            {
                Save.SaveManager.Instance?.SaveGame(isAutosave: false);
            }

            if (Input.GetKeyDown(loadKey))
            {
                Save.SaveManager.Instance?.LoadGame(fromAutosave: false);
            }
        }

        private void ShowSaveMessage(string slotName)
        {
            ShowMessage($"Game Saved ({slotName})");
        }

        private void ShowLoadMessage(string slotName)
        {
            ShowMessage($"Game Loaded ({slotName})");
        }

        private void ShowMessage(string message)
        {
            if (messageCoroutine != null)
            {
                StopCoroutine(messageCoroutine);
            }

            messageCoroutine = StartCoroutine(DisplayMessage(message));
        }

        private IEnumerator DisplayMessage(string message)
        {
            if (messagePanel != null)
            {
                messagePanel.SetActive(true);
            }

            if (messageText != null)
            {
                messageText.text = message;
            }

            yield return new WaitForSeconds(2f);

            if (messagePanel != null)
            {
                messagePanel.SetActive(false);
            }

            messageCoroutine = null;
        }
    }
}
