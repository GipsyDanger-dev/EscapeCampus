using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using EscapeCampus.Documents;
using EscapeCampus.Evidence;

namespace EscapeCampus.Save
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        private const string SAVE_FOLDER = "Saves";
        private const string MANUAL_SAVE_FILE = "manual_save.json";
        private const string AUTOSAVE_FILE = "autosave.json";

        private float playTimeSeconds;
        private bool isPlaying;

        public event Action<string> OnGameSaved;
        public event Action<string> OnGameLoaded;
        public event Action<string> OnSaveDeleted;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            EnsureSaveDirectory();
        }

        private void Start()
        {
            isPlaying = true;

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
            if (isPlaying)
            {
                playTimeSeconds += Time.deltaTime;
            }
        }

        // ============================================
        // PUBLIC API
        // ============================================

        public void SaveGame(bool isAutosave = false)
        {
            try
            {
                SaveData data = CreateSaveData(isAutosave);
                string json = JsonUtility.ToJson(data, true);
                string filePath = GetSaveFilePath(isAutosave);

                File.WriteAllText(filePath, json);

                string slotName = isAutosave ? "Autosave" : "Manual Save";
                Debug.Log($"[SaveManager] {slotName} saved to: {filePath}");
                OnGameSaved?.Invoke(slotName);
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Save failed: {e.Message}");
            }
        }

        public bool LoadGame(bool fromAutosave = false)
        {
            try
            {
                string filePath = GetSaveFilePath(fromAutosave);

                if (!File.Exists(filePath))
                {
                    Debug.LogWarning($"[SaveManager] No save file found at: {filePath}");
                    return false;
                }

                string json = File.ReadAllText(filePath);
                SaveData data = JsonUtility.FromJson<SaveData>(json);

                if (data == null)
                {
                    Debug.LogError("[SaveManager] Failed to parse save data.");
                    return false;
                }

                ApplySaveData(data);

                string slotName = fromAutosave ? "Autosave" : "Manual Save";
                Debug.Log($"[SaveManager] {slotName} loaded successfully.");
                OnGameLoaded?.Invoke(slotName);

                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Load failed: {e.Message}");
                return false;
            }
        }

        public void DeleteSave(bool autosave = false)
        {
            try
            {
                string filePath = GetSaveFilePath(autosave);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    string slotName = autosave ? "Autosave" : "Manual Save";
                    Debug.Log($"[SaveManager] {slotName} deleted.");
                    OnSaveDeleted?.Invoke(slotName);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[SaveManager] Delete failed: {e.Message}");
            }
        }

        public bool HasSave(bool autosave = false)
        {
            string filePath = GetSaveFilePath(autosave);
            return File.Exists(filePath);
        }

        public SaveData GetCurrentSaveData(bool fromAutosave = false)
        {
            string filePath = GetSaveFilePath(fromAutosave);

            if (!File.Exists(filePath))
                return null;

            string json = File.ReadAllText(filePath);
            return JsonUtility.FromJson<SaveData>(json);
        }

        // ============================================
        // SAVE DATA CREATION
        // ============================================

        private SaveData CreateSaveData(bool isAutosave)
        {
            SaveData data = new SaveData();
            data.saveName = isAutosave ? "Autosave" : "Manual Save";
            data.timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            data.playTimeSeconds = playTimeSeconds;
            data.currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            // Player
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                data.player.SetPosition(player.transform.position);
                data.player.SetRotation(player.transform.rotation);
            }

            // Documents
            if (DocumentManager.Instance != null)
            {
                foreach (DocumentData doc in DocumentManager.Instance.CollectedDocuments)
                {
                    if (doc != null)
                    {
                        data.collectedDocumentIDs.Add(doc.documentID);
                    }
                }
            }

            // Evidence
            if (EvidenceManager.Instance != null)
            {
                foreach (EvidenceData ev in EvidenceManager.Instance.CollectedEvidence)
                {
                    if (ev != null)
                    {
                        data.collectedEvidenceIDs.Add(ev.evidenceID);
                    }
                }
            }

            return data;
        }

        // ============================================
        // SAVE DATA APPLICATION
        // ============================================

        private void ApplySaveData(SaveData data)
        {
            playTimeSeconds = data.playTimeSeconds;

            // Player
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                CharacterController cc = player.GetComponent<CharacterController>();
                if (cc != null)
                {
                    cc.enabled = false;
                }

                player.transform.position = data.player.GetPosition();
                player.transform.rotation = data.player.GetRotation();

                if (cc != null)
                {
                    cc.enabled = true;
                }
            }

            // Documents - reload from ScriptableObjects
            if (DocumentManager.Instance != null && data.collectedDocumentIDs.Count > 0)
            {
                LoadDocumentsIntoManager(data.collectedDocumentIDs);
            }

            // Evidence - reload from ScriptableObjects
            if (EvidenceManager.Instance != null && data.collectedEvidenceIDs.Count > 0)
            {
                LoadEvidenceIntoManager(data.collectedEvidenceIDs);
            }
        }

        private void LoadDocumentsIntoManager(List<string> documentIDs)
        {
            DocumentData[] allDocuments = Resources.LoadAll<DocumentData>("");

            if (allDocuments.Length == 0)
            {
                // Try loading from known paths
                string[] knownPaths = {
                    "Assets/ScriptableObjects/Documents/DOC_001_DeanMemo.asset",
                    "Assets/ScriptableObjects/Documents/DOC_002_StudentComplaint.asset",
                    "Assets/ScriptableObjects/Documents/DOC_003_LibraryLog.asset",
                    "Assets/ScriptableObjects/Documents/DOC_004_MaintenanceRequest.asset",
                    "Assets/ScriptableObjects/Documents/DOC_005_ResearchNotes.asset"
                };

                List<DocumentData> foundDocs = new List<DocumentData>();
                foreach (string path in knownPaths)
                {
                    DocumentData doc = LoadDocumentFromPath(path);
                    if (doc != null)
                    {
                        foundDocs.Add(doc);
                    }
                }
                allDocuments = foundDocs.ToArray();
            }

            foreach (DocumentData doc in allDocuments)
            {
                if (doc != null && documentIDs.Contains(doc.documentID))
                {
                    DocumentManager.Instance.CollectDocument(doc);
                }
            }
        }

        private void LoadEvidenceIntoManager(List<string> evidenceIDs)
        {
            EvidenceData[] allEvidence = Resources.LoadAll<EvidenceData>("");

            if (allEvidence.Length == 0)
            {
                string[] knownPaths = {
                    "Assets/ScriptableObjects/Evidence/EV_001_StrangeLocks.asset",
                    "Assets/ScriptableObjects/Evidence/EV_002_CovertResearch.asset",
                    "Assets/ScriptableObjects/Evidence/EV_003_MissingBook.asset"
                };

                List<EvidenceData> foundEvidence = new List<EvidenceData>();
                foreach (string path in knownPaths)
                {
                    EvidenceData ev = LoadEvidenceFromPath(path);
                    if (ev != null)
                    {
                        foundEvidence.Add(ev);
                    }
                }
                allEvidence = foundEvidence.ToArray();
            }

            foreach (EvidenceData ev in allEvidence)
            {
                if (ev != null && evidenceIDs.Contains(ev.evidenceID))
                {
                    EvidenceManager.Instance.CollectEvidence(ev);
                }
            }
        }

        private DocumentData LoadDocumentFromPath(string path)
        {
#if UNITY_EDITOR
            return UnityEditor.AssetDatabase.LoadAssetAtPath<DocumentData>(path);
#else
            return null;
#endif
        }

        private EvidenceData LoadEvidenceFromPath(string path)
        {
#if UNITY_EDITOR
            return UnityEditor.AssetDatabase.LoadAssetAtPath<EvidenceData>(path);
#else
            return null;
#endif
        }

        // ============================================
        // AUTOSAVE
        // ============================================

        private void OnDocumentCollected(DocumentData document)
        {
            Autosave();
        }

        private void OnEvidenceCollected(EvidenceData evidence)
        {
            Autosave();
        }

        private void Autosave()
        {
            SaveGame(isAutosave: true);
        }

        // ============================================
        // HELPERS
        // ============================================

        private string GetSaveFilePath(bool autosave)
        {
            string fileName = autosave ? AUTOSAVE_FILE : MANUAL_SAVE_FILE;
            return Path.Combine(GetSaveDirectory(), fileName);
        }

        private string GetSaveDirectory()
        {
            return Path.Combine(Application.persistentDataPath, SAVE_FOLDER);
        }

        private void EnsureSaveDirectory()
        {
            string dir = GetSaveDirectory();
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        public float GetPlayTime()
        {
            return playTimeSeconds;
        }

        public string GetPlayTimeFormatted()
        {
            TimeSpan t = TimeSpan.FromSeconds(playTimeSeconds);
            return $"{t.Hours:D2}:{t.Minutes:D2}:{t.Seconds:D2}";
        }

        public string GetSavePath()
        {
            return GetSaveDirectory();
        }
    }
}
