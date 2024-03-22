using TMPro;
using UnityEngine;

public class TeleporterMaster : MonoBehaviour
{
    public Vector2 teleportPosition;
    public float delayBeforeTeleport = 2f;

    public GameObject particlePrefab;
    private GameObject spawnedParticles; 

    private bool playerOnTeleporter = false;

    public bool hasMasterKey = false;

    private GameController gameController;
    public TMP_Text teleporterMasterError;

    private Animator animator;


    private void Start() {
        gameController = FindObjectOfType<GameController>();
        animator = GetComponent<Animator>();
        animator.speed = 0f;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && hasMasterKey)
        {
            FindObjectOfType<AudioManager>().Play("TeleportCharge");
            playerOnTeleporter = true;
            animator.speed = 1f;
            Invoke("TeleportPlayer", delayBeforeTeleport);

            if (particlePrefab != null)
            {
                spawnedParticles = Instantiate(particlePrefab, transform.position, Quaternion.identity);
            }
        }else if (other.CompareTag("Player") && !hasMasterKey)
        {
            teleporterMasterError.gameObject.SetActive(true); 
            animator.speed = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        teleporterMasterError.gameObject.SetActive(false); 
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
                animator.speed = 0f;
                player.transform.position = teleportPosition;
            }
            else
            {
                Debug.LogError("Kein Player lol?? Prdn??!.");
            }
        }
    }
}
