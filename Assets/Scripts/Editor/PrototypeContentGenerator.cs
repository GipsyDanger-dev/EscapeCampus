#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using EscapeCampus.Documents;
using EscapeCampus.Evidence;

namespace EscapeCampus.Editor
{
    public class PrototypeContentGenerator : EditorWindow
    {
        [MenuItem("EscapeCampus/Generate Prototype Content")]
        public static void GenerateContent()
        {
            if (EditorUtility.DisplayDialog("Generate Prototype Content",
                "This will create 5 DocumentData and 3 EvidenceData ScriptableObjects.\n\nContinue?",
                "Yes", "Cancel"))
            {
                CreateDocuments();
                CreateEvidence();
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                Debug.Log("[EscapeCampus] Prototype content generated!");
            }
        }

        private static void CreateDocuments()
        {
            string folderPath = "Assets/ScriptableObjects/Documents";
            EnsureDirectory(folderPath);

            // Document 1 - Critical
            DocumentData doc1 = CreateInstance<DocumentData>();
            doc1.documentID = "DOC_001";
            doc1.title = "Dean's Confidential Memo";
            doc1.category = DocumentCategory.Official;
            doc1.isCritical = true;
            doc1.content = @"CONFIDENTIAL - INTERNAL USE ONLY

Date: March 15, 2024
From: Dean Morrison
To: Department Heads

Subject: Campus Restructuring

It has come to my attention that certain research activities in the east wing must be immediately suspended. The funding has been... redirected.

All staff are to cooperate fully with the external audit team arriving next week. Do not discuss departmental matters with students.

Destroy this memo after reading.

- Dean Morrison";
            AssetDatabase.CreateAsset(doc1, $"{folderPath}/DOC_001_DeanMemo.asset");

            // Document 2
            DocumentData doc2 = CreateInstance<DocumentData>();
            doc2.documentID = "DOC_002";
            doc2.title = "Student Complaint Letter";
            doc2.category = DocumentCategory.Personal;
            doc2.isCritical = false;
            doc2.content = @"Dear Administration,

I am writing to formally complain about the strange noises coming from the basement of the science building during late hours. As a resident of the nearby dormitory, I have been unable to sleep properly for the past three weeks.

Several other students have also reported seeing unusual lights and hearing muffled conversations. When I approached Professor Chen about this, he became very defensive and told me to mind my own business.

I request a formal investigation into this matter.

Sincerely,
Maria Santos
Student ID: 2023-CS-0147";
            AssetDatabase.CreateAsset(doc2, $"{folderPath}/DOC_002_StudentComplaint.asset");

            // Document 3
            DocumentData doc3 = CreateInstance<DocumentData>();
            doc3.documentID = "DOC_003";
            doc3.title = "Library Checkout Log";
            doc3.category = DocumentCategory.Academic;
            doc3.isCritical = false;
            doc3.content = @"CAMPUS LIBRARY - CHECKOUT LOG
Week of March 10-16, 2024

Title: Advanced Quantum Mechanics (Vol. 3)
Borrower: Dr. R. Chen
Due Date: March 24, 2024
Status: OVERDUE

Title: Historical Records of Campus Architecture
Borrower: Prof. A. Williams
Due Date: March 17, 2024
Status: Returned

Title: Cryptography and Modern Security
Borrower: Unknown (card damaged)
Due Date: March 12, 2024
Status: MISSING

Note: Librarian reports that the cryptography book was last seen being taken to the restricted section by an unidentified individual wearing a lab coat.";
            AssetDatabase.CreateAsset(doc3, $"{folderPath}/DOC_003_LibraryLog.asset");

            // Document 4
            DocumentData doc4 = CreateInstance<DocumentData>();
            doc4.documentID = "DOC_004";
            doc4.title = "Maintenance Request Form";
            doc4.category = DocumentCategory.Report;
            doc4.isCritical = false;
            doc4.content = @"MAINTENANCE REQUEST FORM
Request #: MT-2024-0389

Date Submitted: March 8, 2024
Submitted By: Janitorial Staff - Section B
Location: Science Building, Basement Level 2

Issue Description:
Multiple attempts to access basement level 2 have been blocked by electronic locks that are not in our system. Security office claims no knowledge of these locks being installed.

Additional Notes:
- Strange chemical smell reported by night shift staff
- Unusual amount of electrical usage from sub-basement
- Request for building blueprints denied by administration

Status: PENDING - FLAGGED FOR REVIEW";
            AssetDatabase.CreateAsset(doc4, $"{folderPath}/DOC_004_MaintenanceRequest.asset");

            // Document 5
            DocumentData doc5 = CreateInstance<DocumentData>();
            doc5.documentID = "DOC_005";
            doc5.title = "Professor's Research Notes";
            doc3.category = DocumentCategory.Note;
            doc5.isCritical = false;
            doc5.content = @"Personal Research Notes - Dr. R. Chen
Project: HARMONY (Codename)

Day 47: The neural interface prototype is showing promising results. Subject response time has improved by 340%. However, there are... side effects that the committee doesn't need to know about yet.

Day 52: Memory consolidation is becoming an issue. Subjects report gaps in their recollection. This could be a feature, not a bug, depending on the application.

Day 58: Dean Morrison is asking too many questions. I've moved the sensitive materials to the secondary location. If anyone finds these notes, know that the truth is buried deeper than they think.

The key is in the clock tower.";
            AssetDatabase.CreateAsset(doc5, $"{folderPath}/DOC_005_ResearchNotes.asset");

            Debug.Log("[PrototypeContent] Created 5 documents.");
        }

        private static void CreateEvidence()
        {
            string folderPath = "Assets/ScriptableObjects/Evidence";
            EnsureDirectory(folderPath);

            // Load documents for references
            DocumentData doc1 = AssetDatabase.LoadAssetAtPath<DocumentData>("Assets/ScriptableObjects/Documents/DOC_001_DeanMemo.asset");
            DocumentData doc3 = AssetDatabase.LoadAssetAtPath<DocumentData>("Assets/ScriptableObjects/Documents/DOC_003_LibraryLog.asset");
            DocumentData doc4 = AssetDatabase.LoadAssetAtPath<DocumentData>("Assets/ScriptableObjects/Documents/DOC_004_MaintenanceRequest.asset");
            DocumentData doc5 = AssetDatabase.LoadAssetAtPath<DocumentData>("Assets/ScriptableObjects/Documents/DOC_005_ResearchNotes.asset");

            // Evidence 1
            EvidenceData ev1 = CreateInstance<EvidenceData>();
            ev1.evidenceID = "EV_001";
            ev1.title = "Strange Lock Installation";
            ev1.description = "Electronic locks not present in any official building management system have been installed in the science building basement. These locks appear to have been installed covertly, blocking access to sub-basement levels.";
            ev1.isCritical = false;
            if (doc4 != null) ev1.relatedDocuments.Add(doc4);
            AssetDatabase.CreateAsset(ev1, $"{folderPath}/EV_001_StrangeLocks.asset");

            // Evidence 2
            EvidenceData ev2 = CreateInstance<EvidenceData>();
            ev2.evidenceID = "EV_002";
            ev2.title = "Covert Research Activity";
            ev2.description = "Multiple sources indicate unauthorized research is being conducted on campus. Student complaints about strange noises, unusual electrical usage, and a missing cryptography textbook all point to a secret project operating outside official channels.";
            ev2.isCritical = true;
            if (doc1 != null) ev2.relatedDocuments.Add(doc1);
            if (doc3 != null) ev2.relatedDocuments.Add(doc3);
            if (doc5 != null) ev2.relatedDocuments.Add(doc5);
            AssetDatabase.CreateAsset(ev2, $"{folderPath}/EV_002_CovertResearch.asset");

            // Evidence 3
            EvidenceData ev3 = CreateInstance<EvidenceData>();
            ev3.evidenceID = "EV_003";
            ev3.title = "Missing Cryptography Book";
            ev3.description = "A cryptography textbook was checked out by an unidentified individual and never returned. The librarian reports seeing someone in a lab coat take it to the restricted section. This may be connected to the security anomalies on campus.";
            ev3.isCritical = false;
            if (doc3 != null) ev3.relatedDocuments.Add(doc3);
            AssetDatabase.CreateAsset(ev3, $"{folderPath}/EV_003_MissingBook.asset");

            Debug.Log("[PrototypeContent] Created 3 evidence items.");
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
