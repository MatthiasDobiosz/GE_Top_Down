using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System;

// Basic Logic for the script taken by: https://www.youtube.com/watch?v=jvtFUfJ6CP8

/**
    Logic for an Enemy type that chases the player directly
*/
public class EnemyAIChase : MonoBehaviour
{
    public Transform target;
    public float speed = 2f;
    public float nextWaypointDistance = 0.5f;
    public Transform enemyGFX;
    public float chaseStartDistance = 0.5f;
    public float chaseEndDistance = 3f;


    private Path path;
    private int currentWaypoint = 0;
    //private bool reachedEndOfPath = false;
    private bool currentlyChasing = false;
    private bool currentlyAttacking = false;
    private bool hasLineOfSight = false;
    private bool isPlayerDead = false;

    private Animator anim;
    private Seeker seeker;
    private Rigidbody2D rb;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        InvokeRepeating(nameof(UpdatePath), 0f, .01f);

        EventManager.StartListening("attackStart", DiscontinueChase);
        EventManager.StartListening("attackEnd", ContinueChase);

        EventManager.StartListening("playerDeath", HandlePlayerDeath);
        EventManager.StartListening("playerRespawn", HandlePlayerRespawn);

        EventManager.StartListening("damageTaken", OnAttack);
    }

    void UpdatePath()
    {
        if(seeker.IsDone() && currentlyChasing && !currentlyAttacking)
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
        if(isPlayerDead)
        {
            return;
        }

        if(Vector2.Distance(rb.position, target.position) < chaseStartDistance && hasLineOfSight)
        { 
            StartChase();
        }

        if (path == null || !currentlyChasing || currentlyAttacking)
        {
            return;
        }

        if (Vector2.Distance(rb.position, target.position) > chaseEndDistance)
        {
            EndChase();
        }

        
        if(currentWaypoint >= path.vectorPath.Count)
        {
            //reachedEndOfPath = true;
            return;
        } 
        /**else 
        {
            reachedEndOfPath = false;
        }*/
        

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 velocity = direction * speed;

        rb.position += velocity * Time.deltaTime;
        // rb.AddForce(force);

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

    void FixedUpdate()
    {
        CheckIfPlayerIsInLineOfSight();
    }

    void StartChase()
    {
        if(!currentlyChasing)
        {
            EventManager.TriggerEvent("chaseStart", new Dictionary<string, object> {
                {"body", rb}
            });
            currentlyChasing = true;
        }
    }

    void EndChase()
    {
        EventManager.TriggerEvent("chaseEnd", new Dictionary<string, object> {
            {"body", rb}
        });
        currentlyChasing = false;
    }

    void HandlePlayerDeath(Dictionary<string, object> message = null)
    {
        EndChase();
        isPlayerDead = true;
    }

    void HandlePlayerRespawn(Dictionary<string, object> message = null)
    {
        isPlayerDead = false;
    }

    void CheckIfPlayerIsInLineOfSight()
    {
        RaycastHit2D ray = Physics2D.Linecast(transform.position, target.transform.position, 1 << LayerMask.NameToLayer("Obstacles"));

        if(ray.collider != null)
        {
            hasLineOfSight = false;
            Debug.DrawLine(transform.position, target.transform.position, Color.red);
        } else 
        {
            hasLineOfSight = true;
            Debug.DrawLine(transform.position, target.transform.position, Color.green);
        }
    }

    void DiscontinueChase(Dictionary<string, object> message)
    {
        if((Rigidbody2D)message["body"] == rb)
        {
            currentlyAttacking = true;
        }
    }

    void ContinueChase(Dictionary<string, object> message)
    {
        if((Rigidbody2D)message["body"] == rb)
        {
            currentlyAttacking = false;
        }
    }

    void OnAttack(Dictionary<string, object> message)
    {
        if((GameObject)message["gameobject"] == transform.gameObject)
        {
            StartChase();
        }
    }

    public void OnDestroy()
    {
        EventManager.StopListening("damageTaken", OnAttack);
        EventManager.StopListening("attackStart", DiscontinueChase);
        EventManager.StopListening("attackEnd", ContinueChase);
        EventManager.StopListening("playerDeath", HandlePlayerDeath);
        EventManager.StopListening("playerRespawn", HandlePlayerRespawn);
    }
}