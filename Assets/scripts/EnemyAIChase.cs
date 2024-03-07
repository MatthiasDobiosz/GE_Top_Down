using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAIChase : MonoBehaviour
{
    public Transform target;
    public float speed = 2f;
    public float nextWaypointDistance = 3f;
    public Transform enemyGFX;
    public float chaseStartDistance = 0.5f;
    public float chaseEndDistance = 3f;


    private Path path;
    private int currentWaypoint = 0;
    private bool reachedEndOfPath = false;
    private bool currentlyChasing = false;

    private Seeker seeker;
    private Rigidbody2D rb;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating(nameof(UpdatePath), 0f, .01f);
    }

    void UpdatePath()
    {
        if(seeker.IsDone() && currentlyChasing)
            seeker.StartPath(rb.position, target.position, OnPathComplete);
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

        if(Vector2.Distance(rb.position, target.position) < chaseStartDistance)
        { 
            EventManager.TriggerEvent("chaseStart", null);
            currentlyChasing = true;
        }

        if (path == null || !currentlyChasing)
            return;

        if (Vector2.Distance(rb.position, target.position) > chaseEndDistance)
        {
            EventManager.TriggerEvent("chaseEnd", null);
            currentlyChasing = false;
        }

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        } else 
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 velocity = direction * speed;

        rb.position += velocity * Time.deltaTime;
        // rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }

        if(velocity.x >= 0.01f)
        {
            enemyGFX.localScale = new Vector3(-1f, 1f, 1f);
        } else if (velocity.x <= -0.01f)
        {
            enemyGFX.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}