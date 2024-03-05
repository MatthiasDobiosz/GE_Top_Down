using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float life = 3f;

    private void Awake() {
        Destroy(gameObject, life);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Destroy(other.gameObject);
        Destroy(gameObject);
    }
}
