using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun2D : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;

    private Player player;
    private Vector2 lastMovementInput;
    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {   
            lastMovementInput = player.lastMovementInput;
            Vector2 shootDirection = GetShootDirection();
            ShootBullet(shootDirection);
        }
    }

    private Vector2 GetShootDirection()
    {
        float horizontalInput = lastMovementInput.x;
        float verticalInput = lastMovementInput.y;

        if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
        {
            if (horizontalInput > 0)
                lastMovementInput = Vector2.right;
            else
                lastMovementInput = Vector2.left;
        }
        else
        {
            if (verticalInput > 0)
            {
                if (horizontalInput > 0)
                    lastMovementInput = new Vector2(1, 1);
                else if (horizontalInput < 0)
                    lastMovementInput = new Vector2(-1, 1);
                else
                    lastMovementInput = Vector2.up;
            }
            else if (verticalInput < 0)
            {
                if (horizontalInput > 0)
                    lastMovementInput = new Vector2(1, -1);
                else if (horizontalInput < 0)
                    lastMovementInput = new Vector2(-1, -1);
                else
                    lastMovementInput = Vector2.down;
            }
        }
        return lastMovementInput;
    }

    private void ShootBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
    }
}
