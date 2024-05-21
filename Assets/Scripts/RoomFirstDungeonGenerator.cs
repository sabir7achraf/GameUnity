using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomFirstDungeonGenerator : RandomWalkMap
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
      [SerializeField]
    private GameObject coinsPrefab;
    private List<GameObject> enemyInstances = new List<GameObject>();
    private List<GameObject> coinsInstances = new List<GameObject>();

    protected override void RunProceduralGeneration()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        var roomList = ProceduralGeneratingAlgorithme.BinarySpacePartition(
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
            InstantiateRandomCoinsInRoom(room);
            InstantiateRandomEnemiesInRoom(room);
        }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        corridors = IncreaseCorridorSize(corridors);
        floor.UnionWith(corridors);

        tilemapVisualizer.PainFlorTiles(floor);
        WallGenerator.CreateWalls(floor, tilemapVisualizer);
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
        int numberOfEnemies = Random.Range(1, 6); // Générer un nombre aléatoire d'ennemis entre 1 et 5
        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Générer une position aléatoire à l'intérieur de la pièce en tenant compte des marges de 3
            float randomX = Random.Range(roomBounds.min.x+5, roomBounds.max.x-5);
            float randomY = Random.Range(roomBounds.min.y+5, roomBounds.max.y-5);
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
                DestroyImmediate(enemy);
            }
        }
        enemyInstances.Clear(); // Vider la liste après suppression
                foreach (var coins in coinsInstances)
        {
            if (coins != null)
            {
                DestroyImmediate(coins);
            }
        }
    }
        private void InstantiateRandomCoinsInRoom(BoundsInt roomBounds)
    {
        int numberOfEnemies = Random.Range(1, 7); // Générer un nombre aléatoire d'ennemis entre 1 et 5
        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Générer une position aléatoire à l'intérieur de la pièce en tenant compte des marges de 3
            float randomX = Random.Range(roomBounds.min.x + 3, roomBounds.max.x - 3);
            float randomY = Random.Range(roomBounds.min.y + 3, roomBounds.max.y - 3);
            Vector3 coinsPosition = new Vector3(randomX, randomY, 0);
            // Instancier un ennemi à la position aléatoire
            GameObject coinsInstance = Instantiate(coinsPrefab, coinsPosition, Quaternion.identity);
           coinsInstances.Add(coinsInstance); // Ajouter la référence à la liste des instances
        }
    }
}
