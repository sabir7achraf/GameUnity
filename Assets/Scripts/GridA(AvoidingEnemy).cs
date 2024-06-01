using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grida : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public Node[,] grid;

    float nodeDiameter;
    public int gridSizeX, gridSizeY;
    public int unwalkableProximityPenalty = 50;

    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    void CreateGrid()
    {
        
        grid = new Node[gridSizeX, gridSizeY];
        Vector2 worldBottomLeft = (Vector2)transform.position;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
                if (!walkable)
                {
                    Debug.Log($"Node at ({x},{y}) is not walkable.");
                }
                else
                {
                    Debug.Log($"Node at ({x},{y}) is walkable.");
                }
            }
        }
    }

    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x - transform.position.x) / gridWorldSize.x;
        float percentY = (worldPosition.y - transform.position.y) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);
        return grid[x, y];
    }

    public List<Node> path;
    // void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireCube(transform.position + Vector3.right * gridWorldSize.x / 2 + Vector3.up * gridWorldSize.y / 2, new Vector3(gridWorldSize.x, gridWorldSize.y, 1));

    //     if (grid != null)
    //     {
    //         foreach (Node n in grid)
    //         {
    //             Gizmos.color = (n.walkable) ? Color.white : Color.red;
    //             if (path != null)
    //                 if (path.Contains(n))
    //                     Gizmos.color = Color.black;
    //             Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
    //         }
    //     }
    // }

    public Vector3 WorldPointFromNode(Node node)
    {
        return node.worldPosition;
    }
    
    void MarkUnwalkableNearNodes()
{
    for (int x = 0; x < gridSizeX; x++)
    {
        for (int y = 0; y < gridSizeY; y++)
        {
            Node node = grid[x, y];
            if (IsUnwalkableNear(node))
            {
                // Ajoutez la pénalité à la valeur de coût du nœud
                node.gCost += unwalkableProximityPenalty;
            }
        }
    }
}

public bool IsUnwalkableNear(Node node)
{
    // Vérifiez les nœuds qui sont à une plus grande distance
    for (int x = -2; x <= 2; x++)
    {
        for (int y = -2; y <= 2; y++)
        {
            int checkX = node.gridX + x;
            int checkY = node.gridY + y;

            if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
            {
                Node neighbor = grid[checkX, checkY];
                if (!neighbor.walkable)
                {
                    return true;
                }
            }
        }
    }

    return false;
}
}
