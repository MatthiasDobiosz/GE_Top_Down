using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyBullet : MonoBehaviour
{
    public float life = 3f;
    public int damage = 10;

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

        if(other.gameObject.CompareTag("Enemy")){
            Debug.Log("pass");
        }else {
            Destroy(gameObject);
        }
    }
}
