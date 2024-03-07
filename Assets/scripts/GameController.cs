using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Vector2 startPos;
    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        Debug.Log(startPos);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other);
        if(other.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    void Die(){
        Respawn();
    }

    void Respawn(){
        transform.position = startPos;
    }
}
