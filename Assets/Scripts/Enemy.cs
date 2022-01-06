using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    PatrolingEnemy,
    ChasingEnemy,
}

[CreateAssetMenu]
public class Enemy : ScriptableObject
{
    public new string name;
    public Material mainMaterial;
    public Material debuffMaterial;
    public EnemyType enemyType;

    [Space(10)]
    public float speed;
    public float sightRange;
    public float walkPointRange;

    
    public string GetEnemyType()
    {
        return enemyType.ToString();
    }


}

