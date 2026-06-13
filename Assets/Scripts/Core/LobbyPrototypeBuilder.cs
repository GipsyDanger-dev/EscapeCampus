using UnityEngine;
using EscapeCampus.Interaction;

namespace EscapeCampus.Core
{
    public class LobbyPrototypeBuilder : MonoBehaviour
    {
        [Header("Materials")]
        [SerializeField] private Color floorColor = new Color(0.3f, 0.3f, 0.35f);
        [SerializeField] private Color wallColor = new Color(0.6f, 0.6f, 0.65f);
        [SerializeField] private Color ceilingColor = new Color(0.4f, 0.4f, 0.45f);
        [SerializeField] private Color doorColor = new Color(0.5f, 0.3f, 0.1f);
        [SerializeField] private Color accentColor = new Color(0.8f, 0.2f, 0.2f);

        private Material floorMat;
        private Material wallMat;
        private Material ceilingMat;
        private Material doorMat;
        private Material accentMat;

        private void Awake()
        {
            CreateMaterials();
            BuildLevel();
        }

        private void CreateMaterials()
        {
            floorMat = CreateMaterial(floorColor);
            wallMat = CreateMaterial(wallColor);
            ceilingMat = CreateMaterial(ceilingColor);
            doorMat = CreateMaterial(doorColor);
            accentMat = CreateMaterial(accentColor);
        }

        private Material CreateMaterial(Color color)
        {
            Material mat = new Material(Shader.Find("Standard"));
            mat.color = color;
            return mat;
        }

        private void BuildLevel()
        {
            GameObject levelRoot = new GameObject("--- LOBBY PROTOTYPE ---");

            // Spawn Area (0, 0, 0) - 10x10
            BuildRoom(levelRoot.transform, "SpawnArea", new Vector3(0, 0, 0), 10f, 10f, 4f);
            SpawnPlayer(new Vector3(0, 1, 0));

            // Hallway (0, 0, -12) - 4x16
            BuildRoom(levelRoot.transform, "Hallway", new Vector3(0, 0, -12), 4f, 16f, 4f);
            BuildDoorframe(levelRoot.transform, "DoorFrame_Spawn", new Vector3(0, 0, -4f));

            // Library Room (0, 0, -28) - 12x12
            BuildRoom(levelRoot.transform, "LibraryRoom", new Vector3(0, 0, -28), 12f, 12f, 5f);
            BuildDoorframe(levelRoot.transform, "DoorFrame_Library", new Vector3(0, 0, -20f));
            AddBookshelfRow(levelRoot.transform, new Vector3(-4, 0, -28), 6, 2f);
            AddBookshelfRow(levelRoot.transform, new Vector3(4, 0, -28), 6, 2f);

            // Archive Room (0, 0, -44) - 10x10
            BuildRoom(levelRoot.transform, "ArchiveRoom", new Vector3(0, 0, -44), 10f, 10f, 4f);
            BuildDoorframe(levelRoot.transform, "DoorFrame_Archive", new Vector3(0, 0, -34f));

            // Exit Door
            BuildExitDoor(levelRoot.transform, new Vector3(0, 0, -49f));

            // Add interactables
            AddInteractables(levelRoot.transform);
        }

        private void BuildRoom(Transform parent, string name, Vector3 center, float width, float depth, float height)
        {
            GameObject room = new GameObject(name);
            room.transform.SetParent(parent);
            room.transform.position = center;

            // Floor
            CreateCube(room.transform, "Floor", center + new Vector3(0, -0.05f, 0),
                new Vector3(width, 0.1f, depth), floorMat);

            // Ceiling
            CreateCube(room.transform, "Ceiling", center + new Vector3(0, height, 0),
                new Vector3(width, 0.1f, depth), ceilingMat);

            // Walls
            float halfW = width / 2f;
            float halfD = depth / 2f;

            // North wall
            CreateCube(room.transform, "Wall_North", center + new Vector3(0, height / 2f, -halfD),
                new Vector3(width, height, 0.3f), wallMat);

            // South wall
            CreateCube(room.transform, "Wall_South", center + new Vector3(0, height / 2f, halfD),
                new Vector3(width, height, 0.3f), wallMat);

            // East wall
            CreateCube(room.transform, "Wall_East", center + new Vector3(halfW, height / 2f, 0),
                new Vector3(0.3f, height, depth), wallMat);

            // West wall
            CreateCube(room.transform, "Wall_West", center + new Vector3(-halfW, height / 2f, 0),
                new Vector3(0.3f, height, depth), wallMat);
        }

        private void BuildDoorframe(Transform parent, string name, Vector3 position)
        {
            GameObject doorframe = new GameObject(name);
            doorframe.transform.SetParent(parent);
            doorframe.transform.position = position;

            float doorWidth = 2f;
            float doorHeight = 3f;
            float wallThick = 0.3f;

            // Left pillar
            CreateCube(doorframe.transform, "Pillar_L",
                position + new Vector3(-doorWidth / 2f - 0.25f, doorHeight / 2f, 0),
                new Vector3(0.5f, doorHeight, wallThick), accentMat);

            // Right pillar
            CreateCube(doorframe.transform, "Pillar_R",
                position + new Vector3(doorWidth / 2f + 0.25f, doorHeight / 2f, 0),
                new Vector3(0.5f, doorHeight, wallThick), accentMat);

            // Top beam
            CreateCube(doorframe.transform, "Beam_Top",
                position + new Vector3(0, doorHeight + 0.25f, 0),
                new Vector3(doorWidth + 1f, 0.5f, wallThick), accentMat);
        }

        private void BuildExitDoor(Transform parent, Vector3 position)
        {
            GameObject door = new GameObject("ExitDoor");
            door.transform.SetParent(parent);
            door.transform.position = position;

            GameObject doorCube = CreateCube(door.transform, "Door",
                position + new Vector3(0, 1.5f, 0),
                new Vector3(2f, 3f, 0.3f), doorMat);

            doorCube.AddComponent<InteractableObject>();

            // EXIT sign above
            CreateCube(door.transform, "ExitSign",
                position + new Vector3(0, 3.5f, 0),
                new Vector3(2.5f, 0.5f, 0.1f), accentMat);
        }

        private void AddBookshelfRow(Transform parent, Vector3 startPos, int count, float spacing)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = startPos + new Vector3(0, 1.5f, -count * spacing / 2f + i * spacing);
                CreateCube(parent, $"Bookshelf_{i}", pos,
                    new Vector3(1f, 3f, 0.8f), wallMat);
            }
        }

        private void AddInteractables(Transform parent)
        {
            // Add some interactable objects in the library
            Vector3[] bookPositions = new Vector3[]
            {
                new Vector3(-3, 1, -25),
                new Vector3(3, 1, -30),
                new Vector3(-2, 1, -32),
                new Vector3(4, 1, -26),
            };

            for (int i = 0; i < bookPositions.Length; i++)
            {
                GameObject book = CreateCube(parent, $"Book_{i}", bookPositions[i],
                    new Vector3(0.3f, 0.4f, 0.2f), accentMat);
                InteractableObject interactable = book.AddComponent<InteractableObject>();
                // InteractableObject prompt is set via serialized field
            }
        }

        private void SpawnPlayer(Vector3 position)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = position;
            }
        }

        private GameObject CreateCube(Transform parent, string name, Vector3 position, Vector3 scale, Material material)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.name = name;
            cube.transform.SetParent(parent);
            cube.transform.position = position;
            cube.transform.localScale = scale;
            cube.GetComponent<Renderer>().material = material;

            return cube;
        }
    }
}
