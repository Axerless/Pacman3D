using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform mainSpawnPoint;
    [SerializeField] private int enemyAmount;


    private List<Transform> spawnPointList = new List<Transform>();
    public List<GameObject> enemiesList;
    private GameObject enemy;
    private int enemyRandomInt;
    private int randomInt;


    void Start()
    {
        if(spawnPointList.Count != 0)
        {
            SpawnEnemies();
        }
    }

    //Setting ghost to react to playerPowerUp
    public void PlayerPowerUp(bool isOn)
    {
        for(int i = 0; i < enemyAmount; i++)
        {
            enemiesList[i].GetComponent<EnemyAI>().SetMaterial(isOn);
        }
    }

    //Stops all enemies on map
    public void StopEnemies()
    {
        for(int i = 0; i < enemyAmount; i++)
        {
            enemiesList[i].GetComponent<EnemyAI>().Stop();
        }
    }

    //Spawn points of enemies added to list to later enemy spawn on random selected spawn point
    public void AddSpawnPointList(Transform spawnPoint)
    {
        spawnPointList.Add(spawnPoint);
    }
    //For each enemy assigned, randomly sets spawn point from list and instantiate enemy at this point. Also remove spawn point from the list to prevent spawning on the same point
    void SpawnEnemies()
    {
        for(int i = 0; i < enemyAmount; i++)
        {
            randomInt = Random.Range(0, spawnPointList.Count);
            Transform enemySpawnPoint = spawnPointList[randomInt];

            enemyRandomInt = Random.Range(0, enemyPrefabs.Length);
            GameObject enemyObject = Instantiate(enemyPrefabs[enemyRandomInt], enemySpawnPoint.position, Quaternion.identity);
            enemiesList.Add(enemyObject);

            if(enemyAmount < spawnPointList.Count)
            {
                spawnPointList.Remove(enemySpawnPoint);
            }
        }   
    }
    public void RespawnEnemies()
    {
        for(int i = 0; i < enemiesList.Count; i++)
        {
            enemiesList[i].transform.position = mainSpawnPoint.position;
            enemiesList[i].GetComponent<EnemyAI>().Respawn();
        }
    }
}
