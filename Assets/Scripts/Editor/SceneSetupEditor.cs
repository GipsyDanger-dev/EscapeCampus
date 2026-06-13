#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

namespace EscapeCampus.Editor
{
    public class SceneSetupEditor : EditorWindow
    {
        [MenuItem("EscapeCampus/Setup Project")]
        public static void SetupProject()
        {
            if (EditorUtility.DisplayDialog("Setup EscapeCampus",
                "This will create MainMenu and LobbyPrototype scenes, add them to Build Settings, and set up the project structure.\n\nContinue?",
                "Yes", "Cancel"))
            {
                CreateScenes();
                SetupBuildSettings();
                Debug.Log("[EscapeCampus] Project setup complete!");
            }
        }

        [MenuItem("EscapeCampus/Create MainMenu Scene")]
        public static void CreateMainMenuScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "MainMenu";

            // Camera
            GameObject camObj = new GameObject("MainCamera");
            Camera cam = camObj.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.05f, 0.05f, 0.1f);
            camObj.AddComponent<AudioListener>();
            camObj.transform.position = new Vector3(0, 1, -10);

            // SceneBootstrapper
            GameObject bootstrapper = new GameObject("SceneBootstrapper");
            var sb = bootstrapper.AddComponent<Core.SceneBootstrapper>();

            // Use reflection to set isMainMenu
            var field = typeof(Core.SceneBootstrapper).GetField("isMainMenu",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(sb, true);
            }

            string scenePath = "Assets/Scenes/MainMenu.unity";
            EnsureDirectory("Assets/Scenes");
            EditorSceneManager.SaveScene(scene, scenePath);
            Debug.Log($"[EscapeCampus] MainMenu scene created at {scenePath}");
        }

        [MenuItem("EscapeCampus/Create LobbyPrototype Scene")]
        public static void CreateLobbyPrototypeScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "LobbyPrototype";

            // Directional Light
            GameObject lightObj = new GameObject("DirectionalLight");
            Light light = lightObj.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1f;
            light.color = new Color(1f, 0.95f, 0.9f);
            lightObj.transform.rotation = Quaternion.Euler(50, -30, 0);

            // Camera (will be replaced by player camera)
            GameObject camObj = new GameObject("MainCamera");
            camObj.tag = "MainCamera";
            Camera cam = camObj.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.02f, 0.02f, 0.05f);
            camObj.AddComponent<AudioListener>();
            camObj.transform.position = new Vector3(0, 2, 0);

            // Player
            GameObject playerObj = new GameObject("Player");
            playerObj.tag = "Player";
            playerObj.transform.position = new Vector3(0, 1, 0);

            // CharacterController
            CharacterController cc = playerObj.AddComponent<CharacterController>();
            cc.height = 2f;
            cc.center = new Vector3(0, 1, 0);
            cc.radius = 0.4f;
            cc.slopeLimit = 45f;
            cc.stepOffset = 0.3f;

            // FirstPersonController
            playerObj.AddComponent<EscapeCampus.Player.FirstPersonController>();

            // InteractionSystem
            InteractionSystem interaction = playerObj.AddComponent<InteractionSystem>();

            // Ground Check
            GameObject groundCheck = new GameObject("GroundCheck");
            groundCheck.transform.SetParent(playerObj.transform);
            groundCheck.transform.localPosition = new Vector3(0, -0.1f, 0);

            // SceneBootstrapper
            GameObject bootstrapper = new GameObject("SceneBootstrapper");
            var sb = bootstrapper.AddComponent<Core.SceneBootstrapper>();
            var field = typeof(Core.SceneBootstrapper).GetField("isGameplay",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(sb, true);
            }

            // Lobby Prototype Builder
            GameObject lobbyBuilder = new GameObject("LobbyPrototypeBuilder");
            lobbyBuilder.AddComponent<Core.LobbyPrototypeBuilder>();

            // Game Manager
            GameObject gmObj = new GameObject("GameManager");
            gmObj.AddComponent<Core.GameManager>();

            string scenePath = "Assets/Scenes/LobbyPrototype.unity";
            EnsureDirectory("Assets/Scenes");
            EditorSceneManager.SaveScene(scene, scenePath);
            Debug.Log($"[EscapeCampus] LobbyPrototype scene created at {scenePath}");
        }

        private static void CreateScenes()
        {
            CreateMainMenuScene();
            CreateLobbyPrototypeScene();
        }

        private static void SetupBuildSettings()
        {
            EditorBuildSettingsScene[] scenes = new EditorBuildSettingsScene[]
            {
                new EditorBuildSettingsScene("Assets/Scenes/MainMenu.unity", true),
                new EditorBuildSettingsScene("Assets/Scenes/LobbyPrototype.unity", true),
            };

            EditorBuildSettings.scenes = scenes;
            Debug.Log("[EscapeCampus] Build settings updated with MainMenu and LobbyPrototype scenes.");
        }

        private static void EnsureDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
#endif
