using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    public SpriteRenderer spriteRenderer;

    protected Rigidbody2D rb;
    protected Vector2 initialPosition;
    protected Color originalColor;
    protected bool hasLineOfSight;

    public GameObject particlePrefab;
    private GameObject deathParticles; 

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;

        EventManager.StartListening("death", CheckForDeath);
        EventManager.StartListening("respawnAll", Respawn);
    }

    protected virtual void FixedUpdate()
    {
        CheckIfPlayerIsInLineOfSight();
    }

    protected virtual void Respawn(Dictionary<string, object> message)
    {
        if(!transform.gameObject.activeSelf)
        {
            transform.gameObject.SetActive(true);
            StartCoroutine(Reset());
        }
    }

    protected virtual IEnumerator Reset()
    {
        rb.position = initialPosition;
        yield return new WaitForSeconds(0.1f);
        
        spriteRenderer.material.color = originalColor;
        GetComponent<Health>().ResetHealth();
    }


    protected virtual void CheckForDeath(Dictionary<string, object> message)
    {
        if((GameObject)message["gameobject"] == transform.gameObject)
        {
            StartCoroutine(Die(1));
            deathParticles = Instantiate(particlePrefab, transform.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("MazeDeath");
            StartCoroutine(DestroyParticlesAfterDelay());
        }
    }

    IEnumerator DestroyParticlesAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        Destroy(deathParticles);
    }

    protected virtual IEnumerator Die(int secs)
    {
        Color objectColor = spriteRenderer.material.color;
        originalColor = objectColor;
        objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, 0);
        spriteRenderer.material.color = objectColor;
        transform.gameObject.SetActive(false);
        yield return null;
    }

    
    void CheckIfPlayerIsInLineOfSight()
    {
        RaycastHit2D ray = Physics2D.Linecast(transform.position, target.transform.position, 1 << LayerMask.NameToLayer("Obstacles"));

        if(ray.collider != null)
        {
            hasLineOfSight = false;
            Debug.DrawLine(transform.position, target.transform.position, Color.red);
        } else 
        {
            hasLineOfSight = true;
            Debug.DrawLine(transform.position, target.transform.position, Color.green);
        }
    }
}
