using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class PlayerMovement : MonoBehaviour
{
    public Animator cameraStateAnimator;
    public AlertInfo powerUpAlert;
    public Image[] playerLives;
    public Transform playerSpawnPoint;
    public Transform cameraTransform;
    public EnemySpawner enemySpawner;
    public GameObject playerDieVFXPrefab;
    public PauseMenu pauseMenu;
    public bool isDead{get; private set;}
    public bool isPowerUpActive{get; private set;}
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float smoothTime = 0.1f;

    private Volume globalVolume;
    private Animator animator;
    private CharacterController characterController;
    private Vector3 direction;
    private float powerUpTime;
    private float xRotation;
    private float yAxis;
    private float xAxis;
    private float turnSmoothVelocity;
    private bool isGrounded;
    private bool canMove;
    private bool canStand;
    private bool firstPersonCamera = false;
    private int lives = 3;

    

    void Awake()
    {
        if(enemySpawner == null)
        {   
            enemySpawner = FindObjectOfType<EnemySpawner>();
        }
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        globalVolume = FindObjectOfType<Volume>();
    }

    void Start()
    {
        if(playerSpawnPoint == null)
        {
            playerSpawnPoint = GameObject.FindGameObjectWithTag("PlayerSpawn").GetComponent<Transform>();
        }
        transform.position = playerSpawnPoint.position;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.C))
        {
            SwitchCameraState();
        }
       
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(!isDead)
        {
            if(direction.magnitude >= 0.1f)
            {
                animator.SetBool("Walk", true);
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, smoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                characterController.Move(moveDir.normalized * movementSpeed * Time.deltaTime);
            }
            else 
            {
                animator.SetBool("Walk", false);
            }
        }

        if(isPowerUpActive)
        {
            if(Time.time >= powerUpTime)
            {
                isPowerUpActive = false;
                enemySpawner.PlayerPowerUp(false);
            }
        }
        
    }

    public void SwitchCameraState()
    {
        if(!firstPersonCamera)
        {
            cameraStateAnimator.Play("FPPlayerCamera");
            firstPersonCamera = true;
        }
        else
        {
            cameraStateAnimator.Play("TPPlayerCamera");
            firstPersonCamera = false;
        }
    }

    public void SetPowerUp()
    {
        powerUpTime = Time.time + 10f;
        isPowerUpActive = true;
        enemySpawner.PlayerPowerUp(true);
        powerUpAlert.SetAlertInfo("Power Up Collected!");
        animator.SetTrigger("PowerUp");
    }

    //Player Die and respawn is called with delay of 2sec
    public void Die()
    {
        if(!isDead)
        {
            StartCoroutine(PlayerDeath(2.0f));
        }
    }

    IEnumerator PlayerDeath(float respawnTime)
    {
        isDead = true;
        AudioManager.Instance.Play("PlayerDeath");
        animator.SetTrigger("Death");
        if(globalVolume.profile.TryGet(out DepthOfField depthOfField))
        {
            depthOfField.active = true;
        }
        
        GameObject playerDieVFX = Instantiate(playerDieVFXPrefab, transform.position, Quaternion.identity);
        Destroy(playerDieVFX, 1f);

        yield return new WaitForSeconds(respawnTime);

        if(lives > 0)
        {
            if(globalVolume.profile.TryGet(out DepthOfField depthOfFieId))
            {
                depthOfField.active = false;
            }
            enemySpawner.RespawnEnemies();
            transform.position = playerSpawnPoint.position;
            playerLives[lives-1].enabled = false;
            lives--;
            isDead = false;
        }
        else
        {
            pauseMenu.LevelCompletePanel(false);
        }
    }
}
