using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using EscapeCampus.Player;
using EscapeCampus.Interaction;
using EscapeCampus.Documents;
using EscapeCampus.Evidence;
using EscapeCampus.Puzzle;
using EscapeCampus.Save;
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

            // Managers
            if (DocumentManager.Instance == null)
            {
                GameObject docManagerObj = new GameObject("DocumentManager");
                docManagerObj.AddComponent<DocumentManager>();
            }

            if (EvidenceManager.Instance == null)
            {
                GameObject evManagerObj = new GameObject("EvidenceManager");
                evManagerObj.AddComponent<EvidenceManager>();
            }

            if (SaveManager.Instance == null)
            {
                GameObject saveManagerObj = new GameObject("SaveManager");
                saveManagerObj.AddComponent<SaveManager>();
            }

            if (PuzzleManager.Instance == null)
            {
                GameObject puzzleManagerObj = new GameObject("PuzzleManager");
                puzzleManagerObj.AddComponent<PuzzleManager>();
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

                // Document Viewer
                CreateDocumentViewer(canvas);

                // Investigation Journal
                CreateInvestigationJournal(canvas);

                // Save UI
                CreateSaveUI(canvasObj);

                // Save Debug Tool
                CreateSaveDebugTool(canvasObj);

                // Puzzle Debug Tool
                CreatePuzzleDebugTool(canvasObj);
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

        private void CreateDocumentViewer(Canvas canvas)
        {
            // Document Viewer Panel (full screen overlay)
            GameObject viewerPanel = new GameObject("DocumentViewer");
            viewerPanel.transform.SetParent(canvas.transform, false);

            Image viewerBg = viewerPanel.AddComponent<Image>();
            viewerBg.color = new Color(0, 0, 0, 0.85f);

            RectTransform viewerRect = viewerPanel.GetComponent<RectTransform>();
            viewerRect.anchorMin = Vector2.zero;
            viewerRect.anchorMax = Vector2.one;
            viewerRect.sizeDelta = Vector2.zero;

            // Content area
            GameObject contentArea = new GameObject("ContentArea");
            contentArea.transform.SetParent(viewerPanel.transform, false);

            Image contentBg = contentArea.AddComponent<Image>();
            contentBg.color = new Color(0.1f, 0.1f, 0.15f);

            RectTransform contentRect = contentArea.GetComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0.15f, 0.1f);
            contentRect.anchorMax = new Vector2(0.85f, 0.85f);
            contentRect.sizeDelta = Vector2.zero;

            // ScrollRect
            ScrollRect scrollRect = contentArea.AddComponent<ScrollRect>();
            scrollRect.horizontal = false;

            // Viewport
            GameObject viewport = new GameObject("Viewport");
            viewport.transform.SetParent(contentArea.transform, false);

            RectTransform viewportRect = viewport.AddComponent<RectTransform>();
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.sizeDelta = Vector2.zero;
            viewportRect.offsetMin = new Vector2(10, 10);
            viewportRect.offsetMax = new Vector2(-10, -10);

            Mask viewportMask = viewport.AddComponent<Mask>();
            viewportMask.showMaskGraphic = false;

            Image viewportBg = viewport.AddComponent<Image>();
            viewportBg.color = new Color(1, 1, 1, 0.01f);

            // Content
            GameObject content = new GameObject("Content");
            content.transform.SetParent(viewport.transform, false);

            RectTransform contentContentRect = content.AddComponent<RectTransform>();
            contentContentRect.anchorMin = new Vector2(0, 1);
            contentContentRect.anchorMax = new Vector2(1, 1);
            contentContentRect.pivot = new Vector2(0.5f, 1);
            contentContentRect.sizeDelta = new Vector2(0, 800);

            ContentSizeFitter fitter = content.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            VerticalLayoutGroup layout = content.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 20;
            layout.padding = new RectOffset(20, 20, 20, 20);
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            scrollRect.viewport = viewportRect;
            scrollRect.content = contentContentRect;

            // Title Text
            GameObject titleObj = new GameObject("TitleText");
            titleObj.transform.SetParent(content.transform, false);

            Text titleText = titleObj.AddComponent<Text>();
            titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            titleText.fontSize = 32;
            titleText.color = Color.white;
            titleText.alignment = TextAnchor.MiddleCenter;

            LayoutElement titleLayout = titleObj.AddComponent<LayoutElement>();
            titleLayout.preferredHeight = 50;

            // Category Text
            GameObject categoryObj = new GameObject("CategoryText");
            categoryObj.transform.SetParent(content.transform, false);

            Text categoryText = categoryObj.AddComponent<Text>();
            categoryText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            categoryText.fontSize = 18;
            categoryText.color = new Color(0.7f, 0.7f, 0.7f);
            categoryText.alignment = TextAnchor.MiddleCenter;

            LayoutElement catLayout = categoryObj.AddComponent<LayoutElement>();
            catLayout.preferredHeight = 30;

            // Separator
            GameObject sep = new GameObject("Separator");
            sep.transform.SetParent(content.transform, false);
            Image sepImg = sep.AddComponent<Image>();
            sepImg.color = new Color(0.3f, 0.3f, 0.4f);
            LayoutElement sepLayout = sep.AddComponent<LayoutElement>();
            sepLayout.preferredHeight = 2;

            // Content Text
            GameObject contentTextObj = new GameObject("ContentText");
            contentTextObj.transform.SetParent(content.transform, false);

            Text contentText = contentTextObj.AddComponent<Text>();
            contentText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            contentText.fontSize = 20;
            contentText.color = Color.white;
            contentText.alignment = TextAnchor.UpperLeft;

            LayoutElement contentTextLayout = contentTextObj.AddComponent<LayoutElement>();
            contentTextLayout.flexibleHeight = 1;

            // Close instruction
            GameObject closeObj = new GameObject("CloseText");
            closeObj.transform.SetParent(viewerPanel.transform, false);

            Text closeText = closeObj.AddComponent<Text>();
            closeText.text = "Press ESC to close";
            closeText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            closeText.fontSize = 16;
            closeText.color = new Color(0.5f, 0.5f, 0.5f);
            closeText.alignment = TextAnchor.MiddleCenter;

            RectTransform closeRect = closeObj.GetComponent<RectTransform>();
            closeRect.anchorMin = new Vector2(0.5f, 0.05f);
            closeRect.anchorMax = new Vector2(0.5f, 0.05f);
            closeRect.sizeDelta = new Vector2(200, 30);

            // DocumentViewer component
            DocumentViewer viewer = viewerPanel.AddComponent<DocumentViewer>();

            // Use reflection to set serialized fields
            var viewerType = typeof(DocumentViewer);
            SetSerializedField(viewerType, viewer, "viewerPanel", viewerPanel);
            SetSerializedField(viewerType, viewer, "titleText", titleText);
            SetSerializedField(viewerType, viewer, "contentText", contentText);
            SetSerializedField(viewerType, viewer, "categoryText", categoryText);
            SetSerializedField(viewerType, viewer, "scrollRect", scrollRect);

            viewerPanel.SetActive(false);
        }

        private void CreateInvestigationJournal(Canvas canvas)
        {
            // Journal Panel
            GameObject journalPanel = new GameObject("InvestigationJournal");
            journalPanel.transform.SetParent(canvas.transform, false);

            Image journalBg = journalPanel.AddComponent<Image>();
            journalBg.color = new Color(0, 0, 0, 0.9f);

            RectTransform journalRect = journalPanel.GetComponent<RectTransform>();
            journalRect.anchorMin = Vector2.zero;
            journalRect.anchorMax = Vector2.one;
            journalRect.sizeDelta = Vector2.zero;

            // Main container
            GameObject container = new GameObject("Container");
            container.transform.SetParent(journalPanel.transform, false);

            Image containerBg = container.AddComponent<Image>();
            containerBg.color = new Color(0.12f, 0.12f, 0.18f);

            RectTransform containerRect = container.GetComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(0.1f, 0.08f);
            containerRect.anchorMax = new Vector2(0.9f, 0.92f);
            containerRect.sizeDelta = Vector2.zero;

            // Title
            GameObject titleObj = new GameObject("JournalTitle");
            titleObj.transform.SetParent(container.transform, false);

            Text titleText = titleObj.AddComponent<Text>();
            titleText.text = "INVESTIGATION JOURNAL";
            titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            titleText.fontSize = 28;
            titleText.color = Color.white;
            titleText.alignment = TextAnchor.MiddleCenter;

            RectTransform titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0, 0.9f);
            titleRect.anchorMax = new Vector2(1, 1);
            titleRect.sizeDelta = Vector2.zero;

            // Tab buttons
            GameObject tabArea = new GameObject("TabArea");
            tabArea.transform.SetParent(container.transform, false);

            RectTransform tabAreaRect = tabArea.AddComponent<RectTransform>();
            tabAreaRect.anchorMin = new Vector2(0, 0.83f);
            tabAreaRect.anchorMax = new Vector2(1, 0.9f);
            tabAreaRect.sizeDelta = Vector2.zero;

            HorizontalLayoutGroup tabLayout = tabArea.AddComponent<HorizontalLayoutGroup>();
            tabLayout.spacing = 10;
            tabLayout.padding = new RectOffset(10, 10, 5, 5);
            tabLayout.childForceExpandWidth = true;

            // Documents Tab Button
            Button docsTabBtn = CreateTabButton(tabArea.transform, "DocumentsTab", "DOCUMENTS");

            // Evidence Tab Button
            Button evTabBtn = CreateTabButton(tabArea.transform, "EvidenceTab", "EVIDENCE");

            // Tab content area
            GameObject tabContent = new GameObject("TabContent");
            tabContent.transform.SetParent(container.transform, false);

            RectTransform tabContentRect = tabContent.AddComponent<RectTransform>();
            tabContentRect.anchorMin = new Vector2(0, 0.05f);
            tabContentRect.anchorMax = new Vector2(1, 0.83f);
            tabContentRect.sizeDelta = Vector2.zero;

            // Documents tab content
            GameObject docsContent = CreateTabContent(tabContent.transform, "DocumentsContent");
            CreateScrollableList(docsContent.transform, "DocumentList");

            // Evidence tab content
            GameObject evContent = CreateTabContent(tabContent.transform, "EvidenceContent");
            evContent.SetActive(false);
            CreateScrollableList(evContent.transform, "EvidenceList");

            // Detail panel
            GameObject detailPanel = CreateDetailPanel(container.transform);

            // Close instruction
            GameObject closeObj = new GameObject("CloseText");
            closeObj.transform.SetParent(container.transform, false);

            Text closeText = closeObj.AddComponent<Text>();
            closeText.text = "Press ESC or J to close";
            closeText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            closeText.fontSize = 14;
            closeText.color = new Color(0.4f, 0.4f, 0.4f);
            closeText.alignment = TextAnchor.MiddleCenter;

            RectTransform closeRect = closeObj.GetComponent<RectTransform>();
            closeRect.anchorMin = new Vector2(0.5f, 0);
            closeRect.anchorMax = new Vector2(0.5f, 0);
            closeRect.sizeDelta = new Vector2(200, 30);

            // InvestigationJournal component
            InvestigationJournal journal = journalPanel.AddComponent<InvestigationJournal>();

            var journalType = typeof(InvestigationJournal);
            SetSerializedField(journalType, journal, "journalPanel", journalPanel);
            SetSerializedField(journalType, journal, "documentsTabButton", docsTabBtn);
            SetSerializedField(journalType, journal, "evidenceTabButton", evTabBtn);
            SetSerializedField(journalType, journal, "documentsTabContent", docsContent);
            SetSerializedField(journalType, journal, "evidenceTabContent", evContent);
            SetSerializedField(journalType, journal, "documentListParent", docsContent.transform.Find("DocumentList/Viewport/Content"));
            SetSerializedField(journalType, journal, "evidenceListParent", evContent.transform.Find("EvidenceList/Viewport/Content"));
            SetSerializedField(journalType, journal, "detailPanel", detailPanel);
            SetSerializedField(journalType, journal, "detailTitle", detailPanel.transform.Find("DetailTitle")?.GetComponent<Text>());
            SetSerializedField(journalType, journal, "detailContent", detailPanel.transform.Find("DetailContent/Viewport/DetailContentText")?.GetComponent<Text>());
            SetSerializedField(journalType, journal, "detailMeta", detailPanel.transform.Find("DetailMeta")?.GetComponent<Text>());

            journalPanel.SetActive(false);
        }

        private Button CreateTabButton(Transform parent, string name, string text)
        {
            GameObject btnObj = new GameObject(name);
            btnObj.transform.SetParent(parent, false);

            Image btnImage = btnObj.AddComponent<Image>();
            btnImage.color = new Color(0.2f, 0.2f, 0.35f);

            Button btn = btnObj.AddComponent<Button>();

            Text btnText = btnObj.AddComponent<Text>();
            btnText.text = text;
            btnText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            btnText.fontSize = 20;
            btnText.color = Color.white;
            btnText.alignment = TextAnchor.MiddleCenter;

            LayoutElement layout = btnObj.AddComponent<LayoutElement>();
            layout.preferredHeight = 40;

            return btn;
        }

        private GameObject CreateTabContent(Transform parent, string name)
        {
            GameObject content = new GameObject(name);
            content.transform.SetParent(parent, false);

            RectTransform rect = content.AddComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.sizeDelta = Vector2.zero;

            return content;
        }

        private void CreateScrollableList(Transform parent, string name)
        {
            GameObject listObj = new GameObject(name);
            listObj.transform.SetParent(parent, false);

            RectTransform listRect = listObj.AddComponent<RectTransform>();
            listRect.anchorMin = Vector2.zero;
            listRect.anchorMax = Vector2.one;
            listRect.sizeDelta = Vector2.zero;

            ScrollRect scrollRect = listObj.AddComponent<ScrollRect>();
            scrollRect.horizontal = false;

            // Viewport
            GameObject viewport = new GameObject("Viewport");
            viewport.transform.SetParent(listObj.transform, false);

            RectTransform viewportRect = viewport.AddComponent<RectTransform>();
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.sizeDelta = Vector2.zero;

            Mask mask = viewport.AddComponent<Mask>();
            mask.showMaskGraphic = false;

            Image viewportBg = viewport.AddComponent<Image>();
            viewportBg.color = new Color(1, 1, 1, 0.01f);

            // Content
            GameObject content = new GameObject("Content");
            content.transform.SetParent(viewport.transform, false);

            RectTransform contentRect = content.AddComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0, 1);
            contentRect.anchorMax = new Vector2(1, 1);
            contentRect.pivot = new Vector2(0.5f, 1);
            contentRect.sizeDelta = new Vector2(0, 0);

            ContentSizeFitter fitter = content.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            VerticalLayoutGroup layout = content.AddComponent<VerticalLayoutGroup>();
            layout.spacing = 5;
            layout.padding = new RectOffset(10, 10, 10, 10);
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            scrollRect.viewport = viewportRect;
            scrollRect.content = contentRect;
        }

        private GameObject CreateDetailPanel(Transform parent)
        {
            GameObject panel = new GameObject("DetailPanel");
            panel.transform.SetParent(parent, false);

            Image bg = panel.AddComponent<Image>();
            bg.color = new Color(0.15f, 0.15f, 0.22f);

            RectTransform rect = panel.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.35f, 0.05f);
            rect.anchorMax = new Vector2(0.95f, 0.83f);
            rect.sizeDelta = Vector2.zero;

            // Title
            GameObject titleObj = new GameObject("DetailTitle");
            titleObj.transform.SetParent(panel.transform, false);

            Text titleText = titleObj.AddComponent<Text>();
            titleText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            titleText.fontSize = 24;
            titleText.color = Color.white;
            titleText.alignment = TextAnchor.MiddleCenter;

            RectTransform titleRect = titleObj.GetComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0, 0.9f);
            titleRect.anchorMax = new Vector2(1, 1);
            titleRect.sizeDelta = Vector2.zero;

            // Content scroll area
            GameObject contentArea = new GameObject("DetailContent");
            contentArea.transform.SetParent(panel.transform, false);

            RectTransform contentRect = contentArea.AddComponent<RectTransform>();
            contentRect.anchorMin = new Vector2(0, 0.1f);
            contentRect.anchorMax = new Vector2(1, 0.9f);
            contentRect.sizeDelta = Vector2.zero;

            ScrollRect scrollRect = contentArea.AddComponent<ScrollRect>();
            scrollRect.horizontal = false;

            GameObject viewport = new GameObject("Viewport");
            viewport.transform.SetParent(contentArea.transform, false);

            RectTransform viewportRect = viewport.AddComponent<RectTransform>();
            viewportRect.anchorMin = Vector2.zero;
            viewportRect.anchorMax = Vector2.one;
            viewportRect.sizeDelta = Vector2.zero;

            Mask mask = viewport.AddComponent<Mask>();
            mask.showMaskGraphic = false;

            Image viewportBg = viewport.AddComponent<Image>();
            viewportBg.color = new Color(1, 1, 1, 0.01f);

            GameObject content = new GameObject("DetailContentText");
            content.transform.SetParent(viewport.transform, false);

            RectTransform contentContentRect = content.AddComponent<RectTransform>();
            contentContentRect.anchorMin = new Vector2(0, 1);
            contentContentRect.anchorMax = new Vector2(1, 1);
            contentContentRect.pivot = new Vector2(0.5f, 1);
            contentContentRect.sizeDelta = new Vector2(0, 0);

            ContentSizeFitter fitter = content.AddComponent<ContentSizeFitter>();
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            Text contentText = content.AddComponent<Text>();
            contentText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            contentText.fontSize = 18;
            contentText.color = Color.white;

            scrollRect.viewport = viewportRect;
            scrollRect.content = contentContentRect;

            // Meta text
            GameObject metaObj = new GameObject("DetailMeta");
            metaObj.transform.SetParent(panel.transform, false);

            Text metaText = metaObj.AddComponent<Text>();
            metaText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            metaText.fontSize = 14;
            metaText.color = new Color(0.6f, 0.6f, 0.6f);

            RectTransform metaRect = metaObj.GetComponent<RectTransform>();
            metaRect.anchorMin = new Vector2(0, 0);
            metaRect.anchorMax = new Vector2(1, 0.1f);
            metaRect.sizeDelta = Vector2.zero;

            panel.SetActive(false);
            return panel;
        }

        private void SetSerializedField(System.Type type, object target, string fieldName, object value)
        {
            var field = type.GetField(fieldName,
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (field != null)
            {
                field.SetValue(target, value);
            }
        }

        private void CreateSaveUI(GameObject canvasObj)
        {
            // Message panel at top center
            GameObject msgPanel = new GameObject("SaveMessagePanel");
            msgPanel.transform.SetParent(canvasObj.transform, false);

            Image msgBg = msgPanel.AddComponent<Image>();
            msgBg.color = new Color(0, 0, 0, 0.8f);

            RectTransform msgRect = msgPanel.GetComponent<RectTransform>();
            msgRect.anchorMin = new Vector2(0.35f, 0.85f);
            msgRect.anchorMax = new Vector2(0.65f, 0.95f);
            msgRect.sizeDelta = Vector2.zero;

            GameObject msgTextObj = new GameObject("MessageText");
            msgTextObj.transform.SetParent(msgPanel.transform, false);

            Text msgText = msgTextObj.AddComponent<Text>();
            msgText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            msgText.fontSize = 24;
            msgText.color = Color.white;
            msgText.alignment = TextAnchor.MiddleCenter;

            RectTransform msgTextRect = msgTextObj.GetComponent<RectTransform>();
            msgTextRect.anchorMin = Vector2.zero;
            msgTextRect.anchorMax = Vector2.one;
            msgTextRect.sizeDelta = Vector2.zero;

            SaveUI saveUI = canvasObj.AddComponent<SaveUI>();
            var saveUIType = typeof(SaveUI);
            SetSerializedField(saveUIType, saveUI, "messagePanel", msgPanel);
            SetSerializedField(saveUIType, saveUI, "messageText", msgText);

            msgPanel.SetActive(false);
        }

        private void CreateSaveDebugTool(GameObject canvasObj)
        {
            // Debug panel at top-left
            GameObject debugPanel = new GameObject("SaveDebugPanel");
            debugPanel.transform.SetParent(canvasObj.transform, false);

            Image debugBg = debugPanel.AddComponent<Image>();
            debugBg.color = new Color(0, 0, 0, 0.85f);

            RectTransform debugRect = debugPanel.GetComponent<RectTransform>();
            debugRect.anchorMin = new Vector2(0, 0.3f);
            debugRect.anchorMax = new Vector2(0.25f, 0.95f);
            debugRect.sizeDelta = Vector2.zero;

            GameObject debugTextObj = new GameObject("DebugText");
            debugTextObj.transform.SetParent(debugPanel.transform, false);

            Text debugText = debugTextObj.AddComponent<Text>();
            debugText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            debugText.fontSize = 14;
            debugText.color = Color.green;

            RectTransform debugTextRect = debugTextObj.GetComponent<RectTransform>();
            debugTextRect.anchorMin = Vector2.zero;
            debugTextRect.anchorMax = Vector2.one;
            debugTextRect.offsetMin = new Vector2(10, 10);
            debugTextRect.offsetMax = new Vector2(-10, -10);

            SaveDebugTool debugTool = canvasObj.AddComponent<SaveDebugTool>();
            var debugType = typeof(SaveDebugTool);
            SetSerializedField(debugType, debugTool, "debugPanel", debugPanel);
            SetSerializedField(debugType, debugTool, "debugText", debugText);

            debugPanel.SetActive(false);
        }

        private void CreatePuzzleDebugTool(GameObject canvasObj)
        {
            PuzzleDebugTool puzzleDebug = canvasObj.AddComponent<PuzzleDebugTool>();
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
