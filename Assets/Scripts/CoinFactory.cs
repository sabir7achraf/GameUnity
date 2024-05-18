using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinFactory : MonoBehaviour
{
    
    [SerializeField] private GameObject coin;

    public GameObject CreateCoin(Vector3 position, Transform parent)
    {
        GameObject Coins = Instantiate(coin, position, Quaternion.identity, parent);
        return Coins;
    }
}