using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CoriidorFirstDungeonGenerator : RandomWalkMap
{
   [SerializeField]
    private int corridorLength=14,corridorCount=5;
    [SerializeField]
    [Range(0.1f,1)]
    private float roomPercent=0.8f;
   


 protected override void RunProceduralGeneration()
 {
    CorridorFirstGeneration();
 }
 /********** CorridorFirstGeneration  ****************************************/
 private void  CorridorFirstGeneration(){
    HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
    HashSet<Vector2Int> PotentialRoomPositions = new HashSet<Vector2Int>();
     List<List<Vector2Int>> corridors =  CreateCorridors(floorPositions , PotentialRoomPositions);

    CreateCorridors(floorPositions,PotentialRoomPositions);

    HashSet <Vector2Int> roomPositions =CreateRooms(PotentialRoomPositions);
    List<Vector2Int> deadEnds =FindAllDeadEnds(floorPositions);

    CreateRoomsAtDeadEnd(deadEnds,roomPositions);

    floorPositions.UnionWith(roomPositions);
    for (int i=0;i< corridors.Count;i++){

      corridors[i] = sizeOf3X3corridors(corridors[i]).ToList<Vector2Int>();
      floorPositions.UnionWith(corridors[i]);
      
    }

    tilemapVisualizer.PainFlorTiles(floorPositions);

    WallGenerator.CreateWalls(floorPositions,tilemapVisualizer);
 }
 /****************************************sizeOf3X3corridors****************************/
     private HashSet<Vector2Int> sizeOf3X3corridors(List<Vector2Int> corrider)
    {
        HashSet<Vector2Int> newcorrider = new HashSet<Vector2Int>();
        for (int i = 1; i < corrider.Count; i++)
        {
            for (int x = 1; x < 2; x++)
            {
                for (int y = 1; y < 2; y++)
                {
                    newcorrider.Add(corrider[i-1]+new Vector2Int(x,y));
                }
            }
        }
        return newcorrider;
    }
 /********************** CreateRoomsAtDeadEnd***********************/
 public void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds ,   HashSet <Vector2Int> roomFloors){
   foreach(var position in deadEnds){
      if (roomFloors.Contains(position)==false){
         var room =RunRandomWalk(position);
         roomFloors.UnionWith(room);


      }
   }
 }



 /********** find all dead Ends ****************************************/
 private List <Vector2Int> FindAllDeadEnds( HashSet<Vector2Int> floorPositions){
   List <Vector2Int> deadEnds= new List <Vector2Int>();

   foreach(var position in floorPositions){
      int neighbourCount =0;
      foreach(var direction in Direction2D.cardinalDirectionList){
         if(floorPositions.Contains(position + direction))
            neighbourCount ++;
      }
       if (neighbourCount == 1)
      deadEnds.Add(position);
   }
   return deadEnds;

 }


 /********** CreateRooms****************************************/
 private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> PotentialRoomPositions){
   HashSet<Vector2Int> roomPositions= new HashSet<Vector2Int>();
   int roomToCreateCount = Mathf.RoundToInt(PotentialRoomPositions.Count * roomPercent);

List<Vector2Int> roomsToCreate= PotentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();
 foreach( var roomPosition in roomsToCreate)
 {
      var roomFloor =RunRandomWalk(roomPosition);
      roomPositions.UnionWith(roomFloor);
 }
 return roomPositions;
 }



/********** creat corridors ****************************************/
 private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions,HashSet<Vector2Int> PotentialRoomPositions){
    var currentPosition =startPosition;
   PotentialRoomPositions.Add(currentPosition);
   List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();

    for(int i=0;i<corridorCount;i++){
        var corridor = ProceduralGeneratingAlgorithme.RandomWalkCarridor(currentPosition, corridorLength);
        corridors.Add(corridor);
        currentPosition=corridor[corridor.Count-1];
        PotentialRoomPositions.Add(currentPosition);
        floorPositions.UnionWith(corridor);
    }
    return corridors;
 }
}
