using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotCollection : MonoBehaviour
{
    public GameObject CollectVFXPrefab;
    [SerializeField] private bool isPowerUp;

    private PlayerMovement playerMovement;

    void Start()
    {
        if(isPowerUp)
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            AudioManager.Instance.Play("DotCollect");
            ScoreManager.Instance.SetScore();
            GameObject collectVFX = Instantiate(CollectVFXPrefab, transform.position, Quaternion.identity);
            if(isPowerUp)
            {
                playerMovement.SetPowerUp();
            }
            Destroy(collectVFX, 1.2f);
            gameObject.SetActive(false);
        }
    }
}
