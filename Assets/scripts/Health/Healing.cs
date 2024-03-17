using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Healing : MonoBehaviour
{
    public int healValue;
    public GameObject particles;


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if(collider.gameObject.TryGetComponent<Health>(out var healthComponent))
            {
                if(healthComponent.currentHealth != healthComponent.maxHealth)
                {
                    healthComponent.Heal(healValue);
                    Destroy(transform.gameObject);
                    Instantiate(particles, transform.position, Quaternion.identity);
                    FindObjectOfType<AudioManager>().Play("Heal");
                }
            }
        }
    }
}
