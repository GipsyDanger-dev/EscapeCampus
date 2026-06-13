using System;
using System.Collections.Generic;

namespace EscapeCampus.Save
{
    [Serializable]
    public class SaveData
    {
        // Meta
        public string saveID;
        public string saveName;
        public string timestamp;
        public float playTimeSeconds;
        public string currentScene;

        // Player
        public PlayerSaveData player;

        // Documents
        public List<string> collectedDocumentIDs = new List<string>();

        // Evidence
        public List<string> collectedEvidenceIDs = new List<string>();

        // Puzzles
        public List<PuzzleSaveEntry> puzzleStates = new List<PuzzleSaveEntry>();

        // Horror
        public HorrorSaveEntry horrorState = new HorrorSaveEntry();

        // Semester 14 Observations
        public ObservationSaveEntry observationState = new ObservationSaveEntry();

        public SaveData()
        {
            saveID = Guid.NewGuid().ToString();
            saveName = "Manual Save";
            timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            playTimeSeconds = 0f;
            currentScene = "LobbyPrototype";
            player = new PlayerSaveData();
        }
    }

    [Serializable]
    public class PuzzleSaveEntry
    {
        public string puzzleID;
        public string state;

        public PuzzleSaveEntry() { }

        public PuzzleSaveEntry(string id, string state)
        {
            this.puzzleID = id;
            this.state = state;
        }
    }

    [Serializable]
    public class HorrorSaveEntry
    {
        public float horrorLevel;
        public List<string> triggeredEventIDs = new List<string>();

        public HorrorSaveEntry() { }
    }

    [Serializable]
    public class ObservationSaveEntry
    {
        public int totalObservations;
        public float lastObservationTime;

        public ObservationSaveEntry() { }
    }

    [Serializable]
    public class PlayerSaveData
    {
        public float posX;
        public float posY;
        public float posZ;
        public float rotX;
        public float rotY;
        public float rotZ;

        public PlayerSaveData()
        {
            posX = 0f;
            posY = 1f;
            posZ = 0f;
            rotX = 0f;
            rotY = 0f;
            rotZ = 0f;
        }

        public void SetPosition(UnityEngine.Vector3 position)
        {
            posX = position.x;
            posY = position.y;
            posZ = position.z;
        }

        public UnityEngine.Vector3 GetPosition()
        {
            return new UnityEngine.Vector3(posX, posY, posZ);
        }

        public void SetRotation(UnityEngine.Quaternion rotation)
        {
            UnityEngine.Vector3 euler = rotation.eulerAngles;
            rotX = euler.x;
            rotY = euler.y;
            rotZ = euler.z;
        }

        public UnityEngine.Quaternion GetRotation()
        {
            return UnityEngine.Quaternion.Euler(rotX, rotY, rotZ);
        }
    }
}
