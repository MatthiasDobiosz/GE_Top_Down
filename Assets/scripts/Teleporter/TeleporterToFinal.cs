using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
    Seperate logic for the final teleporter
*/
public class TeleporterToFinal : MonoBehaviour
{

    public Vector2 teleportPosition;
    public GameObject targetObject;
    public float delayBeforeTeleport = 2f;
    public int destinationLayer = 0;
    public bool allKeyFragments = false;
    public GameObject particlePrefab;
    public TMP_Text teleporterError;

    private GameObject spawnedParticles; 
    private bool playerOnTeleporter = false;
    private GameController gameController;
    private Animator animator;

    private void Start() {
        gameController = FindObjectOfType<GameController>();
        animator = GetComponent<Animator>();
        animator.speed = 0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!allKeyFragments)
            {
                teleporterError.text = "You dont have enough batteries to use the teleporter!";
                teleporterError.gameObject.SetActive(true); 
                animator.speed = 0f;
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("TeleportCharge");
                playerOnTeleporter = true;
                animator.speed = 1f;
                Invoke("TeleportPlayer", delayBeforeTeleport);
                EventManager.TriggerEvent("CloseTofinish", null);
                if (particlePrefab != null)
                {
                    spawnedParticles = Instantiate(particlePrefab, transform.position, Quaternion.identity);
                }
            }
        }
    }

    public void AllKeyFragmentsCollected()
    {
        allKeyFragments = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        teleporterError.gameObject.SetActive(false); 
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<AudioManager>().Cancel("TeleportCharge");
            playerOnTeleporter = false;
            animator.speed = 0f;
            CancelInvoke("TeleportPlayer");

            if (spawnedParticles != null)
            {
                Destroy(spawnedParticles);
            }
        }
    }

    private void TeleportPlayer()
    {
        if (playerOnTeleporter)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                FindObjectOfType<AudioManager>().Play("Teleport");
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                rb.velocity = Vector2.zero;

                
                if(targetObject)
                {
                    player.transform.position = targetObject.transform.position;
                }
                else 
                {
                    player.transform.position = teleportPosition;
                }
                
                animator.speed = 0f;
                gameController.collectedCount = 0;
                gameController.UpdateCollectedText();

                if(FindObjectOfType<ProgressBar>())
                {
                    FindObjectOfType<ProgressBar>().IncrementProgress(0.2f);
                }
                
                EventManager.TriggerEvent("teleportFinal", null);
                EventManager.TriggerEvent("ShowPopup4", null);
            }
            else
            {
                Debug.LogError("No player found");
            }
        }
    }
}
