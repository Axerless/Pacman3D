using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Enemy enemy;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsGround;
    public MeshRenderer meshRenderer;
    public Vector3 walkPoint;
    public bool isEnemyDead{get; private set;}

    
    private Animator animator;
    private Transform targetToFollow;
    private Transform playerTransform;
    private PlayerMovement playerMovement;
    private NavMeshAgent agent;
    private EnemySpawner enemySpawner;
    private Material debuffMaterial;
    private Material material;
    private int enemiesDeadCount;
    private float sightRange;
    private float walkPointRange;
    private bool isPlayerInSightRange;
    private bool walkPointSet;
    private bool canMove = true;
  

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(!playerMovement.isPowerUpActive)
            {
                if(!playerMovement.isDead && !isEnemyDead)
                {   
                    KillPlayer();
                }
            }
            else
            {
                if(!isEnemyDead)
                {
                    KillEnemy();
                }
            }
        }
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerTransform = playerMovement.GetComponent<Transform>();
        enemySpawner = FindObjectOfType<EnemySpawner>();
        animator = GetComponent<Animator>();
        AudioManager.Instance.Play("GhostEnemy");
        
        targetToFollow = playerTransform;
        agent.speed = enemy.speed;
        debuffMaterial = enemy.debuffMaterial;
        material = enemy.mainMaterial;
        sightRange = enemy.sightRange;
        walkPointRange = enemy.walkPointRange;
        meshRenderer.material = material;
    }
    
    private void Update()
    {
        //Checking if playesr is in range
        isPlayerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);

        if(canMove)
        {
            if(enemy.GetEnemyType() == "PatrolingEnemy")
            {
                if(!isPlayerInSightRange) 
                {
                    Patroling();
                }
                else 
                {
                    ChasePlayer();
                }
            }
            else if(enemy.GetEnemyType() == "ChasingEnemy")
            {
                ChasePlayer();
            }
        }
    }
    private void Patroling()
    {
        if(!walkPointSet) SearchWalkPoint();

        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
           

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in setted range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }   
    }
    public void SetPlayer(PlayerMovement playerMov)
    {
        playerMovement = playerMov;
    }
    public void SetMaterial(bool isPowerUp)
    {
        if(isPowerUp)
        {
            animator.enabled = false;
            meshRenderer.material = debuffMaterial;
        }
        else
        {
            animator.enabled = true;
        }
    }
    private void ChasePlayer()
    {
        agent.destination = targetToFollow.position;
    }
    public void Stop()
    {
        canMove = false;
        agent.enabled = false;
    }
    public void KillPlayer()
    {
        playerMovement.Die();
        AudioManager.Instance.StopPlaying("GhostEnemy");
    }
    private void KillEnemy()
    {
        agent.speed = enemy.speed+4;
        meshRenderer.material = material;
        animator.enabled = true;
        animator.SetTrigger("Death");
        targetToFollow = enemySpawner.mainSpawnPoint;
        isEnemyDead = true;
        //Checkign loop for each enemy signed in enemies list
        for(int i = 0; i < enemySpawner.enemiesList.Count; i++)
        {
            //If enemy is dead add enemiesDeadCount
            if(enemySpawner.enemiesList[i].GetComponent<EnemyAI>().isEnemyDead)
            {
                enemiesDeadCount++;
            }
        }
        //If all enemies are dead, stop playing enemies sound
        if(enemiesDeadCount >= enemySpawner.enemiesList.Count)
        {
            AudioManager.Instance.StopPlaying("GhostEnemy");
        }
    }
    public void Respawn()
    {
        if(isEnemyDead)
        {   
            agent.speed = enemy.speed;
            AudioManager.Instance.Play("GhostEnemy");
            animator.SetTrigger("Respawn");
            targetToFollow = playerTransform;
            isEnemyDead = false;
        }
    }

    private void OnDrawGizmosSelected()
    {   
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }
}