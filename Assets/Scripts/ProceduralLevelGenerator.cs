using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralLevelGenerator : MonoBehaviour
{
    public List<GameObject> goToSpawnList = new List<GameObject>();
    void Awake()
    {
        GameObject gameObjectToSpawn = goToSpawnList[Random.Range(0,goToSpawnList.Count)];
        Instantiate(gameObjectToSpawn, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
