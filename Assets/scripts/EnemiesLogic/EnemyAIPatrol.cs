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
    public float originalAlignmentX;
    public float originalAlignmentY;

    private Path path;
    private int currentWaypoint = 0;
    // private bool reachedEndOfPath = false;

    private Seeker seeker;
    private Rigidbody2D rb;
    private Transform currentPoint;
    private Animator anim;

    private int currentIndex;
    private int arrayDirection = 1;
    private bool isPatroling = true;

    private Vector2 originalPosition;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        currentPoint = patrolPoints[0];
        originalPosition = rb.position;

        EventManager.StartListening("chaseStart", StopPatrol);
        EventManager.StartListening("chaseEnd", StartPatrol);

        InvokeRepeating(nameof(UpdatePath), 0f, .1f);
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
        } else if(Vector2.Distance(transform.position, currentPoint.position) < nextWaypointDistance && patrolPoints.Length == 1)
        {
            ResetEnemyToStanding();
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
            if(Math.Abs(velocity.y) < 0.1f)
            {   
                anim.SetFloat("YInput", 0);
            }
            anim.SetFloat("XInput", velocity.x);
        }
        if(Math.Abs(path.vectorPath[currentWaypoint].y - rb.position.y) > 0.1f)
        {
            if(Math.Abs(velocity.x) < 0.1f)
            {   
                anim.SetFloat("XInput", 0);
            }
            anim.SetFloat("YInput", velocity.y);
        }

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    void ResetEnemyToStanding()
    {
        isPatroling = false;
        rb.position = originalPosition;
        anim.SetFloat("XInput", originalAlignmentX);
        anim.SetFloat("YInput", originalAlignmentY);
        anim.SetBool("Moving", false);

        EventManager.TriggerEvent("enemyStanding", new Dictionary<string, object> {
            {"body", rb}
        });
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