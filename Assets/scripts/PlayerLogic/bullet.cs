using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float life = 3f;
    public int damage = 20;

    private void Awake()
    {
        Destroy(gameObject, life);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
                if(other.gameObject.TryGetComponent<Health>(out var healthComponent))
                {
                    healthComponent.TakeDamage(damage);
                    Debug.Log(healthComponent.currentHealth);
                    if(healthComponent.currentHealth <= 0)
                    {
                        Destroy(other.gameObject);
                    }
                }
        }

        if(other.gameObject.CompareTag("Player")){
            Debug.Log("pass");
        }else {
            Destroy(gameObject);
        }
    }
}
