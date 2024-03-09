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
            Vector2 playerDirection = GetPlayerDirection();
            Debug.Log(playerDirection);
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = playerDirection * bulletSpeed;
        }
    }

    private Vector2 GetPlayerDirection()
    {
        Vector2 inputDirection = player.GetMovementInput();

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
            else
            {
                if (horizontalInput > 0)
                    return Vector2.right;
                else if (horizontalInput < 0)
                    return Vector2.left;
                else
                    return Vector2.zero;
            }
        }
    }
}
