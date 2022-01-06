using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnPointsCount : MonoBehaviour
{
    private EnemySpawner enemySpawner;
    public void Awake()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        foreach(Transform child in transform)
        {
            enemySpawner.AddSpawnPointList(child);
        }
    }

}
