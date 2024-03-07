using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed;
    public Transform[] points;

    private Transform currentPoint;
    private Rigidbody2D rb;
    private int currentIndex = 0;
    private int direction = 1;
    private string axisMode;
    private string directionMode = "none";
    private bool isPatroling = true;
    
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPoint = points[0];
        GetNewMode(currentPoint);
        EventManager.StartListening("chaseStart", StopPatrol);
        EventManager.StartListening("chaseEnd", StartPatrol);
    }
    

    void Update()
    {
        if(isPatroling)
        {
            Vector2 movementDirection = Vector2.zero;

            if(axisMode == "horizontal")
            {
                float distanceX = transform.position.x - currentPoint.position.x;
                if (distanceX > 0)
                {
                    // move left
                    // Debug.Log("left");
                    movementDirection.x = -0.3f;
                }
                else 
                {
                    // move right
                    // Debug.Log("right");
                    movementDirection.x = 0.3f;
                }
            }
            else
            {
                float distanceY = transform.position.y - currentPoint.position.y;

                if (distanceY > 0)
                {
                    // move down
                    // Debug.Log("down");
                    movementDirection.y = -0.3f;
                }
                else 
                {
                    // move up
                    // Debug.Log("up");
                    movementDirection.y = 0.3f;
                }
            }

            rb.velocity = movementDirection * speed;

            //Debug.Log(Vector2.Distance(transform.position, currentPoint.position));
            // check if enemy is close to patrolpoint
            if (Vector2.Distance(transform.position, currentPoint.position) < 0.1f)
            {
            // change array direction to go route backwards
            if (currentIndex >= points.Length - 1 && direction == 1 || currentIndex <= 0 && direction == -1)
            {   
                direction *= -1;
            }
                // switch to next patrolpoint and change mode
                currentIndex += direction;
                currentPoint = points[currentIndex];
                GetNewMode(currentPoint);
            }
        }
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        if(directionMode == "left")
        {
            scale.x = 1;
        }
        else if(directionMode == "right")
        {
            scale.x = -1;
        }


        transform.localScale = scale;
    }

    void GetNewMode(Transform point)
    {
        float distanceX = transform.position.x - point.position.x;
        float distanceY = transform.position.y - point.position.y;

        if(Math.Abs(distanceX) > Math.Abs(distanceY))
        {
            axisMode = "horizontal";
            directionMode = distanceX > 0 ? "left" : "right";
        }
        else
        {
            axisMode = "vertical";
            directionMode = distanceY > 0 ? "down" : "up";
        }

        Flip();
    }

    void StartPatrol(Dictionary<string, object> message)
    {
        isPatroling = true;
    }

    void StopPatrol(Dictionary<string, object> message)
    {
        isPatroling = false;
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i < points.Length; i++)
        {
            Gizmos.DrawWireSphere(points[i].transform.position, 0.1f);
        }
    }
}
