using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class RoomFirstDungeonGenerator : RandomWalkMap  
{
  [SerializeField]
  private int minRoomWidth =4, minRoomHeight=4;
  [SerializeField]
  private int dungeonWidth =20, dungeonHeight=20;
  [SerializeField]
  [Range(0,10)]
  private int offset =1;
  [SerializeField]
  private bool randomWalkRooms =false;
  [SerializeField]
   private int numRooms = 5;
     [SerializeField]
    private EnemyFactory enemyFactory;
    [SerializeField]
    private Transform enemiesParent;
    [SerializeField]
    private CoinFactory CoinFactory;
    [SerializeField]
    private Transform coinsParent;

    private List<GameObject> enemyInstances = new List<GameObject>();
    private List<GameObject> coinInstances = new List<GameObject>();

  protected override void RunProceduralGeneration(){
   
            List<BoundsInt> rooms = CreateRooms();
        GenerateEnemiesInRooms(rooms);
        GenerateCoinsInRooms(rooms);

  }
  private  List<BoundsInt> CreateRooms(){

    var roomList =ProceduralGeneratingAlgorithme.BinarySpacePartition(new BoundsInt ((Vector3Int)startPosition,new Vector3Int (dungeonWidth ,dungeonHeight,0)),minRoomWidth,minRoomHeight);
    HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
    floor =CreateSimpleRooms(roomList);
    List<Vector2Int> roomCenters = new List<Vector2Int>();
    foreach( var room in roomList){
      roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center));
    }

        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        corridors = IncreaseCorridorSize(corridors);

        floor.UnionWith(corridors);


        tilemapVisualizer.PainFlorTiles(floor);
    WallGenerator.CreateWalls(floor,tilemapVisualizer);
    return roomList;
  }
    /****************************************SpawnEnemies****************************/
    private void GenerateEnemiesInRooms(List<BoundsInt> rooms)
    {
        foreach (var enemyInstance in enemyInstances)
        {
            DestroyImmediate(enemyInstance);
        }
        enemyInstances.Clear();

        foreach (var roomBounds in rooms)
        {
            GenerateEnemyInRoom(roomBounds);
        }
    }

    private void GenerateEnemyInRoom(BoundsInt roomBounds)
    {
        int numberOfEnemies = Random.Range(1, 5);
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(roomBounds.min.x +3, roomBounds.max.x - 3), Random.Range(roomBounds.min.y + 3, roomBounds.max.y - 3), 0);
            GameObject newEnemy = enemyFactory.CreateEnemy(randomPosition, enemiesParent);
            enemyInstances.Add(newEnemy);
        }
    }
/********************GenerateCoinsInRooms************************/
    private void GenerateCoinsInRooms(List<BoundsInt> rooms)
    {
        foreach (var coinInstance in coinInstances)
        {
            DestroyImmediate(coinInstance);
        }
        coinInstances.Clear();

        foreach (var roomBounds in rooms)
        {
            GenerateCoinInRoom(roomBounds);
        }
    }

    private void GenerateCoinInRoom(BoundsInt roomBounds)
    {
        int numberOfCoins = Random.Range(1, 20);
        for (int i = 0; i < numberOfCoins; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(roomBounds.min.x + 10, roomBounds.max.x - 10), Random.Range(roomBounds.min.y + 10, roomBounds.max.y - 10), 0);
            GameObject newCoin = CoinFactory.CreateCoin(randomPosition, coinsParent);
            coinInstances.Add(newCoin);
        }
    }

    /****************************************IncreaseCorridorSize****************************/
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
    /*****************************ConnectRooms*************************************************/
    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters){
  HashSet <Vector2Int> corridors= new HashSet<Vector2Int>();
  var currentRoomCenter =roomCenters[Random.Range(0,roomCenters.Count)];
  roomCenters.Remove(currentRoomCenter);
  while(roomCenters.Count>0){
    Vector2Int closest =FindClosestPoint(currentRoomCenter,roomCenters);
    roomCenters.Remove(closest);
    HashSet <Vector2Int> newCorridor =CreateCorridor(currentRoomCenter,closest);
    currentRoomCenter =closest;
    corridors.UnionWith(newCorridor);

  }
return corridors;

}
/******************************CreateCorridor************************************/
private HashSet <Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int destination){
HashSet <Vector2Int> corridor =new HashSet <Vector2Int>();
var position =currentRoomCenter;
corridor.Add(position);
while(position.y !=destination.y){
  if(destination.y > position.y){
    position+= Vector2Int.up;
  }
  else if(destination.y < position.y )
  {
    position+=Vector2Int.down;
  }
  corridor.Add(position);
}
while( position.x !=destination.x )
{
  if( destination.x > position.x ){
    position+= Vector2Int.right;
  }
  else if(destination.x<position.x){
    position+=Vector2Int.left;
  }
  corridor.Add(position);
}
return corridor;

}
/******************************FindClosestPoint************************************/
private  Vector2Int FindClosestPoint(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters){
  Vector2Int closest =Vector2Int.zero;
  float distance =float.MaxValue;
  foreach(var position in roomCenters){
    float currentDistance =Vector2Int.Distance(position,currentRoomCenter);
    if(currentDistance <distance){
      distance=currentDistance;
      closest = position;
    }
  }
return closest;
}
  /**********CreateSimpleRooms*************/
  private HashSet<Vector2Int>CreateSimpleRooms(List<BoundsInt> roomList){
    HashSet<Vector2Int> floor=new HashSet<Vector2Int>();
    foreach(var room in roomList){
        for(int col=offset;col<room.size.x-offset;col++){
            for(int row=offset; row<room.size.x-offset; row++){
                Vector2Int position =(Vector2Int)room.min+new Vector2Int(col,row);
                floor.Add(position);
            }
        }
    }
    return floor;
  }
}
