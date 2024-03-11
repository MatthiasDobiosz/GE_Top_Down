using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

// Basic Logic for the script taken by: https://www.youtube.com/watch?v=jvtFUfJ6CP8

/**
    Logic for an Enemy that will Patrol between a given array of points
*/
public class EnemyAIPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float speed = 2f;
    public float nextWaypointDistance = 0.1f;

    private Path path;
    private int currentWaypoint = 0;
    // private bool reachedEndOfPath = false;

    private Seeker seeker;
    private Rigidbody2D rb;
    private Transform currentPoint;
    private int currentIndex;
    private int arrayDirection = 1;
    private bool isPatroling = true;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        currentPoint = patrolPoints[0];

        EventManager.StartListening("chaseStart", StopPatrol);
        EventManager.StartListening("chaseEnd", StartPatrol);

        InvokeRepeating(nameof(UpdatePath), 0f, .01f);
    }

    void UpdatePath()
    {
        if(seeker.IsDone() && isPatroling)
        {
            seeker.StartPath(rb.position, currentPoint.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void Update()
    {
        if (path == null || !isPatroling)
            return;

        if (Vector2.Distance(transform.position, currentPoint.position) < nextWaypointDistance && patrolPoints.Length > 1)
            {
            // change array direction to go route backwards
            if (currentIndex >= patrolPoints.Length - 1 && arrayDirection == 1 || currentIndex <= 0 && arrayDirection == -1)
            {   
                arrayDirection *= -1;
            }
                // switch to next patrolpoint
                currentIndex += arrayDirection;
                currentPoint = patrolPoints[currentIndex];
            }


        if(currentWaypoint >= path.vectorPath.Count)
        {
            // reachedEndOfPath = true;
            return;
        } 
        /**
        else 
        {
            reachedEndOfPath = false;
        }
        */


        // Debug.Log("Path: " + path.vectorPath[currentWaypoint].x + ", Position: " + rb.position.x);
        
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 velocity = direction * speed;
        
        rb.position += velocity * Time.deltaTime;
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        
        if(Math.Abs(path.vectorPath[currentWaypoint].x - rb.position.x) > 0.1f)
        {
            if(velocity.x >= 0.01f)
            {
                transform.localScale = new Vector3(0.6f, 0.6f, 1f);
            } else if (velocity.x <= -0.01f)
            {
                transform.localScale = new Vector3(-0.6f, 0.6f, 1f);
            }
        }

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }


    void StartPatrol(Dictionary<string, object> message)
    {
        if((Rigidbody2D)message["body"] == rb)
        {
            isPatroling = true;

            EventManager.TriggerEvent("patrolStart", new Dictionary<string, object> {
                {"body", rb}
            });
        }
    }

    void StopPatrol(Dictionary<string, object> message)
    {
        if((Rigidbody2D)message["body"] == rb)
        {
            isPatroling = false;

            EventManager.TriggerEvent("patrolStop", new Dictionary<string, object> {
                {"body", rb}
            });
        }
    }

    public void OnDestroy()
    {
        EventManager.StopListening("chaseStart", StopPatrol);
        EventManager.StopListening("chaseEnd", StartPatrol);
    }

    private void OnDrawGizmos()
    {
        for(int i = 0; i < patrolPoints.Length; i++)
        {
            Gizmos.DrawWireSphere(patrolPoints[i].transform.position, 0.1f);
        }
    }
}