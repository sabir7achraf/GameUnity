using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraPathfinding : MonoBehaviour
{
    public Transform player; // Le point de départ
    public Transform boss; // Le point d'arrivée
    GridA grid;

    void Awake()
    {
        grid = GetComponent<GridA>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            List<Node> path = DjikstraFindPath(player.position, boss.position);
            if (path != null)
            {
                // Mettez à jour le chemin dans la grille
                grid.SetPath(path);

                // Convertir le chemin en une liste de Vector3 pour le script FollowDjikstra
                List<Vector3> waypoints = new List<Vector3>();
                foreach (Node node in path)
                {
                    waypoints.Add(grid.WorldPointFromNode(node));
                }

                // Passez le chemin trouvé au script FollowDjikstra
                FindObjectOfType<FollowDjikstra>().SetPath(waypoints);
            }
        }
    }

    List<Node> DjikstraFindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        startNode.gCost = 0;
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].gCost < currentNode.gCost)
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
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }
        }

        return null; // Aucun chemin trouvé
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
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);
        return dstX + dstY;
    }
}
