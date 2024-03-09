using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAIPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float speed = 2f;
    public float nextWaypointDistance = 0.1f;
    public Transform enemyGFX;

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

        if (Vector2.Distance(transform.position, currentPoint.position) < nextWaypointDistance)
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


        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 velocity = direction * speed;

        rb.position += velocity * Time.deltaTime;
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if(velocity.x >= 0.1f)
        {
            transform.localScale = new Vector3(0.6f, 0.6f, 1f);
        } else if (velocity.x <= -0.1f)
        {
            transform.localScale = new Vector3(-0.6f, 0.6f, 1f);
        }
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
        for(int i = 0; i < patrolPoints.Length; i++)
        {
            Gizmos.DrawWireSphere(patrolPoints[i].transform.position, 0.1f);
        }
    }
}