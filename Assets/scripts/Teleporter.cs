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
    public TMP_Text teleporterError;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && allKeyFragments)
        {
            FindObjectOfType<AudioManager>().Play("TeleportCharge");
            playerOnTeleporter = true;
            Invoke("TeleportPlayer", delayBeforeTeleport);

            if (particlePrefab != null)
            {
                spawnedParticles = Instantiate(particlePrefab, transform.position, Quaternion.identity);
            }
        }else if (other.CompareTag("Player") && !allKeyFragments)
        {
            teleporterError.gameObject.SetActive(true); 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        teleporterError.gameObject.SetActive(false); 
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<AudioManager>().Cancel("TeleportCharge");
            playerOnTeleporter = false;
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
            }
            else
            {
                Debug.LogError("Kein Player lol?? Prdn??!.");
            }
        }
    }
}
