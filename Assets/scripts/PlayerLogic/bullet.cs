using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float life = 0.05f;
    public float maxDistance = 3f;
    private float distanceTraveled = 0f;
    public int damage = 20;

    private void Awake()
    {
        Destroy(gameObject, life);
    }
    
    private void Update()
    {
        distanceTraveled += Time.deltaTime * GetComponent<Rigidbody2D>().velocity.magnitude;
        if (distanceTraveled >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
                if(other.gameObject.TryGetComponent<Health>(out var healthComponent))
                {
                    healthComponent.TakeDamage(damage);
                }
        }

        if(other.gameObject.CompareTag("Player")){
            Debug.Log("pass");
        }else {
            Destroy(gameObject);
        }
    }
}
