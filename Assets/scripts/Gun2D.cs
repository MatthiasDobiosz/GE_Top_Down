using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun2D : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10f;

    private Player player;

    private void Start()
    {
        player = GetComponentInParent<Player>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector2 shootDirection = GetShootDirection();
            ShootBullet(shootDirection);
        }
    }

    private Vector2 GetShootDirection()
    {
        Vector2 inputDirection = player.GetMovementInput().normalized;

        if (inputDirection == Vector2.zero)
        {
            return Vector2.up;
        }

        float horizontalInput = inputDirection.x;
        float verticalInput = inputDirection.y;

        if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
        {
            if (horizontalInput > 0)
                return Vector2.right;
            else
                return Vector2.left;
        }
        else
        {
            if (verticalInput > 0)
            {
                if (horizontalInput > 0)
                    return new Vector2(1, 1);
                else if (horizontalInput < 0)
                    return new Vector2(-1, 1);
                else
                    return Vector2.up;
            }
            else if (verticalInput < 0)
            {
                if (horizontalInput > 0)
                    return new Vector2(1, -1);
                else if (horizontalInput < 0)
                    return new Vector2(-1, -1);
                else
                    return Vector2.down;
            }
        }
        return Vector2.zero;
    }

    private void ShootBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
    }
}
