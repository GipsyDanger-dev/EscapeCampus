using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using EscapeCampus.Documents;
using EscapeCampus.Evidence;

namespace EscapeCampus.UI
{
    public class InvestigationJournal : MonoBehaviour
    {
        [SerializeField] private GameObject journalPanel;
        [SerializeField] private KeyCode openKey = KeyCode.J;

        [Header("Tabs")]
        [SerializeField] private Button documentsTabButton;
        [SerializeField] private Button evidenceTabButton;
        [SerializeField] private GameObject documentsTabContent;
        [SerializeField] private GameObject evidenceTabContent;

        [Header("Document List")]
        [SerializeField] private Transform documentListParent;
        [SerializeField] private GameObject documentEntryPrefab;

        [Header("Evidence List")]
        [SerializeField] private Transform evidenceListParent;
        [SerializeField] private GameObject evidenceEntryPrefab;

        [Header("Detail Panel")]
        [SerializeField] private GameObject detailPanel;
        [SerializeField] private Text detailTitle;
        [SerializeField] private Text detailContent;
        [SerializeField] private Text detailMeta;

        private bool isOpen;
        private int currentTab; // 0 = documents, 1 = evidence
        private List<GameObject> spawnedEntries = new List<GameObject>();

        public bool IsOpen => isOpen;

        public event System.Action OnJournalOpened;
        public event System.Action OnJournalClosed;

        private void Awake()
        {
            if (journalPanel != null)
            {
                journalPanel.SetActive(false);
            }

            if (detailPanel != null)
            {
                detailPanel.SetActive(false);
            }
        }

        private void Start()
        {
            if (documentsTabButton != null)
            {
                documentsTabButton.onClick.AddListener(() => SwitchTab(0));
            }

            if (evidenceTabButton != null)
            {
                evidenceTabButton.onClick.AddListener(() => SwitchTab(1));
            }

            if (DocumentManager.Instance != null)
            {
                DocumentManager.Instance.OnDocumentCollected += OnDocumentCollected;
            }

            if (EvidenceManager.Instance != null)
            {
                EvidenceManager.Instance.OnEvidenceCollected += OnEvidenceCollected;
            }
        }

        private void OnDestroy()
        {
            if (DocumentManager.Instance != null)
            {
                DocumentManager.Instance.OnDocumentCollected -= OnDocumentCollected;
            }

            if (EvidenceManager.Instance != null)
            {
                EvidenceManager.Instance.OnEvidenceCollected -= OnEvidenceCollected;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(openKey))
            {
                if (isOpen)
                    CloseJournal();
                else
                    OpenJournal();
            }

            if (isOpen && Input.GetKeyDown(KeyCode.Escape))
            {
                CloseJournal();
            }
        }

        public void OpenJournal()
        {
            if (isOpen) return;

            isOpen = true;
            currentTab = 0;

            if (journalPanel != null)
            {
                journalPanel.SetActive(true);
            }

            SetPlayerMovement(false);
            RefreshCurrentTab();
            OnJournalOpened?.Invoke();

            Debug.Log("[Journal] Opened investigation journal.");
        }

        public void CloseJournal()
        {
            if (!isOpen) return;

            isOpen = false;

            if (journalPanel != null)
            {
                journalPanel.SetActive(false);
            }

            if (detailPanel != null)
            {
                detailPanel.SetActive(false);
            }

            SetPlayerMovement(true);
            OnJournalClosed?.Invoke();

            Debug.Log("[Journal] Closed investigation journal.");
        }

        private void SwitchTab(int tab)
        {
            currentTab = tab;

            if (documentsTabContent != null)
            {
                documentsTabContent.SetActive(tab == 0);
            }

            if (evidenceTabContent != null)
            {
                evidenceTabContent.SetActive(tab == 1);
            }

            if (documentsTabButton != null)
            {
                var colors = documentsTabButton.colors;
                colors.normalColor = tab == 0 ? Color.white : new Color(0.7f, 0.7f, 0.7f);
                documentsTabButton.colors = colors;
            }

            if (evidenceTabButton != null)
            {
                var colors = evidenceTabButton.colors;
                colors.normalColor = tab == 1 ? Color.white : new Color(0.7f, 0.7f, 0.7f);
                evidenceTabButton.colors = colors;
            }

            if (detailPanel != null)
            {
                detailPanel.SetActive(false);
            }

            RefreshCurrentTab();
        }

        private void RefreshCurrentTab()
        {
            ClearSpawnedEntries();

            if (currentTab == 0)
            {
                RefreshDocumentsTab();
            }
            else
            {
                RefreshEvidenceTab();
            }
        }

        private void RefreshDocumentsTab()
        {
            DocumentManager manager = DocumentManager.Instance;
            if (manager == null || documentListParent == null) return;

            foreach (DocumentData doc in manager.CollectedDocuments)
            {
                CreateDocumentEntry(doc);
            }

            if (manager.TotalCollected == 0)
            {
                CreateEmptyMessage(documentListParent, "No documents collected yet.");
            }
        }

        private void RefreshEvidenceTab()
        {
            EvidenceManager manager = EvidenceManager.Instance;
            if (manager == null || evidenceListParent == null) return;

            foreach (EvidenceData evidence in manager.CollectedEvidence)
            {
                CreateEvidenceEntry(evidence);
            }

            if (manager.TotalCollected == 0)
            {
                CreateEmptyMessage(evidenceListParent, "No evidence collected yet.");
            }
        }

        private void CreateDocumentEntry(DocumentData document)
        {
            GameObject entry;

            if (documentEntryPrefab != null)
            {
                entry = Instantiate(documentEntryPrefab, documentListParent);
            }
            else
            {
                entry = CreateDefaultEntry(documentListParent, document.title, document.category.ToString(), document.isCritical);
            }

            Button button = entry.GetComponent<Button>();
            if (button == null)
            {
                button = entry.AddComponent<Button>();
            }

            DocumentData capturedDoc = document;
            button.onClick.AddListener(() => ShowDocumentDetail(capturedDoc));

            spawnedEntries.Add(entry);
        }

        private void CreateEvidenceEntry(EvidenceData evidence)
        {
            GameObject entry;

            if (evidenceEntryPrefab != null)
            {
                entry = Instantiate(evidenceEntryPrefab, evidenceListParent);
            }
            else
            {
                entry = CreateDefaultEntry(evidenceListParent, evidence.title, "Evidence", evidence.isCritical);
            }

            Button button = entry.GetComponent<Button>();
            if (button == null)
            {
                button = entry.AddComponent<Button>();
            }

            EvidenceData capturedEvidence = evidence;
            button.onClick.AddListener(() => ShowEvidenceDetail(capturedEvidence));

            spawnedEntries.Add(entry);
        }

        private GameObject CreateDefaultEntry(Transform parent, string title, string subtitle, bool isCritical)
        {
            GameObject entry = new GameObject($"Entry_{title}");
            entry.transform.SetParent(parent, false);

            Image bg = entry.AddComponent<Image>();
            bg.color = isCritical ? new Color(0.8f, 0.2f, 0.2f, 0.3f) : new Color(0.2f, 0.2f, 0.3f, 0.8f);

            RectTransform rect = entry.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(0, 50);

            LayoutElement layout = entry.AddComponent<LayoutElement>();
            layout.minHeight = 50;
            layout.preferredHeight = 50;

            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(entry.transform, false);

            Text text = textObj.AddComponent<Text>();
            string criticalTag = isCritical ? " [CRITICAL]" : "";
            text.text = $"{title}{criticalTag}\n<size=12>{subtitle}</size>";
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 16;
            text.color = Color.white;

            RectTransform textRect = textObj.GetComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = new Vector2(10, 5);
            textRect.offsetMax = new Vector2(-10, -5);

            return entry;
        }

        private void CreateEmptyMessage(Transform parent, string message)
        {
            GameObject msg = new GameObject("EmptyMessage");
            msg.transform.SetParent(parent, false);

            Text text = msg.AddComponent<Text>();
            text.text = message;
            text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            text.fontSize = 18;
            text.color = new Color(0.6f, 0.6f, 0.6f);
            text.alignment = TextAnchor.MiddleCenter;

            spawnedEntries.Add(msg);
        }

        private void ShowDocumentDetail(DocumentData document)
        {
            if (detailPanel == null) return;

            detailPanel.SetActive(true);

            if (detailTitle != null)
            {
                detailTitle.text = document.title;
            }

            if (detailContent != null)
            {
                detailContent.text = document.content;
            }

            if (detailMeta != null)
            {
                string criticalTag = document.isCritical ? " [CRITICAL]" : "";
                detailMeta.text = $"Category: {document.category}{criticalTag}";
            }
        }

        private void ShowEvidenceDetail(EvidenceData evidence)
        {
            if (detailPanel == null) return;

            detailPanel.SetActive(true);

            if (detailTitle != null)
            {
                detailTitle.text = evidence.title;
            }

            if (detailContent != null)
            {
                string content = evidence.description;

                if (evidence.relatedDocuments != null && evidence.relatedDocuments.Count > 0)
                {
                    content += "\n\n<b>Related Documents:</b>";
                    foreach (var doc in evidence.relatedDocuments)
                    {
                        if (doc != null)
                        {
                            content += $"\n• {doc.title}";
                        }
                    }
                }

                detailContent.text = content;
            }

            if (detailMeta != null)
            {
                string criticalTag = evidence.isCritical ? " [CRITICAL]" : "";
                detailMeta.text = $"Type: Evidence{criticalTag}";
            }
        }

        private void ClearSpawnedEntries()
        {
            foreach (GameObject entry in spawnedEntries)
            {
                if (entry != null)
                {
                    Destroy(entry);
                }
            }
            spawnedEntries.Clear();
        }

        private void OnDocumentCollected(DocumentData document)
        {
            if (isOpen)
            {
                RefreshCurrentTab();
            }
        }

        private void OnEvidenceCollected(EvidenceData evidence)
        {
            if (isOpen)
            {
                RefreshCurrentTab();
            }
        }

        private void SetPlayerMovement(bool enabled)
        {
            var playerController = FindObjectOfType<EscapeCampus.Player.FirstPersonController>();
            if (playerController != null)
            {
                playerController.enabled = enabled;
            }

            var interactionSystem = FindObjectOfType<EscapeCampus.Interaction.InteractionSystem>();
            if (interactionSystem != null)
            {
                interactionSystem.enabled = enabled;
            }

            if (enabled)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }
}
