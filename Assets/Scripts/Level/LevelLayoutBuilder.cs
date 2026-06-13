using UnityEngine;
using EscapeCampus.Core;

namespace EscapeCampus.Level
{
    public class LevelLayoutBuilder : MonoBehaviour
    {
        [Header("Materials")]
        private Material floorMat;
        private Material wallMat;
        private Material ceilingMat;
        private Material accentMat;
        private Material glassMat;
        private Material darkMat;

        private GameObject levelRoot;

        private void Awake()
        {
            CreateMaterials();
            BuildLevel();
        }

        private void CreateMaterials()
        {
            floorMat = CreateMat(new Color(0.35f, 0.32f, 0.3f));
            wallMat = CreateMat(new Color(0.6f, 0.58f, 0.55f));
            ceilingMat = CreateMat(new Color(0.45f, 0.43f, 0.4f));
            accentMat = CreateMat(new Color(0.7f, 0.2f, 0.2f));
            glassMat = CreateMat(new Color(0.6f, 0.7f, 0.8f, 0.3f));
            darkMat = CreateMat(new Color(0.05f, 0.05f, 0.08f));
        }

        private Material CreateMat(Color c)
        {
            Material m = new Material(Shader.Find("Standard"));
            m.color = c;
            if (c.a < 1f)
            {
                m.SetFloat("_Mode", 3);
                m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                m.SetInt("_ZWrite", 0);
                m.EnableKeyword("_ALPHABLEND_ON");
                m.renderQueue = 3000;
            }
            return m;
        }

        private void BuildLevel()
        {
            levelRoot = new GameObject("--- CAMPUS LIBRARY COMPLEX ---");

            BuildLobbyEntrance();
            BuildMainLibraryHall();
            BuildArchiveCorridor();
            BuildArchiveRoom();
            BuildMaintenanceHall();
            BuildGraduationHall();

            BuildConnections();
        }

        // ============================================
        // AREA 1: LOBBY ENTRANCE (START AREA)
        // ============================================
        private void BuildLobbyEntrance()
        {
            GameObject area = new GameObject("1_LOBBY_ENTRANCE");
            area.transform.SetParent(levelRoot.transform);

            // Room: 12x8x4
            BuildRoom(area.transform, new Vector3(0, 0, 0), 12f, 8f, 4f, floorMat, wallMat, ceilingMat);

            // Spawn point marker
            GameObject spawn = new GameObject("PLAYER_SPAWN");
            spawn.transform.SetParent(area.transform);
            spawn.transform.position = new Vector3(0, 1, 2);

            // Tutorial UI trigger
            GameObject tutorialTrigger = CreateTrigger(area.transform, "TutorialTrigger",
                new Vector3(0, 1, 0), new Vector3(6, 3, 4));

            // Warm lighting
            CreateLight(area.transform, "WarmLight_1", new Vector3(0, 3.5f, 0), new Color(1f, 0.9f, 0.7f), 1.2f);
            CreateLight(area.transform, "WarmLight_2", new Vector3(-4, 3.5f, -2), new Color(1f, 0.9f, 0.7f), 0.8f);
            CreateLight(area.transform, "WarmLight_3", new Vector3(4, 3.5f, -2), new Color(1f, 0.9f, 0.7f), 0.8f);

            // Reception desk
            CreateCube(area.transform, "ReceptionDesk", new Vector3(0, 0.5f, -2), new Vector3(4, 1, 1), accentMat);

            // Benches
            CreateCube(area.transform, "Bench_1", new Vector3(-3, 0.3f, 1), new Vector3(2, 0.6f, 0.8f), wallMat);
            CreateCube(area.transform, "Bench_2", new Vector3(3, 0.3f, 1), new Vector3(2, 0.6f, 0.8f), wallMat);

            // Doorframe to Main Library
            CreateDoorframe(area.transform, "DoorToLibrary", new Vector3(0, 0, -4), 2.5f, 3f);
        }

        // ============================================
        // AREA 2: MAIN LIBRARY HALL (EXPLORATION HUB)
        // ============================================
        private void BuildMainLibraryHall()
        {
            GameObject area = new GameObject("2_MAIN_LIBRARY_HALL");
            area.transform.SetParent(levelRoot.transform);

            // Room: 20x16x5
            BuildRoom(area.transform, new Vector3(0, 0, -16), 20f, 16f, 5f, floorMat, wallMat, ceilingMat);

            // Large bookshelf rows
            for (int row = 0; row < 4; row++)
            {
                float z = -10 - row * 3f;
                CreateCube(area.transform, $"BookshelfRow_L_{row}", new Vector3(-7, 1.5f, z),
                    new Vector3(1.2f, 3f, 0.8f), darkMat);
                CreateCube(area.transform, $"BookshelfRow_R_{row}", new Vector3(7, 1.5f, z),
                    new Vector3(1.2f, 3f, 0.8f), darkMat);
            }

            // Center reading tables
            CreateCube(area.transform, "ReadingTable_1", new Vector3(0, 0.4f, -12), new Vector3(3, 0.8f, 1.5f), wallMat);
            CreateCube(area.transform, "ReadingTable_2", new Vector3(0, 0.4f, -18), new Vector3(3, 0.8f, 1.5f), wallMat);

            // Chairs
            for (int i = 0; i < 4; i++)
            {
                CreateCube(area.transform, $"Chair_{i}", new Vector3(-1.5f + i * 1f, 0.3f, -12),
                    new Vector3(0.4f, 0.6f, 0.4f), wallMat);
            }

            // Lighting
            CreateLight(area.transform, "LibraryLight_1", new Vector3(0, 4.5f, -12), new Color(0.9f, 0.85f, 0.8f), 1f);
            CreateLight(area.transform, "LibraryLight_2", new Vector3(0, 4.5f, -20), new Color(0.9f, 0.85f, 0.8f), 1f);
            CreateLight(area.transform, "LibraryLight_3", new Vector3(-8, 4.5f, -16), new Color(0.9f, 0.85f, 0.8f), 0.6f);
            CreateLight(area.transform, "LibraryLight_4", new Vector3(8, 4.5f, -16), new Color(0.9f, 0.85f, 0.8f), 0.6f);

            // Librarian desk
            CreateCube(area.transform, "LibrarianDesk", new Vector3(8, 0.5f, -8), new Vector3(2, 1, 1), accentMat);

            // Doorframe to Archive Corridor
            CreateDoorframe(area.transform, "DoorToArchive", new Vector3(0, 0, -24), 2.5f, 3f);
        }

        // ============================================
        // AREA 3: ARCHIVE CORRIDOR (HORROR ZONE)
        // ============================================
        private void BuildArchiveCorridor()
        {
            GameObject area = new GameObject("3_ARCHIVE_CORRIDOR");
            area.transform.SetParent(levelRoot.transform);

            // Corridor: 3x20x3.5
            BuildRoom(area.transform, new Vector3(0, 0, -36), 3f, 20f, 3.5f, floorMat, wallMat, ceilingMat);

            // Corridor lights (will be controlled by setpiece)
            for (int i = 0; i < 6; i++)
            {
                float z = -27 - i * 2.5f;
                GameObject lightObj = CreateLight(area.transform, $"CorridorLight_{i}",
                    new Vector3(0, 3f, z), new Color(0.8f, 0.75f, 0.7f), 0.7f);
                lightObj.tag = "CorridorLight"; // Tag for setpiece control
            }

            // Glass panel (for S14 flash)
            CreateCube(area.transform, "GlassPanel", new Vector3(1.3f, 1.5f, -32), new Vector3(0.1f, 2.5f, 2f), glassMat);

            // Door at end (will close during setpiece)
            GameObject endDoor = CreateCube(area.transform, "CorridorEndDoor", new Vector3(0, 1.5f, -46),
                new Vector3(2.5f, 3f, 0.3f), darkMat);
            endDoor.AddComponent<Core.DoorController>();

            // Setpiece trigger volume
            GameObject setpieceTrigger = CreateTrigger(area.transform, "SetPieceTrigger_WhisperCorridor",
                new Vector3(0, 1, -30), new Vector3(3, 3, 3));
        }

        // ============================================
        // AREA 4: ARCHIVE ROOM (INVESTIGATION CORE)
        // ============================================
        private void BuildArchiveRoom()
        {
            GameObject area = new GameObject("4_ARCHIVE_ROOM");
            area.transform.SetParent(levelRoot.transform);

            // Room: 14x12x4
            BuildRoom(area.transform, new Vector3(0, 0, -56), 14f, 12f, 4f, floorMat, wallMat, ceilingMat);

            // Archive shelves
            for (int i = 0; i < 5; i++)
            {
                float x = -5 + i * 2.5f;
                CreateCube(area.transform, $"ArchiveShelf_{i}", new Vector3(x, 1.5f, -52),
                    new Vector3(1f, 3f, 1f), darkMat);
                CreateCube(area.transform, $"ArchiveShelf_B_{i}", new Vector3(x, 1.5f, -60),
                    new Vector3(1f, 3f, 1f), darkMat);
            }

            // Research desk
            CreateCube(area.transform, "ResearchDesk", new Vector3(0, 0.5f, -56), new Vector3(3, 1, 2), wallMat);

            // Filing cabinets
            CreateCube(area.transform, "FilingCabinet_1", new Vector3(-6, 0.7f, -50), new Vector3(1, 1.4f, 0.6f), wallMat);
            CreateCube(area.transform, "FilingCabinet_2", new Vector3(-6, 0.7f, -62), new Vector3(1, 1.4f, 0.6f), wallMat);

            // Lighting (dimmer)
            CreateLight(area.transform, "ArchiveLight_1", new Vector3(0, 3.5f, -52), new Color(0.7f, 0.65f, 0.6f), 0.6f);
            CreateLight(area.transform, "ArchiveLight_2", new Vector3(0, 3.5f, -60), new Color(0.7f, 0.65f, 0.6f), 0.6f);

            // Glass panel for mirror observation
            CreateCube(area.transform, "ArchiveGlass", new Vector3(6.8f, 1.5f, -56), new Vector3(0.1f, 2.5f, 4f), glassMat);

            // Doorframe to Maintenance Hall
            CreateDoorframe(area.transform, "DoorToMaintenance", new Vector3(0, 0, -62), 2.5f, 3f);
        }

        // ============================================
        // AREA 5: MAINTENANCE HALL (SAFE ZONE)
        // ============================================
        private void BuildMaintenanceHall()
        {
            GameObject area = new GameObject("5_MAINTENANCE_HALL");
            area.transform.SetParent(levelRoot.transform);

            // Room: 10x8x3.5
            BuildRoom(area.transform, new Vector3(0, 0, -72), 10f, 8f, 3.5f, floorMat, wallMat, ceilingMat);

            // Safe zone trigger
            GameObject safeTrigger = CreateTrigger(area.transform, "SafeZoneTrigger",
                new Vector3(0, 1, -72), new Vector3(8, 3, 6));
            SafeZone safeZone = safeTrigger.AddComponent<Core.Pacing.SafeZone>();

            // Maintenance equipment
            CreateCube(area.transform, "ToolShelf_1", new Vector3(-4, 1f, -70), new Vector3(1.5f, 2f, 0.8f), wallMat);
            CreateCube(area.transform, "ToolShelf_2", new Vector3(4, 1f, -70), new Vector3(1.5f, 2f, 0.8f), wallMat);

            // Workbench
            CreateCube(area.transform, "Workbench", new Vector3(0, 0.5f, -74), new Vector3(3, 1, 1.5f), wallMat);

            // Lighting (warm, safe)
            CreateLight(area.transform, "SafeLight_1", new Vector3(0, 3f, -70), new Color(1f, 0.95f, 0.8f), 1f);
            CreateLight(area.transform, "SafeLight_2", new Vector3(0, 3f, -74), new Color(1f, 0.95f, 0.8f), 1f);

            // Locked door to Graduation Hall
            GameObject gradDoor = CreateCube(area.transform, "GraduationDoor", new Vector3(0, 1.5f, -76),
                new Vector3(2.5f, 3f, 0.3f), accentMat);
            DoorController doorCtrl = gradDoor.AddComponent<Core.DoorController>();
            // Door starts locked - will be unlocked during Phase 6
        }

        // ============================================
        // AREA 6: GRADUATION HALL (FINAL AREA)
        // ============================================
        private void BuildGraduationHall()
        {
            GameObject area = new GameObject("6_GRADUATION_HALL");
            area.transform.SetParent(levelRoot.transform);

            // Hall: 16x20x6
            BuildRoom(area.transform, new Vector3(0, 0, -90), 16f, 20f, 6f, floorMat, wallMat, ceilingMat);

            // Stage
            CreateCube(area.transform, "Stage", new Vector3(0, 0.3f, -96), new Vector3(12, 0.6f, 6f), darkMat);

            // Podium
            CreateCube(area.transform, "Podium", new Vector3(0, 0.8f, -98), new Vector3(2, 1.6f, 1f), accentMat);

            // Ritual circle on floor (visual marker)
            GameObject ritualCircle = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            ritualCircle.name = "RitualCircle";
            ritualCircle.transform.SetParent(area.transform);
            ritualCircle.transform.position = new Vector3(0, 0.05f, -90);
            ritualCircle.transform.localScale = new Vector3(6, 0.02f, 6);
            Renderer rcRenderer = ritualCircle.GetComponent<Renderer>();
            if (rcRenderer != null)
            {
                Material rcMat = new Material(Shader.Find("Standard"));
                rcMat.color = new Color(0.5f, 0.1f, 0.1f, 0.8f);
                rcMat.SetFloat("_Mode", 3);
                rcMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                rcMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                rcMat.SetInt("_ZWrite", 0);
                rcMat.EnableKeyword("_ALPHABLEND_ON");
                rcMat.renderQueue = 3000;
                rcRenderer.material = rcMat;
            }
            Collider rcCol = ritualCircle.GetComponent<Collider>();
            if (rcCol != null) Destroy(rcCol);

            // Seating rows
            for (int row = 0; row < 3; row++)
            {
                float z = -84 - row * 2f;
                for (int seat = 0; seat < 6; seat++)
                {
                    float x = -5 + seat * 2f;
                    CreateCube(area.transform, $"Seat_{row}_{seat}", new Vector3(x, 0.4f, z),
                        new Vector3(0.8f, 0.8f, 0.8f), wallMat);
                }
            }

            // Dim, eerie lighting
            CreateLight(area.transform, "HallLight_1", new Vector3(0, 5.5f, -88), new Color(0.4f, 0.3f, 0.5f), 0.5f);
            CreateLight(area.transform, "HallLight_2", new Vector3(0, 5.5f, -96), new Color(0.4f, 0.3f, 0.5f), 0.5f);
            CreateLight(area.transform, "HallLight_3", new Vector3(-6, 5.5f, -90), new Color(0.3f, 0.2f, 0.4f), 0.3f);
            CreateLight(area.transform, "HallLight_4", new Vector3(6, 5.5f, -90), new Color(0.3f, 0.2f, 0.4f), 0.3f);

            // Final decision trigger
            GameObject finalTrigger = CreateTrigger(area.transform, "FinalDecisionTrigger",
                new Vector3(0, 1, -90), new Vector3(8, 4, 8));
        }

        // ============================================
        // CONNECTIONS
        // ============================================
        private void BuildConnections()
        {
            // Lobby → Library (opening)
            CreateCube(levelRoot.transform, "Wall_Lobby_L", new Vector3(-6, 2, -4), new Vector3(0.3f, 4, 0.3f), wallMat);
            CreateCube(levelRoot.transform, "Wall_Lobby_R", new Vector3(6, 2, -4), new Vector3(0.3f, 4, 0.3f), wallMat);

            // Library → Corridor opening
            CreateCube(levelRoot.transform, "Wall_Library_L", new Vector3(-1.5f, 2, -24), new Vector3(0.3f, 4, 0.3f), wallMat);
            CreateCube(levelRoot.transform, "Wall_Library_R", new Vector3(1.5f, 2, -24), new Vector3(0.3f, 4, 0.3f), wallMat);

            // Corridor → Archive opening
            CreateCube(levelRoot.transform, "Wall_Corridor_L", new Vector3(-1.5f, 2, -46), new Vector3(0.3f, 3.5f, 0.3f), wallMat);
            CreateCube(levelRoot.transform, "Wall_Corridor_R", new Vector3(1.5f, 2, -46), new Vector3(0.3f, 3.5f, 0.3f), wallMat);

            // Archive → Maintenance opening
            CreateCube(levelRoot.transform, "Wall_Archive_L", new Vector3(-1.5f, 2, -62), new Vector3(0.3f, 4, 0.3f), wallMat);
            CreateCube(levelRoot.transform, "Wall_Archive_R", new Vector3(1.5f, 2, -62), new Vector3(0.3f, 4, 0.3f), wallMat);

            // Maintenance → Graduation (locked door area)
            CreateCube(levelRoot.transform, "Wall_Maint_L", new Vector3(-1.5f, 2, -76), new Vector3(0.3f, 3.5f, 0.3f), wallMat);
            CreateCube(levelRoot.transform, "Wall_Maint_R", new Vector3(1.5f, 2, -76), new Vector3(0.3f, 3.5f, 0.3f), wallMat);
        }

        // ============================================
        // HELPERS
        // ============================================
        private void BuildRoom(Transform parent, Vector3 center, float w, float d, float h,
            Material floor, Material wall, Material ceiling)
        {
            float halfW = w / 2f;
            float halfD = d / 2f;

            // Floor
            CreatePrimitive(parent, "Floor", center + new Vector3(0, -0.05f, 0),
                new Vector3(w, 0.1f, d), floor, PrimitiveType.Cube);

            // Ceiling
            CreatePrimitive(parent, "Ceiling", center + new Vector3(0, h, 0),
                new Vector3(w, 0.1f, d), ceiling, PrimitiveType.Cube);

            // North wall
            CreatePrimitive(parent, "Wall_N", center + new Vector3(0, h / 2f, -halfD),
                new Vector3(w, h, 0.3f), wall, PrimitiveType.Cube);

            // South wall
            CreatePrimitive(parent, "Wall_S", center + new Vector3(0, h / 2f, halfD),
                new Vector3(w, h, 0.3f), wall, PrimitiveType.Cube);

            // East wall
            CreatePrimitive(parent, "Wall_E", center + new Vector3(halfW, h / 2f, 0),
                new Vector3(0.3f, h, d), wall, PrimitiveType.Cube);

            // West wall
            CreatePrimitive(parent, "Wall_W", center + new Vector3(-halfW, h / 2f, 0),
                new Vector3(0.3f, h, d), wall, PrimitiveType.Cube);
        }

        private GameObject CreateCube(Transform parent, string name, Vector3 pos, Vector3 scale, Material mat)
        {
            return CreatePrimitive(parent, name, pos, scale, mat, PrimitiveType.Cube);
        }

        private GameObject CreatePrimitive(Transform parent, string name, Vector3 pos, Vector3 scale,
            Material mat, PrimitiveType type)
        {
            GameObject obj = GameObject.CreatePrimitive(type);
            obj.name = name;
            obj.transform.SetParent(parent);
            obj.transform.position = pos;
            obj.transform.localScale = scale;
            Renderer r = obj.GetComponent<Renderer>();
            if (r != null) r.material = mat;
            return obj;
        }

        private GameObject CreateLight(Transform parent, string name, Vector3 pos, Color color, float intensity)
        {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(parent);
            obj.transform.position = pos;
            Light light = obj.AddComponent<Light>();
            light.type = LightType.Point;
            light.color = color;
            light.intensity = intensity;
            light.range = 15f;
            return obj;
        }

        private void CreateDoorframe(Transform parent, string name, Vector3 pos, float width, float height)
        {
            GameObject frame = new GameObject(name);
            frame.transform.SetParent(parent);
            frame.transform.position = pos;

            // Left pillar
            CreateCube(frame.transform, "Pillar_L", pos + new Vector3(-width / 2f - 0.2f, height / 2f, 0),
                new Vector3(0.4f, height, 0.3f), accentMat);

            // Right pillar
            CreateCube(frame.transform, "Pillar_R", pos + new Vector3(width / 2f + 0.2f, height / 2f, 0),
                new Vector3(0.4f, height, 0.3f), accentMat);

            // Top beam
            CreateCube(frame.transform, "Beam", pos + new Vector3(0, height + 0.2f, 0),
                new Vector3(width + 0.8f, 0.4f, 0.3f), accentMat);
        }

        private GameObject CreateTrigger(Transform parent, string name, Vector3 pos, Vector3 scale)
        {
            GameObject obj = new GameObject(name);
            obj.transform.SetParent(parent);
            obj.transform.position = pos;
            BoxCollider col = obj.AddComponent<BoxCollider>();
            col.size = scale;
            col.isTrigger = true;
            return obj;
        }
    }
}
