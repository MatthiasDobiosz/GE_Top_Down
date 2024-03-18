using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Basic ranged attack: will shoot bullet directly at the target
*/
public class RangedStandardAttack : MonoBehaviour
{
    public int damage = 20;
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;

    public Vector2 TriggerAttackStart(Rigidbody2D rb, Transform target)
    {
        EventManager.TriggerEvent("attackStart", new Dictionary<string, object> {
            {"body", rb}
        });
        FindObjectOfType<AudioManager>().Play("EnemyShooting");
        Vector2 playerPosition = new(target.position.x, target.position.y);
        Vector2 monsterPosition = new(rb.position.x, rb.position.y);
        Vector2 direction = playerPosition - monsterPosition;

        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.GetComponent<EnemyBullet>().damage = damage;
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());

        return direction;
    }

    public void TriggerAttackEnd(Rigidbody2D rb)
    {
        EventManager.TriggerEvent("attackEnd", new Dictionary<string, object> {
            {"body", rb}
        });
    }
}
