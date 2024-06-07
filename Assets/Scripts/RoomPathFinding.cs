using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomPathFind : RandomWalkMap
{
    [SerializeField]
    private int minRoomWidth = 4, minRoomHeight = 4;
    [SerializeField]
    private int dungeonWidth = 20, dungeonHeight = 20;
    [SerializeField]
    [Range(0, 10)]
    private int offset = 1;
    [SerializeField]
    private bool randomWalkRooms = false;
    [SerializeField]
    private GameObject enemyPrefab;
    private List<GameObject> enemyInstances = new List<GameObject>();
    private List<GameObject> coinsInstances = new List<GameObject>();

    // Variables pour joueur et le Boss
    [SerializeField]
    private Transform player; // Référence à l'objet du joueur dans la hiérarchie
    [SerializeField]
    private Transform boss;   // Référence à l'objet du boss dans la hiérarchie
    [SerializeField]
    private CameraFollow cameraFollow; // Référence au script CameraFollow

    private List<BoundsInt> roomList; // Liste des rooms

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
        PlacePlayerAndBoss();
    }
    
    private void CreateRooms()
    {
        roomList = ProceduralGeneratingAlgorithme.BinarySpacePartition(
            new BoundsInt((Vector3Int)startPosition, new Vector3Int(dungeonWidth, dungeonHeight, 0)),
            minRoomWidth, minRoomHeight
        );

        HashSet<Vector2Int> floor = CreateSimpleRooms(roomList);
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        DestroyAllEnemiesAndCoins();

        foreach (var room in roomList)
        {
            Vector2Int roomCenter = (Vector2Int)Vector3Int.RoundToInt(room.center);
            roomCenters.Add(roomCenter);
            InstantiateRandomEnemiesInRoom(room);
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        corridors = IncreaseCorridorSize(corridors);
        floor.UnionWith(corridors);

        tilemapVisualizer.PainFlorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
    }

    private void PlacePlayerAndBoss()
    {
        if (roomList == null || roomList.Count == 0)
        {
            Debug.LogError("roomList is null or empty");
            return;
        }
        // Le premier room est à l'index 0
        var firstRoom = roomList[0];
    
        // Déplacement du joueur dans le premier room
        Vector3 playerPosition = new Vector3(
            Random.Range(firstRoom.min.x + 2, firstRoom.max.x - 2),
            Random.Range(firstRoom.min.y + 2, firstRoom.max.y - 2),
            0
        );
        player.position = playerPosition;

        // cameraFollowPlayer
        if (cameraFollow != null)
        {
            cameraFollow.SetPlayer(player);
        }
    
        // Trouver le room le plus éloigné du joueur
        var lastRoom = roomList[0];
        float maxDistance = 0;
        foreach (var room in roomList)
        {
            Vector3 roomCenter = new Vector3(
                (room.min.x + room.max.x) / 2,
                (room.min.y + room.max.y) / 2,
                0
            );
            float distance = Vector3.Distance(playerPosition, roomCenter);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                lastRoom = room;
            }
        }
    
        // Déplacement du boss dans le room le plus éloigné
        Vector3 bossPosition = new Vector3(
            Random.Range(lastRoom.min.x + 5, lastRoom.max.x - 5),
            Random.Range(lastRoom.min.y + 5, lastRoom.max.y - 5),
            0
        );
        boss.position = bossPosition;
    }

    private HashSet<Vector2Int> IncreaseCorridorSize(HashSet<Vector2Int> corridors)
    {
        HashSet<Vector2Int> enlargedCorridors = new HashSet<Vector2Int>();
        foreach (var corridorTile in corridors)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    enlargedCorridors.Add(corridorTile + new Vector2Int(x, y));
                }
            }
        }
        return enlargedCorridors;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);
        while (roomCenters.Count > 0)
        {
            Vector2Int closest = FindClosestPoint(currentRoomCenter, roomCenters);
            roomCenters.Remove(closest);
            HashSet<Vector2Int> newCorridor = CreateCorridor(currentRoomCenter, closest);
            currentRoomCenter = closest;
            corridors.UnionWith(newCorridor);
        }
        return corridors;
    }
    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination)
    {
        HashSet<Vector2Int> corridor = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridor.Add(position);

        while (position.y != destination.y)
        {
            if (destination.y > position.y)
            {
                position += Vector2Int.up;
            }
            else if (destination.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridor.Add(position);
        }
        while (position.x != destination.x)
        {
            if (destination.x > position.x)
            {
                position += Vector2Int.right;
            }
            else if (destination.x < position.x)
            {
                position += Vector2Int.left;
            }
            corridor.Add(position);
        }
        return corridor;
    }
    private Vector2Int FindClosestPoint(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closest = Vector2Int.zero;
        float distance = float.MaxValue;
        foreach (var position in roomCenters)
        {
            float currentDistance = Vector2Int.Distance(position, currentRoomCenter);
            if (currentDistance < distance)
            {
                distance = currentDistance;
                closest = position;
            }
        }
        return closest;
    }
    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach (var room in roomList)
        {
            for (int col = offset; col < room.size.x - offset; col++)
            {
                for (int row = offset; row < room.size.y - offset; row++)
                {
                    Vector2Int position = (Vector2Int)room.min + new Vector2Int(col, row);
                    floor.Add(position);
                }
            }
        }
        return floor;
    }
    private void InstantiateRandomEnemiesInRoom(BoundsInt roomBounds)
    {
        int numberOfEnemies = Random.Range(1, 3); // Générer un nombre aléatoire d'ennemis entre 1 et 5
        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Générer une position aléatoire à l'intérieur de la pièce en tenant compte des marges de 3
            float randomX = Random.Range(roomBounds.min.x + 5, roomBounds.max.x - 5);
            float randomY = Random.Range(roomBounds.min.y + 5, roomBounds.max.y - 5);
            Vector3 enemyPosition = new Vector3(randomX, randomY, 0);

            // Instancier un ennemi à la position aléatoire
            GameObject enemyInstance = Instantiate(enemyPrefab, enemyPosition, Quaternion.identity);
            enemyInstances.Add(enemyInstance); // Ajouter la référence à la liste des instances
        }
    }
    public void DestroyAllEnemiesAndCoins()
    {
        foreach (var enemy in enemyInstances)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        enemyInstances.Clear(); // Vider la liste après suppression
    }

    public void ReplayDungeon()
    {
        DestroyAllEnemiesAndCoins();
        tilemapVisualizer.clear();
        RunProceduralGeneration();
        Debug.Log("Dungeon replayed");
    }
}
