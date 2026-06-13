using UnityEngine;

namespace EscapeCampus.Documents
{
    [CreateAssetMenu(fileName = "NewDocument", menuName = "EscapeCampus/Document Data")]
    public class DocumentData : ScriptableObject
    {
        [Header("Identification")]
        public string documentID;
        public string title;

        [Header("Content")]
        [TextArea(5, 20)]
        public string content;
        public DocumentCategory category;

        [Header("Settings")]
        public bool isCritical;
        public Sprite thumbnail;
    }

    public enum DocumentCategory
    {
        Note,
        Letter,
        Report,
        Academic,
        Personal,
        Official,
        Other
    }
}
