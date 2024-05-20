using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPathAstar : MonoBehaviour
{
    public float speed = 5f;  // Vitesse de déplacement du joueur
    private List<Node> path;  // Chemin à suivre
    private int targetIndex;  // Indice du noeud cible actuel

    void Start()
    {
        path = new List<Node>();
    }

    public void StartPath(List<Node> newPath)
    {
        if (newPath != null)
        {
            path = newPath;
            targetIndex = 0;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        if (path.Count == 0)
            yield break;

        Vector3 currentWaypoint = path[0].worldPosition;
        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Count)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex].worldPosition;
            }

            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
    }
}
