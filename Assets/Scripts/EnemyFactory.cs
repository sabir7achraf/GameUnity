using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyFactory : MonoBehaviour
{
    
    [SerializeField] private GameObject enemyPrefab;

    public GameObject CreateEnemy(Vector3 position, Transform parent)
    {
      
        GameObject newEnemy = Instantiate(enemyPrefab, position, Quaternion.identity, parent);
        return newEnemy;
    }
}