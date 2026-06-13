using UnityEngine;
using UnityEngine.SceneManagement;

namespace EscapeCampus.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [SerializeField] private string lobbySceneName = "LobbyPrototype";

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

        public void StartGame()
        {
            SceneManager.LoadScene(lobbySceneName);
        }

        public void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void QuitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
