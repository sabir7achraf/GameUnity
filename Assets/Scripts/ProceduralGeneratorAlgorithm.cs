using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public  static class ProceduralGeneratingAlgorithme
{
  
  public static HashSet<Vector2Int> SimpleRandomwalk(Vector2Int StartPosition ,int walklength){
    HashSet<Vector2Int> path =new HashSet<Vector2Int>();
    path.Add(StartPosition);
    var previousposition =StartPosition;
    for(int i=0;i<walklength;i++){
        var newPosition =previousposition+ Direction2D.GetRandomCardinalDirection();
        path.Add(newPosition);
        previousposition=newPosition;
    }
    return path;
  }

public static List<Vector2Int> RandomWalkCarridor(Vector2Int startPosition ,int corridorLength){
 
  List<Vector2Int> corridor = new List<Vector2Int>(); 
  var direction = Direction2D.GetRandomCardinalDirection();
  var currentPosition=startPosition	;
  corridor.Add(currentPosition);
  for(int i=0;i<corridorLength;i++){
    currentPosition +=direction;
    corridor.Add(currentPosition);
  }
  return corridor;

}
 public static List<BoundsInt> BinarySpacePartition(BoundsInt spaceToSplit , int minWidth , int minHeight){
        
        Queue<BoundsInt> roomQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomList = new List<BoundsInt>();
        

        roomQueue.Enqueue(spaceToSplit);

        while (roomQueue.Count>0)
        {
            var room = roomQueue.Dequeue();
            if (UnityEngine.Random.value < 0.5f) //splite verticualy
            {
                if (room.size.y >= minHeight && room.size.x >= minWidth)
                {
                    if (room.size.x >= minWidth * 2)
                    {
                        SplitVertically(minWidth  , roomQueue , room);

                    }else if(room.size.y >= minHeight * 2){

                        SplitHorizontally( minHeight , roomQueue , room);

                    }else if(room.size.x >= minWidth && room.size.y >= minHeight){
                        roomList.Add(room);
                    }
                }

            }else
            { //splite horizontaly
                if(room.size.y >= minHeight * 2){

                    SplitHorizontally(minHeight , roomQueue , room);
                    
                }else if (room.size.x >= minWidth * 2)
                {
                    SplitVertically(minWidth  , roomQueue , room);

                }else if(room.size.x >= minWidth && room.size.y >= minHeight){
                    roomList.Add(room);
                }
            }
        }
        return roomList;
    }
        private static void SplitVertically(int minWidth, Queue<BoundsInt> roomQueue, BoundsInt room)
    {
        int splitX = Random.Range(1,room.size.x);
        BoundsInt room1 = new BoundsInt(room.min , new  Vector3Int(splitX , room.size.y , room.size.z) );
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + splitX , room.min.y , room.min.z) ,
                                        new  Vector3Int(room.size.x - splitX , room.size.y , room.size.z) );
        roomQueue.Enqueue(room1);
        roomQueue.Enqueue(room2);
        
    }

    private static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomQueue, BoundsInt room)
    {
         int splitY = Random.Range(1,room.size.y);
        BoundsInt room1 = new BoundsInt(room.min , new  Vector3Int(room.size.x , splitY , room.size.z) );
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + splitY , room.min.z) ,
                                        new  Vector3Int(room.size.x, room.size.y - splitY , room.size.z) );
        roomQueue.Enqueue(room1);
        roomQueue.Enqueue(room2);
    }


}

public static class Direction2D
{
    public static List<Vector2Int> cardinalDirectionList = new List<Vector2Int>
    {
        new Vector2Int(0,1), //up
        new Vector2Int(1,0),//Right
        new Vector2Int(-1,0),//Left
        new Vector2Int(0,-1),//Down

    };
    public static Vector2Int GetRandomCardinalDirection(){
        return cardinalDirectionList[Random.Range(0,cardinalDirectionList.Count)];
    }
}