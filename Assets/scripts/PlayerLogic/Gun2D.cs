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
    private AudioManager audioManager;

    private void Start()
    {
        player = GetComponentInParent<Player>();
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {   
            lastMovementInput = player.lastMovementInput;
            Vector2 shootDirection = GetShootDirection();
            audioManager.Play("PlayerShooting");
            ShootBullet(shootDirection);
        }
    }

    private Vector2 GetShootDirection()
    {
        float horizontalInput = lastMovementInput.x;
        float verticalInput = lastMovementInput.y;

        if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
        {
            if (horizontalInput > 0){
                lastMovementInput = Vector2.right;
                AdjustBulletSpawnPointPosition(-0.48f, 1.6f);
            }
            else{
                lastMovementInput = Vector2.left;
                AdjustBulletSpawnPointPosition(-0.53f, -1.24f);
                }
        }
        else
        {
            if (verticalInput > 0)
            {
                if (horizontalInput > 0){
                    lastMovementInput = new Vector2(1, 1);
                    AdjustBulletSpawnPointPosition(-1.04f, 0.95f);
                }
                else if (horizontalInput < 0){
                    lastMovementInput = new Vector2(-1, 1);
                    AdjustBulletSpawnPointPosition(-1.12f, -0.87f);
                }
                else{
                    lastMovementInput = Vector2.up;
                    AdjustBulletSpawnPointPosition(-1.74f, 0.02f);
                }
            }
            else if (verticalInput < 0)
            {
                if (horizontalInput > 0){
                    lastMovementInput = new Vector2(1, -1);
                    AdjustBulletSpawnPointPosition(0.25f, 1f);
                }
                else if (horizontalInput < 0){
                    lastMovementInput = new Vector2(-1, -1);
                    AdjustBulletSpawnPointPosition(0.13f, -1.04f);
                }
                else{
                    lastMovementInput = Vector2.down;
                    AdjustBulletSpawnPointPosition(1.8f, -0.1f);
                }
            }
        }
        return lastMovementInput;
    }

    private void AdjustBulletSpawnPointPosition(float x, float y)
    {
        Vector2 newPosition = Vector2.zero;
        newPosition.x = x;
        newPosition.y = y;
        bulletSpawnPoint.localPosition = newPosition;
    }

    private void ShootBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), player.GetComponent<Collider2D>());
    }   
}
