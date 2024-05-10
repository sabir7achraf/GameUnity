using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
private Tilemap floorTilmap,WallTilmap;
 [SerializeField]
private TileBase floorTile,WallTop;
public void PainFlorTiles(IEnumerable<Vector2Int> floorPositons){
    PaintTiles(floorPositons,floorTilmap,floorTile);
}
private void PaintTiles (IEnumerable<Vector2Int> positions,Tilemap tilmape,TileBase tile){
    foreach(var position in positions){
        PainSingleTitle(tilmape,tile,position);
    }
}
private void  PainSingleTitle ( Tilemap tilmape,TileBase tile ,Vector2Int position){
  var tilePosition = tilmape.WorldToCell((Vector3Int)position);
  tilmape.SetTile(tilePosition,tile);
}
internal  void PaintSingleBasicWall( Vector2Int position){
  PainSingleTitle (WallTilmap,WallTop,position);
}
public void clear(){
    floorTilmap.ClearAllTiles();
    WallTilmap.ClearAllTiles();
}
}
