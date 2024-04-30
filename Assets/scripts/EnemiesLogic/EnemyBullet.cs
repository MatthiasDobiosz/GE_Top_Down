using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Enemy bullet instance
*/
public class EnemyBullet : MonoBehaviour
{
    public float life = 0.05f;
    public int damage = 10;
    public float maxDistance = 3f;

    private void Awake()
    {
        Destroy(gameObject, life);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if(other.gameObject.TryGetComponent<Health>(out var healthComponent))
            {
                healthComponent.TakeDamage(damage);
            }
        }

        if(!other.gameObject.CompareTag("Enemy")){
            Destroy(gameObject);
        }
    }
}
