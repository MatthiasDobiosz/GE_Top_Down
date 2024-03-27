using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Teleporter : MonoBehaviour
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
                
                if(targetObject)
                {
                    player.transform.position = targetObject.transform.position;
                }
                else 
                {
                    player.transform.position = teleportPosition;
                }
                FindObjectOfType<ProgressBar>().IncrementProgress(0.2f);
                EventManager.TriggerEvent("ShowPopup1", null);
                animator.speed = 0f;
                gameController.collectedCount = 0;
                gameController.UpdateCollectedText();
                
                if(destinationLayer != 0)
                {
                    EventManager.TriggerEvent("teleport", new Dictionary<string, object>{
                        {"layer", destinationLayer-1},
                    });
                } else
                {
                    EventManager.TriggerEvent("teleportLast", null);
                }
                
            }
            else
            {
                Debug.LogError("Kein Player lol?? Prdn??!.");
            }
        }
    }

}
