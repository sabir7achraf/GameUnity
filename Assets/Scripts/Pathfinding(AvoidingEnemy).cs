using UnityEngine;
using System;
using System.Collections.Generic;

public class PathFinding : MonoBehaviour
{
    public Transform seeker, target;
    private Grida grid;
    public GameObject winBar;
    private FollowpathAstar pathFollower;  // Référence au script de suivi du chemin
    public float enemyAvoidanceRadius = 1.5f;  // Ajuster le rayon pour éviter les ennemis
    public int enemyProximityPenalty = 10;  // Pénalité pour les nœuds à proximité des ennemis
    public int unwalkableProximityPenalty = 5;
    private bool hasArrived = false;  // Pénalité pour les nœuds à proximité des nœuds non praticables
    void Awake()
    {
        grid = GetComponent<Grida>();
        pathFollower = seeker.GetComponent<FollowpathAstar>();  // Assurez-vous que le joueur a un script PathFollower
    }

    void Update()
    {
        FindAndFollowPath(seeker.position, target.position);
        CheckArrival();
    }

    public void FindAndFollowPath(Vector2 startPos, Vector2 targetPos)
    {
        if (startPos == null || targetPos == null)
        {
            Debug.LogError("Start or target position is null");
            return;
        }
        List<Node> path = FindPath(startPos, targetPos);
        if (path != null && path.Count > 0)
        {
            pathFollower.StartPath(path);  // Démarrer le suivi du chemin
        }
    }
    public List<Node> FindPath(Vector2 startPos, Vector2 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        if (!startNode.walkable || !targetNode.walkable)
        {
            Debug.LogError("StartNode or TargetNode is not walkable");
            return null;
        }

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }
       
        return null;
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();

        return path;
    }
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Math.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Math.Abs(nodeA.gridY - nodeB.gridY);

        int distance;
        if (dstX > dstY)
            distance = 14 * dstY + 10 * (dstX - dstY);
        else
            distance = 14 * dstX + 10 * (dstY - dstX);

       
        if (IsEnemyNear(nodeA))
        {
            distance += enemyProximityPenalty;
        }

        // Ajoutez un coût supplémentaire pour les nœuds qui sont à proximité des nœuds non praticables
        if (IsUnwalkableNear(nodeA))
        {
            distance += unwalkableProximityPenalty;
        }

        return distance;
    }

    bool IsEnemyNear(Node node)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(node.worldPosition, enemyAvoidanceRadius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                return true;
            }
        }
        return false;
    }
    void MarkUnwalkableNearNodes()
{
    for (int x = 0; x < grid.gridSizeX; x++)
    {
        for (int y = 0; y < grid.gridSizeY; y++)
        {
            Node node = grid.grid[x, y];
            if (grid.IsUnwalkableNear(node))
            {
                // Ajoutez la pénalité à la valeur de coût du nœud
                node.gCost += grid.unwalkableProximityPenalty;
            }
        }
    }
}

bool IsUnwalkableNear(Node node)
{
    for (int x = -2; x <= 2; x++)
    {
        for (int y = -2; y <= 2; y++)
        {
            int checkX = node.gridX + x;
            int checkY = node.gridY + y;

            if (checkX >= 0 && checkX < grid.gridSizeX && checkY >= 0 && checkY < grid.gridSizeY)
            {
                Node neighbor = grid.grid[checkX, checkY];
                if (!neighbor.walkable)
                {
                    return true;
                }
            }
        }
    }
    return false;
}
 void CheckArrival()
    {
        if (!hasArrived){
        float distanceToTarget = Vector3.Distance(seeker.position, target.position);
        if (distanceToTarget < 0.5f) 
        {

            Debug.Log("arrived!!");
            hasArrived = true;
            winBar.SetActive(true);
            gameObject.SetActive(false);
            

        }
    }
    }
}
