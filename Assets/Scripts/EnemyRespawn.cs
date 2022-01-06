using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawn : MonoBehaviour
{
    private EnemyAI enemyAI;
    void OnTriggerEnter(Collider other)
    {
        enemyAI = other.gameObject.GetComponent<EnemyAI>();
        if(enemyAI)
        {
            enemyAI.Respawn();
        }
    }
}
