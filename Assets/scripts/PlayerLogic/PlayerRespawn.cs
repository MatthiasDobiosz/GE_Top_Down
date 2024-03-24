using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float fadeSpeed;
    public static Vector2 lastCheckpointPosition;

    private Rigidbody2D rb;

    public GameObject particlePrefab;

    private GameObject deathParticles; 



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        EventManager.StartListening("death", CheckForDeath);
    }

    void RepositionPlayer()
    {
        rb.position = lastCheckpointPosition;
    }

    void CheckForDeath(Dictionary<string, object> message)
    {
        if((GameObject)message["gameobject"] == transform.gameObject)
        {
            //anim.SetTrigger("Death");
            EventManager.TriggerEvent("playerDeath", null);
            rb.bodyType = RigidbodyType2D.Static;
            StartCoroutine(FadeOut());
            deathParticles = Instantiate(particlePrefab, transform.position, Quaternion.identity);
            FindObjectOfType<AudioManager>().Play("PlayerDeath");
            StartCoroutine(DestroyParticlesAfterDelay());
        }
    }

    IEnumerator DestroyParticlesAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        Destroy(deathParticles);
    }

    // https://owlcation.com/stem/How-to-fade-out-a-GameObject-in-Unity
    IEnumerator FadeOut()
    {        
        while (spriteRenderer.material.color.a > 0)
        {
            Color objectColor = spriteRenderer.material.color;
            float fadeAmount = objectColor.a - (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            spriteRenderer.material.color = objectColor;
            yield return null;
        }   

        yield return new WaitForSeconds(1);

        StartCoroutine(FadeIn());
    }

    // https://owlcation.com/stem/How-to-fade-out-a-GameObject-in-Unity
    IEnumerator FadeIn()
    {        
        rb.bodyType = RigidbodyType2D.Kinematic;
        RepositionPlayer();

        while (spriteRenderer.material.color.a < 1)
        {
            Color objectColor = spriteRenderer.material.color;
            float fadeAmount = objectColor.a + (fadeSpeed * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            spriteRenderer.material.color = objectColor;
            yield return null;
        }

        GetComponent<Health>().ResetHealth();
        
        EventManager.TriggerEvent("playerRespawn", null);
        EventManager.TriggerEvent("respawnAll", null);
    }
}