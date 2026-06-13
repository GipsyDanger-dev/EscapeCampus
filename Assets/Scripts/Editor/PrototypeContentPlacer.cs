#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using EscapeCampus.Documents;
using EscapeCampus.Evidence;

namespace EscapeCampus.Editor
{
    public class PrototypeContentPlacer : EditorWindow
    {
        [MenuItem("EscapeCampus/Assign Prototype Content to Pickups")]
        public static void AssignContent()
        {
            if (EditorUtility.DisplayDialog("Assign Content",
                "This will assign DocumentData and EvidenceData ScriptableObjects to pickups in the scene.\n\nMake sure you have generated prototype content first!\n\nContinue?",
                "Yes", "Cancel"))
            {
                AssignDocuments();
                AssignEvidence();
                Debug.Log("[EscapeCampus] Prototype content assigned to pickups!");
            }
        }

        private static void AssignDocuments()
        {
            string[] docIDs = { "DOC_001", "DOC_002", "DOC_003", "DOC_004", "DOC_005" };
            string[] docPaths = {
                "Assets/ScriptableObjects/Documents/DOC_001_DeanMemo.asset",
                "Assets/ScriptableObjects/Documents/DOC_002_StudentComplaint.asset",
                "Assets/ScriptableObjects/Documents/DOC_003_LibraryLog.asset",
                "Assets/ScriptableObjects/Documents/DOC_004_MaintenanceRequest.asset",
                "Assets/ScriptableObjects/Documents/DOC_005_ResearchNotes.asset"
            };

            DocumentPickup[] pickups = Object.FindObjectsOfType<DocumentPickup>();

            for (int i = 0; i < pickups.Length && i < docPaths.Length; i++)
            {
                DocumentData data = AssetDatabase.LoadAssetAtPath<DocumentData>(docPaths[i]);
                if (data != null)
                {
                    pickups[i].SetDocumentData(data);
                    Debug.Log($"[PrototypeContent] Assigned {data.title} to {pickups[i].gameObject.name}");
                }
                else
                {
                    Debug.LogWarning($"[PrototypeContent] Could not load {docPaths[i]}. Generate content first!");
                }
            }
        }

        private static void AssignEvidence()
        {
            string[] evPaths = {
                "Assets/ScriptableObjects/Evidence/EV_001_StrangeLocks.asset",
                "Assets/ScriptableObjects/Evidence/EV_002_CovertResearch.asset",
                "Assets/ScriptableObjects/Evidence/EV_003_MissingBook.asset"
            };

            EvidencePickup[] pickups = Object.FindObjectsOfType<EvidencePickup>();

            for (int i = 0; i < pickups.Length && i < evPaths.Length; i++)
            {
                EvidenceData data = AssetDatabase.LoadAssetAtPath<EvidenceData>(evPaths[i]);
                if (data != null)
                {
                    pickups[i].SetEvidenceData(data);
                    Debug.Log($"[PrototypeContent] Assigned {data.title} to {pickups[i].gameObject.name}");
                }
                else
                {
                    Debug.LogWarning($"[PrototypeContent] Could not load {evPaths[i]}. Generate content first!");
                }
            }
        }
    }
}
#endif
