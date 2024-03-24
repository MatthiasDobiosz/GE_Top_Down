using System;
using TMPro;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Vector2 teleportPosition;
    public float delayBeforeTeleport = 2f;

    public GameObject particlePrefab;
    private GameObject spawnedParticles; 

    private bool playerOnTeleporter = false;

    public bool allKeyFragments = false;

    private GameController gameController;
    public TMP_Text teleporterError;

    private Animator animator;
    public GameObject teleporterEbene1;
    public GameObject teleporterEbene2;
    public GameObject teleporterEbene3;
    private int currentTeleporterCount = 0;

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
                teleporterError.gameObject.SetActive(true); 
                animator.speed = 0f;
            }
            else
            {
                FindObjectOfType<AudioManager>().Play("TeleportCharge");
                playerOnTeleporter = true;
                animator.speed = 1f;
                Invoke("TeleportPlayer", delayBeforeTeleport);

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
                player.transform.position = teleportPosition;
                animator.speed = 0f;
                if(currentTeleporterCount == 0){
                    teleporterEbene1.SetActive(false);
                    teleporterEbene2.SetActive(true);
                } else if(currentTeleporterCount == 1){
                    teleporterEbene2.SetActive(false);
                    teleporterEbene3.SetActive(true);
                }
                currentTeleporterCount++;
                gameController.collectedCount = 0;
                gameController.UpdateCollectedText();
            }
            else
            {
                Debug.LogError("Kein Player lol?? Prdn??!.");
            }
        }
    }

}
