using UnityEngine;
using System.Collections.Generic;
using EscapeCampus.Documents;

namespace EscapeCampus.Evidence
{
    [CreateAssetMenu(fileName = "NewEvidence", menuName = "EscapeCampus/Evidence Data")]
    public class EvidenceData : ScriptableObject
    {
        [Header("Identification")]
        public string evidenceID;
        public string title;

        [Header("Content")]
        [TextArea(3, 10)]
        public string description;

        [Header("Relations")]
        public List<DocumentData> relatedDocuments = new List<DocumentData>();

        [Header("Settings")]
        public bool isCritical;
        public Sprite thumbnail;
    }
}
