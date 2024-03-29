using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyChase : MonoBehaviour
{
    public Transform target;
    public float speed = 2f;
    public float nextWaypointDistance = 0.5f;
    public Transform enemyGFX;
    public float chaseStartDistance = 0.5f;
    public float chaseEndDistance = 3f;
    public string chaseSoundName = "";

    protected Path path;
    protected int currentWaypoint = 0;
    protected bool currentlyChasing = false;
    protected bool currentlyAttacking = false;
    protected bool hasLineOfSight = false;
    protected bool isPlayerDead = false;
    protected bool isPatroling = true;

    protected Animator anim;
    protected Seeker seeker;
    protected Rigidbody2D rb;
    protected AudioManager audioManager;

    protected virtual void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();

        InvokeRepeating(nameof(UpdatePath), 0f, .01f);

        EventManager.StartListening("attackStart", DiscontinueChase);
        EventManager.StartListening("attackEnd", ContinueChase);

        EventManager.StartListening("playerDeath", HandlePlayerDeath);
        EventManager.StartListening("playerRespawn", HandlePlayerRespawn);

        EventManager.StartListening("damageTaken", OnAttack);

        EventManager.StartListening("enemyStanding", StopPatroling);
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

    protected virtual void Update()
    {
        if(isPlayerDead)
        {
            return;
        }

        if(Vector2.Distance(rb.position, target.position) < chaseStartDistance && hasLineOfSight)
        { 
            if(!currentlyChasing)
            {
                StartChase();
            }
        }
    }

    void FixedUpdate()
    {
        CheckIfPlayerIsInLineOfSight();
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

    void StartChase()
    {
        if(chaseSoundName != "")
        {
            audioManager.Play(chaseSoundName);
        }
        
        currentlyChasing = true;
        EventManager.TriggerEvent("chaseStart", new Dictionary<string, object> {
            {"body", rb}
        });    
    }

    protected virtual void EndChase()
    {
        EventManager.TriggerEvent("chaseEnd", new Dictionary<string, object> {
            {"body", rb}
        });
        currentlyChasing = false;
        isPatroling = true;
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
        if((GameObject)message["gameobject"] == transform.gameObject && !currentlyChasing)
        {
            StartChase();
        }
    }

    void StopPatroling(Dictionary<string, object> message)
    {
        if((Rigidbody2D)message["body"] == rb)
        {
            isPatroling = false;
        }
    }
}
