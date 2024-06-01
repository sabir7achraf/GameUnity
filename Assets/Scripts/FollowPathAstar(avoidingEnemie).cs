using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowpathAstar : MonoBehaviour
{
    public float speed = 5f;
    public float obstacleDetectionRadius = 0.5f; // Rayon pour détecter les obstacles
    public float enemyDetectionRadius = 1f; // Rayon pour détecter les ennemis
    private List<Node> path;
    private int currentPathIndex;
    private bool isFollowingPath;
    private Grida grid; // Référence à la grille

    void Awake()
    {
        grid = FindObjectOfType<Grida>(); // Assurez-vous que la grille est référencée
    }

    void Update()
    {
        if (isFollowingPath && path != null && currentPathIndex < path.Count)
        {
            MovePlayerAlongPath();
        }
    }

    public void StartPath(List<Node> newPath)
    {
        path = newPath;
        currentPathIndex = 0;
        isFollowingPath = true;
        Debug.Log("Starting path with " + path.Count + " nodes.");
    }

    void MovePlayerAlongPath()
{
    if (path == null || currentPathIndex >= path.Count)
        return;

    Vector3 targetPosition = path[currentPathIndex].worldPosition;
    Vector3 directionToTarget = (targetPosition - transform.position).normalized;
    Vector3 velocity = directionToTarget * speed;
    transform.position += velocity * Time.deltaTime;

    // Si un ennemi est détecté sur le chemin, recalculez le chemin
    if (IsEnemyInPath(directionToTarget, enemyDetectionRadius))
    {
        RecalculatePath();
    }

    // Si le joueur a atteint le nœud cible, passez au nœud suivant
    if (Vector3.Distance(transform.position, targetPosition) <= 0.1f)
    {
        currentPathIndex++;
    }
}

   bool IsEnemyInPath(Vector3 direction, float detectionRadius)
{
    // Lancez un raycast dans la direction du mouvement du joueur
    RaycastHit hit;
    if (Physics.Raycast(transform.position, direction, out hit, detectionRadius))
    {
        // Si le raycast a touché un ennemi, renvoyez true
        if (hit.collider.CompareTag("Enemy"))
        {
            return true;
        }
    }

    // Si aucun ennemi n'a été détecté, renvoyez false
    return false;
}

    void RecalculatePath()
    {
        PathFinding pathfinding = FindObjectOfType<PathFinding>();
        pathfinding.FindAndFollowPath(transform.position, pathfinding.target.position);
    }
}
