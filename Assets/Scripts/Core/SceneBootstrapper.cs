using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EscapeCampus.Player;
using EscapeCampus.Interaction;
using EscapeCampus.UI;

namespace EscapeCampus.Core
{
    public class SceneBootstrapper : MonoBehaviour
    {
        [Header("Scene Type")]
        [SerializeField] private bool isMainMenu = false;
        [SerializeField] private bool isGameplay = false;

        private void Awake()
        {
            if (isMainMenu)
            {
                SetupMainMenu();
            }
            else if (isGameplay)
            {
                SetupGameplayScene();
            }
        }

        private void SetupMainMenu()
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("MainMenuCanvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
            }

            // Title
            GameObject titleObj = new GameObject("TitleText");
            titleObj.transform.SetParent(canvas.transform, false);
            Text titleText = titleObj.AddComponent<Text>();
            titleText.text = "ESCAPE CAMPUS";
            titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            titleText.fontSize = 48;
            titleText.alignment = TextAnchor.MiddleCenter;
            titleText.color = Color.white;
            RectTransform titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.5f, 0.7f);
            titleRect.anchorMax = new Vector2(0.5f, 0.7f);
            titleRect.sizeDelta = new Vector2(600, 80);
            titleRect.anchoredPosition = Vector2.zero;

            // Start Button
            CreateButton(canvas.transform, "StartButton", "START GAME", new Vector2(0.5f, 0.4f), () =>
            {
                GameManager.Instance?.StartGame();
            });

            // Quit Button
            CreateButton(canvas.transform, "QuitButton", "QUIT", new Vector2(0.5f, 0.3f), () =>
            {
                GameManager.Instance?.QuitGame();
            });
        }

        private void SetupGameplayScene()
        {
            // Player
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj == null)
            {
                playerObj = new GameObject("Player");
                playerObj.tag = "Player";
                playerObj.transform.position = new Vector3(0, 1, 0);
                CharacterController cc = playerObj.AddComponent<CharacterController>();
                cc.height = 2f;
                cc.center = new Vector3(0, 1, 0);
                cc.radius = 0.4f;
                playerObj.AddComponent<FirstPersonController>();
                playerObj.AddComponent<InteractionSystem>();
            }

            // UI Canvas
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("GameplayCanvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();

                UIManager uiManager = canvasObj.AddComponent<UIManager>();

                // Crosshair
                CreateCrosshair(canvas);

                // Interaction Prompt
                CreateInteractionPrompt(canvas);
            }

            // Add Lobby Prototype Builder if not already present
            if (FindObjectOfType<LobbyPrototypeBuilder>() == null)
            {
                GameObject lobbyBuilder = new GameObject("LobbyPrototypeBuilder");
                lobbyBuilder.AddComponent<LobbyPrototypeBuilder>();
            }

            // Add GameManager if not present
            if (GameManager.Instance == null)
            {
                GameObject gmObj = new GameObject("GameManager");
                gmObj.AddComponent<GameManager>();
            }
        }

        private void CreateCrosshair(Canvas canvas)
        {
            GameObject crosshair = new GameObject("Crosshair");
            crosshair.transform.SetParent(canvas.transform, false);

            Image img = crosshair.AddComponent<Image>();
            img.color = Color.white;

            RectTransform rect = crosshair.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(20, 20);
            rect.anchoredPosition = Vector2.zero;
        }

        private void CreateInteractionPrompt(Canvas canvas)
        {
            GameObject promptPanel = new GameObject("InteractionPrompt");
            promptPanel.transform.SetParent(canvas.transform, false);

            Image bg = promptPanel.AddComponent<Image>();
            bg.color = new Color(0, 0, 0, 0.7f);

            RectTransform panelRect = promptPanel.GetComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0.5f, 0.25f);
            panelRect.anchorMax = new Vector2(0.5f, 0.25f);
            panelRect.sizeDelta = new Vector2(250, 50);
            panelRect.anchoredPosition = Vector2.zero;

            GameObject textObj = new GameObject("PromptText");
            textObj.transform.SetParent(promptPanel.transform, false);

            Text text = textObj.AddComponent<Text>();
            text.text = "[E] Interact";
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 22;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.white;

            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
        }

        private void CreateButton(Transform parent, string name, string text, Vector2 anchor, UnityEngine.Events.UnityAction onClick)
        {
            GameObject btnObj = new GameObject(name);
            btnObj.transform.SetParent(parent, false);

            Image btnImage = btnObj.AddComponent<Image>();
            btnImage.color = new Color(0.2f, 0.2f, 0.3f);

            Button btn = btnObj.AddComponent<Button>();
            btn.onClick.AddListener(onClick);

            RectTransform btnRect = btnObj.GetComponent<RectTransform>();
            btnRect.anchorMin = anchor;
            btnRect.anchorMax = anchor;
            btnRect.sizeDelta = new Vector2(200, 50);
            btnRect.anchoredPosition = Vector2.zero;

            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(btnObj.transform, false);

            Text btnText = textObj.AddComponent<Text>();
            btnText.text = text;
            btnText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            btnText.fontSize = 24;
            btnText.alignment = TextAnchor.MiddleCenter;
            btnText.color = Color.white;

            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.sizeDelta = Vector2.zero;
        }
    }
}
