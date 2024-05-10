using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

public class RandomWalkMap : abstractDungeonGenerator
{

[SerializeField]
    private int iterations =10;
    [SerializeField]
    public int walkLength =10;
    [SerializeField]
    public bool startRondmlyEachIteration=true;


    protected override void  RunProceduralGeneration(){
        HashSet<Vector2Int> floorPositions= RunRandomWalk(startPosition);
        tilemapVisualizer.clear();
        tilemapVisualizer.PainFlorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions,tilemapVisualizer);

    }
    protected HashSet<Vector2Int> RunRandomWalk(Vector2Int position){
        var currentPosition= position;
        HashSet<Vector2Int> floorPositions=new HashSet<Vector2Int>();
        for(int i=0;i<iterations;i++)
        {
    var path=ProceduralGeneratingAlgorithme.SimpleRandomwalk(currentPosition,walkLength);
    floorPositions.UnionWith(path);
    if(startRondmlyEachIteration)
        currentPosition=floorPositions.ElementAt(Random.Range(0,floorPositions.Count));
        }
        return floorPositions;
    }
}
